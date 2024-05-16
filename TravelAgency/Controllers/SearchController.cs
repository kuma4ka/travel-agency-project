using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Models.Search;
using TravelAgency.Utils.Abstract;
using Microsoft.AspNetCore.Identity;
using TravelAgency.Data;
using TravelAgency.Models.UserRelated;

namespace TravelAgency.Controllers
{
    public class SearchController(UserManager<AccountModel> userManager, ApplicationDbContext context)
        : BaseController(userManager, context)
    {
        public async Task<IActionResult> SearchResult(SearchViewModel searchViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(new SearchResultModel());
            }

            var matchingFlights = await Context.Flights
                .Where(f =>
                    f.DepartureCity == searchViewModel.DepartureCity &&
                    f.DestinationCity == searchViewModel.ArrivalCity &&
                    f.DepartureTime == searchViewModel.DepartureDate)
                .ToListAsync();

            var searchResultModel = new SearchResultModel
            {
                Flights = matchingFlights
            };

            return View(searchResultModel);
        }
    }
}