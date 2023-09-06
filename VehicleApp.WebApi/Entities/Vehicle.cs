using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;

namespace VehicleApp.WebApi.Entities;

public class Vehicle
{
    public int Id { get; set; }

    [MaxLength(100)]
    public required string OwnerName { get; set; }
    public required ushort ManufactureYear { get; set; }

    [Precision(7, 2)]
    public required decimal WeightInKg { get; set; }

    public int ManufacturerId { get; set; }
    public Manufacturer Manufacturer { get; set; }
    public void Update(Vehicle update) =>
        (Id, OwnerName, ManufactureYear, WeightInKg, ManufacturerId)
        = (update.Id, update.OwnerName, update.ManufactureYear, update.WeightInKg, update.ManufacturerId);
}
