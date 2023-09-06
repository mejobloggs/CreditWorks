using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace VehicleApp.WebApi.Entities;

public class Manufacturer
{
    public int Id { get; set; }
    [MaxLength(50)]
    public required string Name { get; set; }
    public ICollection<Vehicle> Vehicles { get; }

}
