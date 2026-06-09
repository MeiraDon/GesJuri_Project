using GesCPSI_Project.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GesCPSI_Project.Data
{
    public class GesDbContext : IdentityDbContext<UserModel, UserRole, int>
    {
        public GesDbContext(DbContextOptions<GesDbContext> options) : base(options) { }

        public DbSet<AjoutActModel> AjoutActModels { get; set; } = default!;
        public DbSet<AutorisationModel> AutorisationModels { get; set; } = default!;
        public DbSet<BanqueModel> BanqueModels { get; set; } = default!;
        public DbSet<CautionnementModel> CautionnementModels { get; set; } = default!;
        public DbSet<ChargeModel> ChargeModels { get; set; } = default!;
        public DbSet<ClientModel> ClientModels { get; set; } = default!;
        public DbSet<ClientActModel> ClientActModels { get; set; } = default!;
        public DbSet<EntiteJurModel> EntiteJurModels { get; set; } = default!;
        public DbSet<JournalisationModel> JournalisationModels { get; set; } = default!;
        public DbSet<PretModel> PretModels { get; set; } = default!;
        public DbSet<RolesClientActModel> RolesClientActModels { get; set; } = default!;
        public DbSet<TypesActModel> TypesActModels { get; set; } = default!;
        public DbSet<AuditLogModel> AuditLogs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== Renommage des tables Identity en français =====
            modelBuilder.Entity<UserModel>().ToTable("Users");
            modelBuilder.Entity<UserRole>().ToTable("Roles");

            // ===== TypesActe -> User (1 user peut créer plusieurs actes) =====
            modelBuilder.Entity<TypesActModel>()
                .HasOne(t => t.UserModel)
                .WithMany(u => u.ActesSaisis)  // 🔧 ancien : TypesActModels → nouveau : ActesSaisis
                .HasForeignKey(t => t.IdUser)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== TypesActe -> Banque (1 banque, plusieurs actes) =====
            modelBuilder.Entity<TypesActModel>()
                .HasOne(t => t.BanqueModel)
                .WithMany(b => b.TypesActModels)
                .HasForeignKey(t => t.IdBnq)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== Journalisation -> TypesActe (historique) =====
            modelBuilder.Entity<JournalisationModel>()
                .HasOne(h => h.TypesActModel)
                .WithMany(a => a.JournalisationModels)
                .HasForeignKey(h => h.IdActe)
                .OnDelete(DeleteBehavior.Cascade);

            // ===== ClientActe : relation Client <-> TypesActe avec Role =====
            modelBuilder.Entity<ClientActModel>()
                .HasOne(ca => ca.ClientModel)
                .WithMany(c => c.ClientActModels)
                .HasForeignKey(ca => ca.IdClt)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClientActModel>()
                .HasOne(ca => ca.TypesActModel)
                .WithMany(a => a.ClientActModels)
                .HasForeignKey(ca => ca.IdActe)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClientActModel>()
                .HasOne(ca => ca.RolesClientActModel)
                .WithMany(r => r.ClientActModels)
                .HasForeignKey(ca => ca.IdRole)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== Index utiles =====
            modelBuilder.Entity<TypesActModel>()
                .HasIndex(t => t.IdActe);

            modelBuilder.Entity<ClientModel>()
                .HasIndex(c => c.NomRaisonsociale);

            // 🔧 IMPORTANT : Identity crée déjà des index sur UserName et NormalizedUserName.
            //    Ton ancien index manuel sur NameUser fait doublon → on le SUPPRIME.
            // modelBuilder.Entity<UserModel>()
            //     .HasIndex(u => u.NameUser);  // ❌ Supprimé : Identity gère ça nativement
        }
    }
}
