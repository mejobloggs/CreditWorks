using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VehicleApp.WebApi.Models;

namespace VehicleApp.WebApi.Entities;

public class VehicleView
{
    public int Id { get; set; }

    [MaxLength(100)]
    public required string OwnerName { get; set; }
    public required ushort ManufactureYear { get; set; }

    [Precision(7, 2)]
    public required decimal WeightInKg { get; set; }

    [MaxLength(50)]
    public required string Manufacturer { get; set; }

    [MaxLength(50)]
    public required string Category { get; set; }

    [MaxLength(50)]
    public required string CategoryImage { get; set; }
}
