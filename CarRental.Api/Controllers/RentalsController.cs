using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Api.Data;
using CarRental.Api.Dto;
using CarRental.Api.Models;
using CarRental.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalsController : ControllerBase
    {

        private readonly IRentalService _rentalService;

        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRental([FromBody] RentalDto rentalDto)
        {
            var result = await _rentalService.CreateRentalAsync(rentalDto);

            if (!result.Success)
            {
                if (result.Error == "Car not found.")
                    return NotFound(result.Error);
                if (result.Error == "Car is not available for the selected period.")
                    return Conflict(result.Error);
                return BadRequest(result.Error);
            }

            return Ok(new
            {
                result.Rental.Id,
                result.TotalPrice
            });
        }

        [HttpGet("car/{carId}")]
        public async Task<IActionResult> GetRentalsByCar(int carId)
        {
            var rentals = await _rentalService.GetActiveRentalsByCarAsync(carId);
            return Ok(rentals);
        }

        [HttpGet("car/{carId}/availability")]
        public async Task<IActionResult> GetCarAvailability(int carId)
        {
            var result = await _rentalService.GetCarAvailabilityAsync(carId);
            return Ok(result);
        }

        [HttpGet("active/{userId}")]
        public async Task<IActionResult> GetActiveRental(int userId)
        {
            var rental = await _rentalService.GetActiveRentalByUserAsync(userId);

            if (rental == null)
                return NotFound("No hay reservas activas");

            return Ok(new
            {
                rental.Id,
                rental.StartDate,
                rental.EndDate,
                TotalPrice = rental.Price,
                Status = "Confirmada",
                Car = new
                {
                    rental.Car.Brand,
                    rental.Car.Model,
                    rental.Car.Year,
                    rental.Car.Price
                }
            });
        }
    }
}