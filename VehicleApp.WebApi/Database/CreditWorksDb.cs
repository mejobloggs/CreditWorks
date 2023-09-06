using VehicleApp.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace VehicleApp.WebApi.Database;

public class VehicleAppDb : DbContext
{
    public VehicleAppDb(DbContextOptions<VehicleAppDb> options)
        : base(options) { }

    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<VehicleView> VehiclesView { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<VehicleView>()
            .ToView("VehiclesView")
            .HasKey(v => v.Id);
    }
}
