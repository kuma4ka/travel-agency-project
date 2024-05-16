using System.ComponentModel.DataAnnotations;
using TravelAgency.Utils.Enumerations;

namespace TravelAgency.Models
{
    public class BookingModel
    {
        public int BookingId { get; init; }

        [Required(ErrorMessage = "User ID is required")]
        [StringLength(50, ErrorMessage = "User id cannot be longer than 50 characters.")]
        public string? UserId { get; init; }

        [Required(ErrorMessage = "Flight ID is required")]
        public int FlightId { get; init; }

        [Required(ErrorMessage = "User first name is required")]
        [StringLength(100, ErrorMessage = "User first name cannot be longer than 100 characters.")]
        public string? UserFirstName { get; init; }

        [Required(ErrorMessage = "User last name is required")]
        [StringLength(100, ErrorMessage = "User last name cannot be longer than 100 characters.")]
        public string? UserLastName { get; init; }

        [Required(ErrorMessage = "User phone number is required")]
        [StringLength(100, ErrorMessage = "Phone number cannot be longer than 100 characters.")]
        public string? UserPhoneNumber { get; init; }

        [Required(ErrorMessage = "User email is required")]
        [StringLength(100, ErrorMessage = "Email cannot be longer than 100 characters.")]
        public string? UserEmail { get; init; }

        [Required(ErrorMessage = "Departure city is required")]
        [StringLength(100, ErrorMessage = "Departure city cannot be longer than 100 characters.")]
        public string? DepartureCity { get; init; }

        [Required(ErrorMessage = "Destination city is required")]
        [StringLength(100, ErrorMessage = "Destination city cannot be longer than 100 characters.")]
        public string? DestinationCity { get; init; }

        [Required(ErrorMessage = "Departure time is required")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format for departure time")]
        public DateTime DepartureTime { get; init; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value")]
        public decimal Price { get; init; }

        [Required(ErrorMessage = "Booking date is required")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format for booking date")]
        public DateTime BookingDate { get; init; }

        [Required(ErrorMessage = "Booking status is required")]
        public BookingStatus Status { get; init; }
    }
}
