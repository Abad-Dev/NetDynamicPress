using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using NetDynamicPress.Models;

namespace NetDynamicPress.Context;

public class PresupuestoContext : DbContext
{
    public DbSet<User> Users { get;set; }
    public DbSet<Presupuesto> Presupuestos { get;set; }
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
        User user = new()
        {
            Name = "Jose Perez",
            TopImage = "",
            Signature = ""
        };

        Presupuesto presupuesto = new()
        {
            Name = "Presupuesto1",
            UserId = user.Id,
            Config = "{}"
        };

        modelBuilder.Entity<User>().HasData(user);
        modelBuilder.Entity<Presupuesto>().HasData(presupuesto);

        base.OnModelCreating(modelBuilder);
    }

}