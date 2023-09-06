using VehicleApp.WebApi.Database;
using VehicleApp.WebApi.Entities;
using Microsoft.EntityFrameworkCore;
using VehicleApp.WebApi.Validators;
using VehicleApp.WebApi.DataHelpers;
using VehicleApp.WebApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<VehicleAppDb>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnectionString")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddSingleton<CategoryChangeValidator>();
builder.Services.AddSingleton<CategoryDataHelper>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<VehicleAppDb>();
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    DbInitializer.Init(context);
}

RouteGroupBuilder categories = app.MapGroup("/categories");
categories.MapGet("/", GetAllCategories);
categories.MapPatch("/", UpdateCategories);

RouteGroupBuilder vehicles = app.MapGroup("/vehicles");
vehicles.MapGet("/", GetVehiclesView);
vehicles.MapGet("/{id}", GetVehicle);
vehicles.MapPost("/", CreateVehicle);
vehicles.MapPut("/", UpdateVehicle);
vehicles.MapDelete("/", DeleteVehicle);

app.Run();

static async Task<IResult> GetAllCategories(VehicleAppDb db)
{
    return TypedResults.Ok(await db.Categories.OrderBy(c => c.MinKg).ToListAsync());
}

static async Task<IResult> UpdateCategories(IReadOnlyList<VehicleApp.WebApi.Models.Category> categories,
    VehicleAppDb db, CategoryDataHelper categoryDataHelper, CategoryChangeValidator changeValidator)
{
    var validation = changeValidator.IsValid(categories);

    if (!validation.IsValid)
    {
        return TypedResults.BadRequest(validation.ErrorMessage);
    }

    try
    {
        await categoryDataHelper.Update(categories, db);
    }
    catch (DbUpdateConcurrencyException)
    {
        return TypedResults.BadRequest("The categories you tried to save are not the latest. Please reload and try again");
    }

    return TypedResults.NoContent();
}

static async Task<IResult> GetVehiclesView(VehicleAppDb db)
{
    return TypedResults.Ok(await db.VehiclesView.ToListAsync());
}

static async Task<IResult> GetVehicle(int id, VehicleAppDb db)
{
    return await db.Vehicles.FindAsync(id)
        is Vehicle vehicle
        ? TypedResults.Ok(vehicle)
        : TypedResults.NotFound();
}

static async Task<IResult> CreateVehicle(Vehicle vehicle, VehicleAppDb db)
{
    db.Vehicles.Add(vehicle);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/vehicles/{vehicle.Id}", vehicle);
}

static async Task<IResult> UpdateVehicle(int id, Vehicle vehicle, VehicleAppDb db)
{
    var dbVehicle = await db.Vehicles.FindAsync(id);
    if (dbVehicle is null) return TypedResults.NotFound();

    dbVehicle.Update(vehicle);

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> DeleteVehicle(int id, VehicleAppDb db)
{
    if (await db.Vehicles.FindAsync(id) is Vehicle vehicle)
    {
        db.Vehicles.Remove(vehicle);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}