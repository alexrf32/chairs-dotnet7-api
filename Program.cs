using chairs_dotnet7_api;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("chairlist"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

var chairs = app.MapGroup("api/chair");

//TODO: ASIGNACION DE RUTAS A LOS ENDPOINTS
chairs.MapGet("/", GetChairs);
chairs.MapGet("/{name}",GetChairByName);
chairs.MapPost("/",CreateChair);
chairs.MapPost("/{purchchase}",PurchaseChair);
chairs.MapPut("/{id}",UpdateChair)
chairs.MapPut("/{id} stock",InStockChair);
chair.MapDelete("/{id}",DeleteChair);

app.Run();

//TODO: ENDPOINTS SOLICITADOS
static IResult GetChairs(DataContext db)
{
    var chairList = db.Chairs.ToList();
    return TypedResults.Ok();
}
static IResult CreateChair(DataContext db)
{
    var existingChair = db.Chairs.FirstOrDefault(c => c.Name == chair.Name);
    if (existingChair != null)
    {
        return Results.BadRequest("Ya existe una silla con el nombre '{chair.Name}'.");
    }
    db.Chairs.Add(chair);
    db.SaveChanges();
    return Results.Created($"/api/chair/{chair.Name}", chair);
}
static IResult GetChairByName(DataContext db)
{
    var chair = db.Chairs.FirstOrDefault(c => c.Name == name);
    if (chair == null)
    {
        return Results.NotFound($"No se encontró la silla con el nombre'{name}'.");
    }
    return TypedResults.Ok(chair);
}

 static IResult PurchaseChair(DataContext db, int id, int quantity)
{
    var chair = db.Chairs.Find(id);
    if (chair == null)
    {
        return Results.NotFound($"No se encontró la silla con el ID '{id}'.");
    }

    if (chair.Stock < quantity)
    {
        return Results.BadRequest($"No hay suficientes unidades de la silla '{chair.Name}' en stock.");
    }

    chair.Stock = quantity;
    db.SaveChanges();

    return TypedResults.Ok();
}
static IResult UpdateChair(DataContext db)
{
    var existingChair = db.Chairs.Find(id);
    if (existingChair == null){
        return Results.NotFound($"No se encontró la silla con el ID '{id}'.");
    }
    existingChair = updatedChair;
    db.SaveChanges();
    return Results.Ok(existingChair);
}
static IResult InStockChair(DataContext db, int id, int stock)
{
    var existingChair = db.Chairs.Find(id);
    if (existingChair == null)
    {
        return Results.NotFound($"No se encontró silla con el ID '{id}'.");
    }

    existingChair.Stock = stock;
    db.SaveChanges();
    return TypedResults.Ok();
}
static IResult DeleteChair(DataContext db)
{
    var existingChair = db.Chairs.Find(id);
    if (existingChair == null)
    {
        return Results.NotFound($"No se encontró la silla con el ID '{id}'.");
    }
    db.Chairs.Remove(existingChair);
    db.SaveChanges();
    return TypedResults.Ok();
}
