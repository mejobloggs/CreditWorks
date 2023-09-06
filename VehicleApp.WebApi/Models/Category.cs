namespace VehicleApp.WebApi.Models;

public class Category : Entities.ICategory
{
    public int Id { get; set; }
    public required string ImageName { get; set; }
    public decimal? MaxKg { get; set; }
    public required decimal MinKg { get; set; }
    public required string Name { get; set; }
    public bool Delete { get; set; }
    public byte[] RowVersion { get; set; }
}
