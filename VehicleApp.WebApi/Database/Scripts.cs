namespace VehicleApp.WebApi.Database;

public class Scripts
{
    public const string CreateVehiclesView = @"
                CREATE VIEW [dbo].[VehiclesView] AS
                SELECT
                    v.Id,
                    v.OwnerName,
                    v.ManufactureYear,
                    v.WeightInKg,
                    m.Name AS Manufacturer,
                    c.Name AS Category,
                    c.ImageName AS CategoryImage
                FROM [dbo].[Vehicles] v
                    JOIN [dbo].[Manufacturers] m ON m.Id = v.ManufacturerId
                    JOIN [dbo].[Categories] c ON v.WeightInKg BETWEEN c.MinKg AND ISNULL(c.MaxKg, v.WeightInKg);";
}
