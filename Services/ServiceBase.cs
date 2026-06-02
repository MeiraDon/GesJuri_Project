using GesCPSI_Project.Data;
using GesCPSI_Project.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GesCPSI_Project.Services
{
    public class ServiceBase<T> : ICrud<T> where T : class
    {
        protected readonly GesDbContext _context;
        protected IQueryable<T> Entities => _context.Set<T>();

        public ServiceBase(GesDbContext context)
        {
            this._context = context;
        }



        public async Task<Result<T>> AddAsync(T entity)
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return Result<T>.Success(entity);
            }
            catch (Exception ex)
            {
                return Result<T>.Failure($"Erreur lors de l'ajout : {ex.Message}");
            }
        }


        public virtual async Task<Result<T>> UpdateAsync(T entity)
        {
            try
            {
                var entityType = _context.Model.FindEntityType(typeof(T));
                if (entityType == null)
                    return Result<T>.Failure("Type d'entité introuvable dans le modèle EF.");

                var primaryKey = entityType.FindPrimaryKey();
                if (primaryKey == null)
                    return Result<T>.Failure("Clé primaire introuvable.");

                var keyProperty = primaryKey.Properties.First();
                var keyName = keyProperty.Name;

                var entityKeyValue = typeof(T).GetProperty(keyName)?.GetValue(entity);

                // Chercher une instance déjà trackée avec la même clé
                var trackedEntity = _context.ChangeTracker.Entries<T>()
                    .FirstOrDefault(e =>
                    {
                        var trackedValue = typeof(T).GetProperty(keyName)?.GetValue(e.Entity);
                        return trackedValue != null && trackedValue.Equals(entityKeyValue);
                    });

                if (trackedEntity != null)
                {
                    trackedEntity.State = EntityState.Detached;
                }

                _context.Set<T>().Update(entity);
                await _context.SaveChangesAsync();

                return Result<T>.Success(entity);
            }
            catch (Exception ex)
            {
                return Result<T>.Failure($"Erreur lors de la mise à jour : {ex.Message}");
            }
        }


        public async Task<Result<T>> DeleteAsync(T entity)
        {
            try
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
                return Result<T>.Success(entity);
            }
            catch (Exception ex)
            {
                return Result<T>.Failure($"Erreur lors de la suppression : {ex.Message}");
            }
        }
        public async Task<Result<T>> DeleteByIdAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                return Result<T>.Failure("Élément introuvable");

            return await DeleteAsync(entity);
        }



        //public async Task<T?> GetByIdAsync(int id)
        //{
        //    var entity = await _context.Set<T>().FindAsync(id);

        //    if (entity != null)
        //        _context.Entry(entity).State = EntityState.Detached; // ✅ important

        //    return entity;
        //}

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);

            if (entity != null)
                _context.Entry(entity).State = EntityState.Detached;

            return entity;
        }


        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>()
        .AsNoTracking()
        .ToListAsync();
        }
    }
}
