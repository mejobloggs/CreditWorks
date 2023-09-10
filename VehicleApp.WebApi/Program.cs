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
builder.Services.AddScoped<CategoryDataHelper>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<VehicleAppDb>();
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated();

    DbInitializer.Init(context);
}


RouteGroupBuilder manufacturers = app.MapGroup("/manufacturers");
manufacturers.MapGet("/", GetAllManufacturers);

RouteGroupBuilder categories = app.MapGroup("/categories");
categories.MapGet("/", GetAllCategories);
categories.MapPut("/", UpdateCategories);

RouteGroupBuilder vehicles = app.MapGroup("/vehicles");
vehicles.MapGet("/", GetVehiclesView);
vehicles.MapGet("/{id}", GetVehicle);
vehicles.MapPost("/", CreateVehicle);
vehicles.MapPut("/{id}", UpdateVehicle);
vehicles.MapDelete("/{id}", DeleteVehicle);

app.Run();

static async Task<IResult> GetAllManufacturers(VehicleAppDb db)
{
    return Results.Ok(await db.Manufacturers.AsNoTracking().ToListAsync());
}
static async Task<IResult> GetAllCategories(VehicleAppDb db)
{
    return Results.Ok(await db.Categories.AsNoTracking().OrderBy(c => c.MinKg).ToListAsync());
}

static async Task<IResult> UpdateCategories(IReadOnlyList<VehicleApp.WebApi.Models.Category> categories,
    VehicleAppDb db, CategoryDataHelper categoryDataHelper, CategoryChangeValidator changeValidator)
{
    var validation = changeValidator.IsValid(categories);

    if (!validation.IsValid)
    {
        return Results.BadRequest(validation.ErrorMessage);
    }

    try
    {
        await categoryDataHelper.Update(categories);
    }
    catch (DbUpdateConcurrencyException)
    {
        return Results.BadRequest("The categories you tried to save are not the latest. Please reload and try again");
    }

    return Results.Ok(await db.Categories.AsNoTracking().OrderBy(c => c.MinKg).ToListAsync());
}

static async Task<IResult> GetVehiclesView(VehicleAppDb db)
{
    return Results.Ok(await db.VehiclesView.AsNoTracking().ToListAsync());
}

static async Task<IResult> GetVehicle(int id, VehicleAppDb db)
{
    return await db.Vehicles.FindAsync(id)
        is Vehicle vehicle
        ? Results.Ok(vehicle)
        : Results.NotFound();
}

static async Task<IResult> CreateVehicle(Vehicle vehicle, VehicleAppDb db)
{
    db.Vehicles.Add(vehicle);
    await db.SaveChangesAsync();

    return Results.Created($"/vehicles/{vehicle.Id}", vehicle);
}

static async Task<IResult> UpdateVehicle(int id, Vehicle vehicle, VehicleAppDb db)
{
    var dbVehicle = await db.Vehicles.FindAsync(id);
    if (dbVehicle is null) return Results.NotFound();

    dbVehicle.Update(vehicle);

    await db.SaveChangesAsync();

    return Results.NoContent();
}

static async Task<IResult> DeleteVehicle(int id, VehicleAppDb db)
{
    if (await db.Vehicles.FindAsync(id) is Vehicle vehicle)
    {
        db.Vehicles.Remove(vehicle);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
}