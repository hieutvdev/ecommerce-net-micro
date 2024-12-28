using System.Reflection;
using Inventory.Application.Data;
using Inventory.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Inventory.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();

    public DbSet<WarehouseProducts> WarehouseProducts => Set<WarehouseProducts>();

    public DbSet<Suppliers> Suppliers => Set<Suppliers>();

    public DbSet<ProductSuppliers> ProductSuppliers => Set<ProductSuppliers>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseNpgsql("YourConnectionString")  
                .EnableSensitiveDataLogging()  
                .LogTo(Console.WriteLine, LogLevel.Information); 
        }
    }


}