using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Data;
using TravelAgency.Models;
using TravelAgency.Models.Admin;
using TravelAgency.Models.UserRelated;
using TravelAgency.Utils;
using TravelAgency.Utils.Abstract;
using TravelAgency.Utils.Interfaces;
using UserRole = TravelAgency.Utils.Enumerations.UserRole;

namespace TravelAgency.Controllers
{
    public class AdminController(UserManager<AccountModel> userManager, ApplicationDbContext context)
        : BaseController(userManager, context), IAdmin
    {
        [HttpGet]
        public async Task<IActionResult> AdminPanel()
        {
            var user = await UserManager.GetUserAsync(User);

            if (user is not { Role: UserRole.Admin })
            {
                return RedirectToAction("Index", "Home");
            }

            var allUsers = await UserManager.Users.ToListAsync();

            var usersExceptCurrent = allUsers
                .Where(u => u.Id != user.Id).ToList();

            var articles = await Context.Flights.ToListAsync();

            AdminPanelViewModel model = new()
            {
                AccModels = usersExceptCurrent,
                FlightModels = articles
            };
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddUser()
        {
            var newUser = new AddUserModel();
            return View(newUser);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddUser(AddUserModel newUser)
        {
            if (!ModelState.IsValid) return View(newUser);

            var existingUser = await FindBy.FindByEmailAsync(newUser.Email, UserManager);

            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "This email address is already in use!");
                return View(newUser);
            }

            var user = new AccountModel
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                UserName = newUser.Email,
                Email = newUser.Email,
                PhoneNumber = newUser.Phone,
                Role = newUser.Role
            };

            var result = await UserManager.CreateAsync(user, newUser.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("AdminPanel");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(newUser);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateUser(string userId, string firstName, string lastName, string email, string phone, UserRole role)
        {
            var user = await UserManager.FindByIdAsync(userId);

            if (user == null)
            {
                return RedirectToAction("AdminPanel");
            }

            user.FirstName = firstName;
            user.LastName = lastName;
            user.Email = email;
            user.PhoneNumber = phone;
            user.Role = role;

            var result = await UserManager.UpdateAsync(user);
            if (result.Succeeded) return RedirectToAction("AdminPanel");
            
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            
            return RedirectToAction("AdminPanel");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);

            if (user == null) return RedirectToAction("AdminPanel");

            await UserManager.DeleteAsync(user);
            await Context.SaveChangesAsync();

            return RedirectToAction("AdminPanel");
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddFlight()
        {
            var model = new FlightModel
            {
                DepartureTime = DateTime.Now,
                ArrivalTime = DateTime.Now,
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddFlight(FlightModel newFlight)
        {
            if (!ModelState.IsValid) return View(newFlight);

            var existingFlight = await Context.Flights.FirstOrDefaultAsync(f => f.FlightId == newFlight.FlightId);

            if (existingFlight != null)
            {
                ModelState.AddModelError(string.Empty, "Flight with such id already exists!");
                return View(newFlight);
            }

            await Context.Flights.AddAsync(newFlight);
            await Context.SaveChangesAsync();

            return RedirectToAction("AdminPanel");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateFlight(int flightId, string departureCity, string destinationCity,
            DateTime departureTime, DateTime arrivalTime, decimal price, bool isAvailable, string? imageUrl, string? description)
        {
            var flight = await Context.Flights.FindAsync(flightId);

            if (flight == null) return RedirectToAction("AdminPanel");

            flight.DepartureCity = departureCity;
            flight.DestinationCity = destinationCity;
            flight.DepartureTime = departureTime;
            flight.ArrivalTime = arrivalTime;
            flight.Price = price;
            flight.IsAvailable = isAvailable;
            flight.ImageUrl = imageUrl;
            flight.Description = description;

            await Context.SaveChangesAsync();

            return RedirectToAction("AdminPanel");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteFlight(int flightId)
        {
            var flight = await Context.Flights.FindAsync(flightId);

            if (flight == null) return RedirectToAction("AdminPanel");

            Context.Flights.Remove(flight);
            await Context.SaveChangesAsync();

            return RedirectToAction("AdminPanel");
        }
    }
}
