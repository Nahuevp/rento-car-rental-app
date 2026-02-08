using CarRental.Api.Data;
using CarRental.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Api
{
    public static class SeedData
{
    public static void Initialize(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        context.Database.Migrate();

        if (context.Cars.Any())
            return;

        context.Cars.AddRange(
            new Car
            {
                Brand = "BMW",
                Model = "M3",
                Year = 2021,
                Price = 60
            },
            new Car
            {
                Brand = "Mercedes Benz",
                Model = "E220",
                Year = 2020,
                Price = 70
            },
            new Car
            {
                Brand = "Ford",
                Model = "M3",
                Year = 2022,
                Price = 55
            }
        );

        context.SaveChanges();
    }
}

}
