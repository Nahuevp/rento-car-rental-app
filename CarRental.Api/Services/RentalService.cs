using System.Threading.Tasks;
using CarRental.Api.Data;
using CarRental.Api.Dto;
using CarRental.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Api.Services
{

    public class RentalService : IRentalService
    {
        private readonly AppDbContext _context;

        public RentalService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RentalResultDto> CreateRentalAsync(RentalDto rentalDto)
        {
            if (rentalDto.StartDate >= rentalDto.EndDate)
                return new RentalResultDto { Success = false, Error = "Invalid rental period." };

            var car = await _context.Cars.FindAsync(rentalDto.CarId);
            if (car == null)
                return new RentalResultDto { Success = false, Error = "Car not found." };

            // 🚫 1 reserva activa por usuario
            bool hasActiveRental = await _context.Rentals.AnyAsync(r =>
                r.UserId == rentalDto.UserId &&
                r.EndDate >= DateTime.Today
            );

            if (hasActiveRental)
                return new RentalResultDto { Success = false, Error = "Ya tenés una reserva activa." };

            // 🚗 disponibilidad del auto
            bool isCarAvailable = !await _context.Rentals.AnyAsync(r =>
                r.CarId == rentalDto.CarId &&
                (
                    (rentalDto.StartDate >= r.StartDate && rentalDto.StartDate < r.EndDate) ||
                    (rentalDto.EndDate > r.StartDate && rentalDto.EndDate <= r.EndDate) ||
                    (rentalDto.StartDate <= r.StartDate && rentalDto.EndDate >= r.EndDate)
                ));

            if (!isCarAvailable)
                return new RentalResultDto { Success = false, Error = "Car is not available for the selected period." };

            var totalDays = (rentalDto.EndDate.Date - rentalDto.StartDate.Date).Days;
            if (totalDays <= 0)
                return new RentalResultDto { Success = false, Error = "Rental must be at least one day." };

            var totalPrice = totalDays * car.Price;

            var rental = new Rental
            {
                UserId = rentalDto.UserId,
                CarId = rentalDto.CarId,
                StartDate = rentalDto.StartDate,
                EndDate = rentalDto.EndDate,
                Price = totalPrice
            };

            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            return new RentalResultDto
            {
                Success = true,
                Rental = rental,
                TotalPrice = totalPrice
            };
        }

        public async Task<List<RentalPeriodDto>> GetActiveRentalsByCarAsync(int carId)
        {
            var today = DateTime.Today;

            return await _context.Rentals
                .Where(r =>
                    r.CarId == carId &&
                    r.EndDate >= today
                )
                .Select(r => new RentalPeriodDto
                {
                    StartDate = r.StartDate,
                    EndDate = r.EndDate
                })
                .ToListAsync();
        }

        public async Task<CarAvailabilityDto> GetCarAvailabilityAsync(int carId)
        {
            var today = DateTime.Today;

            var rentals = _context.Rentals
                .Where(r => r.CarId == carId && r.EndDate >= today);

            DateTime availableFrom;

            if (await rentals.AnyAsync())
            {
                var lastEndDate = await rentals.MaxAsync(r => r.EndDate);
                availableFrom = lastEndDate.AddDays(1);
            }
            else
            {
                availableFrom = today;
            }

            return new CarAvailabilityDto
            {
                AvailableFrom = availableFrom
            };
        }


        public async Task<Rental?> GetActiveRentalByUserAsync(int userId)
        {
            return await _context.Rentals
                .Include(r => r.Car)
                .Where(r => r.UserId == userId && r.EndDate >= DateTime.Today)
                .OrderByDescending(r => r.StartDate)
                .FirstOrDefaultAsync();
        }


    }
}