using Microsoft.EntityFrameworkCore;
using PaymentService.Infra.Models;

namespace PaymentService.Infra
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Payment>((entity) =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.TotalAmount).HasPrecision(18, 4);
                entity.ToTable("Payments");
            });

            builder.Entity<OutboxMessage>((entity) =>
            {
                entity.HasKey(p => p.Id);
                entity.ToTable("OutboxMessages"); 
            });

            base.OnModelCreating(builder);
        }

    }
}
