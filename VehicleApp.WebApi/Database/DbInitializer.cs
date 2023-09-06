using VehicleApp.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace VehicleApp.WebApi.Database;

public static class DbInitializer
{
    public static void Init(VehicleAppDb db)
    {
        if (db.Manufacturers.Any())
        {
            return; //already seeded
        }

        var manufacturers = new Manufacturer[]
        {
            new Manufacturer { Name = "Mazda" },
            new Manufacturer { Name = "Mercedes" },
            new Manufacturer { Name = "Honda" },
            new Manufacturer { Name = "Ferrari" },
            new Manufacturer { Name = "Toyota" },
        };

        var categories = new Category[]
        {
            new Category { Name = "Light", MinKg = 0, MaxKg = 499.99M, ImageName = "light.png" },
            new Category { Name = "Medium", MinKg = 500, MaxKg = 2499.99M, ImageName = "medium.png" },
            new Category { Name = "Heavy", MinKg = 2500, ImageName = "heavy.png" }
        };

        db.Manufacturers.AddRange(manufacturers);
        db.Categories.AddRange(categories);
        db.SaveChanges();

        var vehicles = new Vehicle[]
        {
            new Vehicle { OwnerName = "A", ManufactureYear = 2009, WeightInKg = 0, ManufacturerId = 1 },
            new Vehicle { OwnerName = "B", ManufactureYear = 2000, WeightInKg = 499.99M, ManufacturerId = 5 },
            new Vehicle { OwnerName = "C", ManufactureYear = 2010, WeightInKg = 500, ManufacturerId = 3 },
            new Vehicle { OwnerName = "D", ManufactureYear = 1990, WeightInKg = 2499.99M, ManufacturerId = 4 },
            new Vehicle { OwnerName = "A", ManufactureYear = 1999, WeightInKg = 2500, ManufacturerId = 5 },
            new Vehicle { OwnerName = "A", ManufactureYear = 2022, WeightInKg = 99999.99M, ManufacturerId = 2 },
        };
        db.Vehicles.AddRange(vehicles);
        db.SaveChanges();

        using(var cmd = db.Database.GetDbConnection().CreateCommand())
        {
            cmd.CommandText = Scripts.CreateVehiclesView;
            db.Database.OpenConnection();
            cmd.ExecuteNonQuery();
        }
    }
}
