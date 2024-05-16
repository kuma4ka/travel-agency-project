using Microsoft.AspNetCore.Mvc;
using TravelAgency.Models;
using TravelAgency.Models.UserRelated;
using TravelAgency.Utils.Enumerations;

namespace TravelAgency.Utils.Interfaces;

public interface IAdmin
{
    public Task<IActionResult> AdminPanel();
    public Task<IActionResult> AddUser(AddUserModel newUser);
    public Task<IActionResult> UpdateUser(string userId, string firstName, string lastName, string email, string phone, UserRole role);
    public Task<IActionResult> DeleteUser(string userId);
    public Task<IActionResult> AddFlight(FlightModel newFlight);
    public Task<IActionResult> UpdateFlight(int flightId, string departureCity, string destinationCity, DateTime departureTime, DateTime arrivalTime, decimal price, bool isAvailable, string? imageUrl, string? description);
    public Task<IActionResult> DeleteFlight(int flightId);
}