namespace VehicleApp.WebApi.Entities;

public interface ICategory
{
    int Id { get; set; }
    string ImageName { get; set; }
    decimal? MaxKg { get; set; }
    decimal MinKg { get; set; }
    string Name { get; set; }
    byte[] RowVersion { get; set; }
}