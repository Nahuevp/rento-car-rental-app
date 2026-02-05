using Xunit;
using CarRental.Api.Data;
using CarRental.Api.Services;
using CarRental.Api.Dto;
using CarRental.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class CarServiceTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateCarAsync_AddsCarToDatabase()
    {
        using var context = GetDbContext();
        var service = new CarService(context);

        var carDto = new CarDto { Brand = "Ford", Model = "Fiesta", Price = 100, Year = 2024 };
        var result = await service.CreateCarAsync(carDto);

        Assert.NotNull(result);
        Assert.Equal("Ford", result.Brand);
        Assert.Equal("Fiesta", result.Model);
        Assert.Equal(100, result.Price);
        Assert.Equal(2024, result.Year);
        Assert.True(result.Id > 0);
    }

    [Fact]
    public async Task GetCarsAsync_ReturnsAllCars()
    {
        using var context = GetDbContext();
        context.Cars.Add(new Car { Brand = "A", Model = "Uno", Price = 50, Year = 2023 });
        context.Cars.Add(new Car { Brand = "B", Model = "Dos", Price = 60, Year = 2024 });
        await context.SaveChangesAsync();

        var service = new CarService(context);
        var cars = await service.GetCarsAsync();

        Assert.Equal(2, cars.Count);
        Assert.Contains(cars, c => c.Brand == "A");
        Assert.Contains(cars, c => c.Brand == "B");
    }

    [Fact]
    public async Task GetCarAsync_ReturnsCar_WhenExists()
    {
        using var context = GetDbContext();
        var car = new Car { Brand = "X", Model = "Test", Price = 80, Year = 2022 };
        context.Cars.Add(car);
        await context.SaveChangesAsync();

        var service = new CarService(context);
        var result = await service.GetCarAsync(car.Id);

        Assert.NotNull(result);
        Assert.Equal("X", result.Brand);
        Assert.Equal("Test", result.Model);
    }

    [Fact]
    public async Task GetCarAsync_ReturnsNull_WhenCarDoesNotExist()
    {
        using var context = GetDbContext();
        var service = new CarService(context);

        var result = await service.GetCarAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateCarAsync_UpdatesCar_WhenExists()
    {
        using var context = GetDbContext();
        var car = new Car { Brand = "Old", Model = "OldModel", Price = 90, Year = 2022 };
        context.Cars.Add(car);
        await context.SaveChangesAsync();

        var service = new CarService(context);
        var updatedDto = new CarDto { Id = car.Id, Brand = "New", Model = "NewModel", Price = 120, Year = 2023 };
        var updated = await service.UpdateCarAsync(car.Id, updatedDto);

        Assert.True(updated);
        var updatedCar = await context.Cars.FindAsync(car.Id);
        Assert.Equal("New", updatedCar.Brand);
        Assert.Equal("NewModel", updatedCar.Model);
        Assert.Equal(120, updatedCar.Price);
        Assert.Equal(2023, updatedCar.Year);
    }

    [Fact]
    public async Task UpdateCarAsync_UpdatesCar_WhenNotExists()
    {
        using var context = GetDbContext();
        var service = new CarService(context);

        var updatedDto = new CarDto { Id = 999, Brand = "New", Model = "NewModel", Price = 120, Year = 2023 };
        var updated = await service.UpdateCarAsync(999, updatedDto);

        Assert.False(updated);
    }

    [Fact]
    public async Task DeleteCarAsync_RemovesCar_WhenExists()
    {
        using var context = GetDbContext();
        var car = new Car { Brand = "DeleteMe", Model = "Model", Price = 70, Year = 2021 };
        context.Cars.Add(car);
        await context.SaveChangesAsync();

        var service = new CarService(context);
        var deleted = await service.DeleteCarAsync(car.Id);

        Assert.True(deleted);
        Assert.Null(await context.Cars.FindAsync(car.Id));
    }

    [Fact]
    public async Task DeleteCarAsync_ReturnsFalse_WhenCarDoesNotExist()
    {
        using var context = GetDbContext();
        var service = new CarService(context);

        var deleted = await service.DeleteCarAsync(999);

        Assert.False(deleted);
    }
}