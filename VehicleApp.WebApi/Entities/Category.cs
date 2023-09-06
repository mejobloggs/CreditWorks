using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace VehicleApp.WebApi.Entities;

[Index(nameof(MinKg), IsUnique = true)]
[Index(nameof(MaxKg), IsUnique = true)]
[Index(nameof(Name), IsUnique = true)]
public class Category : ICategory
{
    public int Id { get; set; }
    [MaxLength(50)]
    public required string Name { get; set; }

    [Precision(7, 2)]
    public required decimal MinKg { get; set; }

    [Precision(7, 2)]
    public decimal? MaxKg { get; set; }

    [MaxLength(50)]
    public required string ImageName { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; }

    public Category()
    {
    }

    [SetsRequiredMembers]
    public Category(Models.Category update) =>
       (Id, Name, MinKg, MaxKg, ImageName) = (update.Id, update.Name, update.MinKg, update.MaxKg, update.ImageName);

    public void Update(Models.Category update) =>
        (Name, MinKg, MaxKg, ImageName) = (update.Name, update.MinKg, update.MaxKg, update.ImageName);
}