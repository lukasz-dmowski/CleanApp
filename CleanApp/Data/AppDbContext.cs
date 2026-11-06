using CleanApp.Data.Entities;
using CleanApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CleanApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<EnterpriseSQL> Enterprises { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OrderSQL> Orders { get; set; }
        public DbSet<ItemSQL> Items { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EnterpriseSQL>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                      .IsRequired();

                //1 enterprise holds N orders
                entity.HasMany(e => e.Orders)
                      .WithOne(o => o.Enterprise)
                      .HasForeignKey(o => o.EnterpriseId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Username)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(u => u.PasswordHash)
                      .IsRequired();

                entity.Property(u => u.IsAdmin)
                      .IsRequired();
            });

            modelBuilder.Entity<OrderSQL>(entity =>
            {
                entity.HasKey(o => o.Id);

                //1 order holds N items
                entity.HasMany(o => o.Items)
                      .WithOne(i => i.Order)
                      .HasForeignKey(i => i.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ItemSQL>(entity =>
            {
                entity.HasKey(i => i.Id);
            });
        }
    }
}
