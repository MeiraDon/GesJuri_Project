using GesCPSI_Project.Services;

namespace GesCPSI_Project.Interfaces
{
    public interface ICrud <T> where T : class
    {
        Task<Result<T>> AddAsync(T entity);
        Task<Result<T>> UpdateAsync(T entity);
        Task<Result<T>> DeleteAsync(T entity);
        Task<Result<T>> DeleteByIdAsync(int id);
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();

    }
}
