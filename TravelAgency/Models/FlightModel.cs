using System.ComponentModel.DataAnnotations;
using TravelAgency.Utils.Validation;

namespace TravelAgency.Models
{
    public class FlightModel
    {
        [Required]
        public int FlightId { get; init; }
        
        [Required(ErrorMessage = "Departure City is required")]
        [StringLength(100, ErrorMessage = "Departure City cannot be longer than 100 characters.")]
        public string? DepartureCity { get; set; }
        
        [Required(ErrorMessage = "Destination City is required")]
        [StringLength(100, ErrorMessage = "Destination City cannot be longer than 100 characters.")]
        public string? DestinationCity { get; set; }
        
        [Required(ErrorMessage = "Departure Time is required")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format for Departure Time")]
        public DateTime DepartureTime { get; set; }
        
        [Required(ErrorMessage = "Arrival Time is required")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format for Arrival Time")]
        [DateGreaterThan("DepartureTime", ErrorMessage = "Arrival Time must be later than Departure Time")]
        public DateTime ArrivalTime { get; set; }
        
        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value")]
        public decimal Price { get; set; }
        
        [Required(ErrorMessage = "Availability status is required")]
        public bool IsAvailable { get; set; }
        
        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters.")]
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "Image URL is required")]
        [StringLength(100, ErrorMessage = "ImageUrl cannot be longer than 100 characters.")]
        public string? ImageUrl { get; set; }
    }
}