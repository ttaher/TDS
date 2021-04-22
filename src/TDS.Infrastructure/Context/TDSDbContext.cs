using Microsoft.EntityFrameworkCore;
using TDS.Infrastructure.Models;

namespace TDS.Infrastructure.Data.Context
{
    public class TDSDbContext : DbContext
    {

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public TDSDbContext(DbContextOptions<TDSDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Models.Account>(x =>
            {
                x.HasIndex(i => new
                {
                    i.Address
                }).IsUnique();
            });
            modelBuilder.Entity<Models.Payment>(x =>
            {
                x.HasIndex(i => new
                {
                    i.PaymentId
                }).IsUnique();
            });

        }
    }
}
