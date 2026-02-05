using CarRental.Api.Dto;

namespace CarRental.Api.Services
{
    public interface ICarService
    {
        Task<CarDto> CreateCarAsync(CarDto carDto);
        Task<bool> DeleteCarAsync(int id);
        Task<CarDto> GetCarAsync(int id);
        Task<List<CarDto>> GetCarsAsync();
        Task<bool> UpdateCarAsync(int id, CarDto carDto);
    }
}