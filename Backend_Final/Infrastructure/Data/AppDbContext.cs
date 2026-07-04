using Backend_Final.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend_Final.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>() //Email unico
            .HasIndex(c => c.Email)
            .IsUnique();
        }
    }
}
