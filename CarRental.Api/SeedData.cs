using CarRental.Api.Data;
using CarRental.Api.Models;

namespace CarRental.Api;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
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
                Model = "Fiesta",
                Year = 2022,
                Price = 55
            }
        );

        context.SaveChanges();
    }
}
