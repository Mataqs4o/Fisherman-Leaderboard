using Fisherman_Board.Models;
using Microsoft.EntityFrameworkCore;

namespace Fisherman_Board.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Fisherman> Fisherman => Set<Fisherman>();

        public DbSet<Boat> Boats => Set<Boat>();

        public DbSet<Hunt> Hunt => Set<Hunt>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Fisherman>(entity =>
            {
                entity.ToTable("Fisherman");
                entity.HasIndex(item => item.LicenseNumber).IsUnique();
            });

            modelBuilder.Entity<Boat>(entity =>
            {
                entity.ToTable("Boats");
                entity.HasIndex(item => item.RegistrationNumber).IsUnique();
                entity.HasOne(item => item.Fisherman)
                    .WithMany(item => item.Boats)
                    .HasForeignKey(item => item.FishermanId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Hunt>(entity =>
            {
                entity.ToTable("Hunt");
                entity.HasOne(item => item.Fisherman)
                    .WithMany(item => item.Hunts)
                    .HasForeignKey(item => item.FishermanId)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(item => item.Boat)
                    .WithMany(item => item.Hunts)
                    .HasForeignKey(item => item.BoatId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
