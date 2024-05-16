using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.Data;
using TravelAgency.Models.UserRelated;

namespace TravelAgency.Utils.Abstract
{
    public abstract class BaseController : Controller
    {
        protected readonly UserManager<AccountModel> UserManager;
        protected readonly ApplicationDbContext Context;

        protected BaseController()
        {
        }

        protected BaseController(UserManager<AccountModel> userManager, ApplicationDbContext context)
        {
            UserManager = userManager;
            Context = context;
        }
    }
}