using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Api.Dto
{
    public class CarDto
    {
       public int Id { get; set; }
       [Required]
        public string Brand { get; set; } = string.Empty;
        [Required]
        public string Model { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Year { get; set; }
    }
}