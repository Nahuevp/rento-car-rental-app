using CarRental.Api.Data;
using CarRental.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Api;

public static class SeedData
{
    public static void Initialize(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Solo para DEV
        context.Database.Migrate();

        // 👤 Usuario demo (SIEMPRE comprobar)
        if (!context.Users.Any())
        {
            context.Users.Add(new User
            {
                Id = 1,
                Username = "demo",
                PasswordHash = "demo",
                CreatedAt = DateTime.UtcNow
            });
        }

        // 🚗 Autos (solo si no existen)
        if (!context.Cars.Any())
        {
            // Si ya los cargás manualmente, este bloque puede quedar vacío
            // o directamente eliminarse
        }

        context.SaveChanges();
    }
}
