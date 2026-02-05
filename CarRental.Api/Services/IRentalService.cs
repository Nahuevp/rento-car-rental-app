using CarRental.Api.Dto;
using CarRental.Api.Models;

namespace CarRental.Api.Services
{
    public interface IRentalService
    {
        Task<RentalResultDto> CreateRentalAsync(RentalDto rentalDto);
        
        Task<List<RentalPeriodDto>> GetActiveRentalsByCarAsync(int carId);

        Task<CarAvailabilityDto> GetCarAvailabilityAsync(int carId);

        Task<Rental?> GetActiveRentalByUserAsync(int userId);

    }
}