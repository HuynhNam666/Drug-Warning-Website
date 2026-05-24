using MedicineWarningAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicineWarningAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Admin> Admins => Set<Admin>();
        public DbSet<Medicine> Medicines => Set<Medicine>();
        public DbSet<Disease> Diseases => Set<Disease>();
        public DbSet<MedicineWarning> MedicineWarnings => Set<MedicineWarning>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("Admins");
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.Username).IsUnique();

                entity.Property(x => x.Username)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.PasswordHash)
                    .HasMaxLength(256)
                    .IsRequired();

                entity.Property(x => x.FullName)
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Medicine>(entity =>
            {
                entity.ToTable("Medicines");
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.Name).IsUnique();

                entity.Property(x => x.Name)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(x => x.Description)
                    .HasMaxLength(1000);

                entity.Property(x => x.Usage)
                    .HasMaxLength(1000);

                entity.Property(x => x.SideEffects)
                    .HasMaxLength(1000);
            });

            modelBuilder.Entity<Disease>(entity =>
            {
                entity.ToTable("Diseases");
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.Name).IsUnique();

                entity.Property(x => x.Name)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(x => x.Description)
                    .HasMaxLength(1000);
            });

            modelBuilder.Entity<MedicineWarning>(entity =>
            {
                entity.ToTable("MedicineWarnings");
                entity.HasKey(x => x.Id);

                entity.HasIndex(x => new { x.MedicineId, x.DiseaseId })
                    .IsUnique();

                entity.Property(x => x.RiskLevel)
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(x => x.WarningContent)
                    .HasMaxLength(2000)
                    .IsRequired();

                entity.Property(x => x.Recommendation)
                    .HasMaxLength(2000);

                entity.HasOne(x => x.Medicine)
                    .WithMany(x => x.MedicineWarnings)
                    .HasForeignKey(x => x.MedicineId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Disease)
                    .WithMany(x => x.MedicineWarnings)
                    .HasForeignKey(x => x.DiseaseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}