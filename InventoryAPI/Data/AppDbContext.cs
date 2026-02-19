using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Inventory> Inventory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.ToTable("Inventory");

                entity.HasKey(e => e.VehicleId);

                entity.Property(e => e.VehicleId)
                      .HasColumnName("VehicleId");

                entity.Property(e => e.Make)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.Model)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.Year)
                      .IsRequired();

                entity.Property(e => e.DailyRate)
                      .HasColumnType("decimal(10,2)")
                      .IsRequired();

                entity.Property(e => e.IsAvailable)
                      .IsRequired();
            });
        }
    }
}
