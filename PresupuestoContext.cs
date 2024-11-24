using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using NetDynamicPress.Models;

namespace NetDynamicPress.Context;

public class PresupuestoContext : DbContext
{
    public DbSet<User> Users { get;set; }
    public DbSet<Presupuesto> Presupuestos { get;set; }
    public DbSet<Recibo> Recibos { get; set; }

    private readonly IConfiguration _configuration;

    public PresupuestoContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(_configuration.GetConnectionString("Presupuestos"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>() // Add unique constraint
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>() // Makes it not null
            .Property(u => u.Email)
            .IsRequired();
    }

}


public class DatabaseInitializer
    {
        private readonly PresupuestoContext _context;

        public DatabaseInitializer(PresupuestoContext context)
        {
            _context = context;
        }

        public void Initialize()
        {
            try
            {
                _context.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error al crear la base de datos: {ex.Message}");
                throw;
            }
        }
    }