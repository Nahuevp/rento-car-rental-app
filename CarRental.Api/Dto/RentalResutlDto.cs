using CarRental.Api.Models;

namespace CarRental.Api.Dto
{
    public class RentalResultDto
    {
        public bool Success { get; set; }
        public string Error { get; set; } = string.Empty;
        public Rental? Rental { get; set; }
        public decimal TotalPrice { get; set; }
    }
}