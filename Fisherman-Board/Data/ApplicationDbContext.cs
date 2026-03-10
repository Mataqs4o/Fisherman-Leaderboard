using Fisherman_Board.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fisherman_Board.Data


{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FishingVessel> FishingVessels { get; set; }
        public DbSet<Engine> Engines { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<VesselOwner> VesselOwners { get; set; }
        public DbSet<FishingPermit> FishingPermits { get; set; }
        public DbSet<FishingGear> FishingGears { get; set; }
        public DbSet<FishingTrip> FishingTrips { get; set; }
        public DbSet<CatchRecord> CatchRecords { get; set; }
        public DbSet<RecreationalTicket> RecreationalTickets { get; set; }
        public DbSet<RecreationalCatch> RecreationalCatches { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<VesselOwner>()
                .HasKey(vo => new { vo.FishingVesselId, vo.PersonId });

            builder.Entity<VesselOwner>()
                .HasOne(vo => vo.FishingVessel)
                .WithMany(v => v.Owners)
                .HasForeignKey(vo => vo.FishingVesselId);

            builder.Entity<VesselOwner>()
                .HasOne(vo => vo.Person)
                .WithMany(p => p.OwnedVessels)
                .HasForeignKey(vo => vo.PersonId);

            builder.Entity<FishingPermit>()
                .HasOne(fp => fp.Owner)
                .WithMany()
                .HasForeignKey(fp => fp.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FishingPermit>()
                .HasOne(fp => fp.Captain)
                .WithMany(p => p.CaptainPermits)
                .HasForeignKey(fp => fp.CaptainId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
