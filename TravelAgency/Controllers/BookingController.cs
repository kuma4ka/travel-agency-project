using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.Data;
using TravelAgency.Models;
using TravelAgency.Models.UserRelated;
using TravelAgency.Utils.Abstract;
using TravelAgency.Utils.Enumerations;

namespace TravelAgency.Controllers
{
    public class BookingController : BaseController
    {
        public delegate Task ConfirmationEventHandler(string userEmail, string message);

        public event ConfirmationEventHandler? BookingConfirmed;

        public BookingController(UserManager<AccountModel> userManager, ApplicationDbContext context)
            : base(userManager, context)
        {
            BookingConfirmed += HandleBookingConfirmation;
        }
        
        private Task HandleBookingConfirmation(string userEmail, string message)
        {
            Console.WriteLine($"Booking confirmation received for user {userEmail}. Message: {message}");
            return Task.CompletedTask;
        }
        
        [HttpPost]
        public async Task<IActionResult> ConfirmBooking(string? flightJson)
        {
            if (string.IsNullOrEmpty(flightJson))
            {
                return BadRequest("Flight JSON is missing or empty.");
            }

            FlightModel? flightModel = null;
            try
            {
                flightModel = JsonSerializer.Deserialize<FlightModel>(flightJson);
            }
            catch (JsonException ex)
            {
                return BadRequest("Error deserializing flight JSON.");
            }

            if (flightModel == null) return View();

            var user = await UserManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var booking = new BookingModel()
            {
                BookingDate = DateTime.Now,
                UserId = user.Id,
                FlightId = flightModel.FlightId,
                UserFirstName = user.FirstName,
                UserLastName = user.LastName,
                UserEmail = user.Email,
                UserPhoneNumber = user.PhoneNumber,
                DepartureCity = flightModel.DepartureCity,
                DestinationCity = flightModel.DestinationCity,
                DepartureTime = flightModel.DepartureTime,
                Price = flightModel.Price,
                Status = BookingStatus.Confirmed,
            };

            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Invalid booking details.");
                    return View("Error");
                }

                await UpdateFlightAvailability(flightModel);
                Context.Bookings.Add(booking);
                await Context.SaveChangesAsync();

                if (BookingConfirmed != null)
                {
                    await BookingConfirmed(user.Email, "Your booking has been confirmed.");
                }

                Console.WriteLine("Booking confirmed successfully!");
                return View(booking);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return View("Error");
            }
        }

        private async Task<IActionResult> UpdateFlightAvailability(FlightModel flightModel)
        {
            var flight = await Context.Flights.FindAsync(flightModel.FlightId);
            if (flight == null)
            {
                return NotFound("Flight not found.");
            }

            flight.IsAvailable = false;

            return Ok("Flight availability updated successfully.");
        }
    }
}
