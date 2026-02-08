using CarRental.Api;
using CarRental.Api.Data;
using CarRental.Api.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IRentalService, RentalService>();

var connectionString = builder.Configuration.GetConnectionString("Database");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.UseSqlite(connectionString);
    }
    else
    {
        options.UseNpgsql(connectionString);
    }
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Migraciones en todos los entornos (necesario para Render/PostgreSQL)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        db.Database.Migrate();
    }
    catch (PostgresException ex) when (ex.SqlState == "42P07")
    {
        // Tabla ya existe (p. ej. deploy anterior parcial) - registrar migración aplicada
        db.Database.ExecuteSqlRaw(
            "INSERT INTO \"__EFMigrationsHistory\" (\"MigrationId\", \"ProductVersion\") VALUES ('20260208090429_InitialSqlite', '10.0.0') ON CONFLICT (\"MigrationId\") DO NOTHING");
        db.Database.Migrate();
    }
    SeedData.Initialize(app);
}

app.UseCors("AllowAngular");

app.UseAuthorization();
app.MapControllers();
app.Run();

