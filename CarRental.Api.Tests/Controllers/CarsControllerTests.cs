using Xunit;
using Moq;
using CarRental.Api.Controllers;
using CarRental.Api.Dto;
using CarRental.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CarsControllerTests
{
    private readonly Mock<ICarService> _carServiceMock;
    private readonly CarsController _controller;

    public CarsControllerTests()
    {
        _carServiceMock = new Mock<ICarService>();
        _controller = new CarsController(_carServiceMock.Object);
    }

    [Fact]
    public async Task GetCars_ReturnsListOfCars()
    {
        var cars = new List<CarDto>
        {
            new CarDto { Id = 1, Brand = "Ford", Model = "Fiesta", Price = 100, Year = 2024 }
        };
        _carServiceMock.Setup(s => s.GetCarsAsync()).ReturnsAsync(cars);

        var result = await _controller.GetCars();

        Assert.Equal(cars, result.Value);
    }

    [Fact]
    public async Task GetCar_ReturnsCar_WhenExists()
    {
        var car = new CarDto { Id = 1, Brand = "Ford", Model = "Fiesta", Price = 100, Year = 2024 };
        _carServiceMock.Setup(s => s.GetCarAsync(1)).ReturnsAsync(car);

        var result = await _controller.GetCar(1);

        Assert.Equal(car, result.Value);
    }

    [Fact]
    public async Task GetCar_ReturnsNotFound_WhenDoesNotExist()
    {
        _carServiceMock.Setup(s => s.GetCarAsync(1)).ReturnsAsync((CarDto)null);

        var result = await _controller.GetCar(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateCar_ReturnsCreatedCar_WhenValid()
    {
        var carDto = new CarDto { Id = 1, Brand = "Ford", Model = "Fiesta", Price = 100, Year = 2024 };
        _carServiceMock.Setup(s => s.CreateCarAsync(carDto)).ReturnsAsync(carDto);

        var result = await _controller.CreateCar(carDto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(carDto, createdResult.Value);
    }

    [Fact]
    public async Task CreateCar_ReturnsBadRequest_WhenYearInvalid()
    {
        var carDto = new CarDto { Id = 1, Brand = "Ford", Model = "Fiesta", Price = 100, Year = 2000 };

        var result = await _controller.CreateCar(carDto);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("The car cannot be older than 5 years.", badRequest.Value);
    }

    [Fact]
    public async Task UpdateCar_ReturnsNoContent_WhenUpdated()
    {
        var carDto = new CarDto { Id = 1, Brand = "Ford", Model = "Fiesta", Price = 100, Year = 2024 };
        _carServiceMock.Setup(s => s.UpdateCarAsync(1, carDto)).ReturnsAsync(true);

        var result = await _controller.UpdateCar(1, carDto);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateCar_ReturnsNotFound_WhenCarDoesNotExist()
    {
        var carDto = new CarDto { Id = 1, Brand = "Ford", Model = "Fiesta", Price = 100, Year = 2024 };
        _carServiceMock.Setup(s => s.UpdateCarAsync(1, carDto)).ReturnsAsync(false);

        var result = await _controller.UpdateCar(1, carDto);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteCar_ReturnsNoContent_WhenDeleted()
    {
        _carServiceMock.Setup(s => s.DeleteCarAsync(1)).ReturnsAsync(true);

        var result = await _controller.DeleteCar(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteCar_ReturnsNotFound_WhenCarDoesNotExist()
    {
        _carServiceMock.Setup(s => s.DeleteCarAsync(1)).ReturnsAsync(false);

        var result = await _controller.DeleteCar(1);

        Assert.IsType<NotFoundResult>(result);
    }
}