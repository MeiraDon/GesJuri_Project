using GesCPSI_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace GesCPSI_Project.Data
{
    public class GesDbContext : DbContext
    {
        public GesDbContext(DbContextOptions<GesDbContext> options) : base(options) { }

        public DbSet<AjoutActModel> AjoutActModels { get; set; }
        public DbSet<AutorisationModel> AutorisationModels { get; set; }
        public DbSet<BanqueModel> BanqueModels { get; set; }
        public DbSet<CautionnementModel> CautionnementModels { get; set; }
        public DbSet<ChargeModel> ChargeModels { get; set; }
        public DbSet<ClientModel> ClientModels { get; set; }
        public DbSet<ClientActModel> ClientActModels { get; set; }
        public DbSet<EntiteJurModel> EntiteJurModels { get; set; }
        public DbSet<JournalisationModel> JournalisationModels { get; set; }
        public DbSet<PretModel> PretModels { get; set; }
        public DbSet<RolesClientActModel> RolesClientActModels { get; set; }
        public DbSet<TypesActModel> TypesActModels { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TypesActe -> User (1 user peut créer plusieurs actes)
            modelBuilder.Entity<TypesActModel>()
                .HasOne(t => t.UserModel)
                .WithMany(u => u.TypesActModels)
                .HasForeignKey(t => t.IdUser)
                .OnDelete(DeleteBehavior.Restrict);

            // TypesActe -> Banque (1 banque, plusieurs actes)
            modelBuilder.Entity<TypesActModel>()
                .HasOne(t => t.BanqueModel)
                .WithMany(b => b.TypesActModels)
                .HasForeignKey(t => t.IdBnq)
                .OnDelete(DeleteBehavior.Restrict);

            // Journalisation -> TypesActe (historique)
            modelBuilder.Entity<JournalisationModel>()
                .HasOne(h => h.TypesActModel)
                .WithMany(a => a.JournalisationModels)
                .HasForeignKey(h => h.IdActe)
                .OnDelete(DeleteBehavior.Cascade);

            // ClientActe : relation Client <-> TypesActe avec Role
            modelBuilder.Entity<ClientActModel>()
                .HasOne(ca => ca.ClientModel)
                .WithMany(c => c.ClientActModels)
                .HasForeignKey(ca => ca.IdClt)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClientActModel>() /*Client ⇄ ClientActe ⇄ TypesActe + Role*/
                .HasOne(ca => ca.TypesActModel)
                .WithMany(a => a.ClientActModels)
                .HasForeignKey(ca => ca.IdActe)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClientActModel>()
                .HasOne(ca => ca.RolesClientActModel)
                .WithMany(r => r.ClientActModels)
                .HasForeignKey(ca => ca.IdRole)
                .OnDelete(DeleteBehavior.Restrict);

            // Index utiles
            modelBuilder.Entity<TypesActModel>()
                .HasIndex(t => t.IdActe);

            modelBuilder.Entity<ClientModel>()
                .HasIndex(c => c.NomRaisonsociale);

            modelBuilder.Entity<UserModel>()
                .HasIndex(u => u.NameUser);
        }
    }
}
