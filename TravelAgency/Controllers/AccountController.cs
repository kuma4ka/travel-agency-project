using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.Data;
using TravelAgency.Models.Authentication;
using TravelAgency.Models.UserRelated;
using TravelAgency.Utils;
using TravelAgency.Utils.Abstract;
using TravelAgency.Utils.Interfaces;

namespace TravelAgency.Controllers
{
    public class AccountController(
        UserManager<AccountModel> userManager,
        SignInManager<AccountModel> signInManager,
        ApplicationDbContext context)
        : BaseController(userManager, context), IAccount
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var existingUser = await FindBy.FindByEmailAsync(model.Email, UserManager);

            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "This email is already in use!");
                return View(model);
            }

            var user = new AccountModel
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            if (model.Password is null) return View(model);

            var result = await UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(model);

            var user = await FindBy.FindByEmailAsync(model.Email, UserManager);

            if (model.Password != null && user != null && await UserManager.CheckPasswordAsync(user, model.Password))
            {
                await signInManager.SignInAsync(user, model.RememberMe);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Error during authorisation!");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            var model = new ChangePasswordViewModel();
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await UserManager.GetUserAsync(User);

            if (user is null)
            {
                return RedirectToAction(nameof(Login));
            }

            if (model is { CurrentPassword: not null, NewPassword: not null })
            {
                var changePasswordResult = await UserManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return View(model);
                }
            }

            await signInManager.RefreshSignInAsync(user);
            model.Changed = true;

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ViewProfile()
        {
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var model = new ProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ViewProfile(ProfileViewModel model)
        {
            var user = await UserManager.GetUserAsync(User);

            if (!ModelState.IsValid) return View(model);

            if (user == null) return View(model);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;

            var result = await UserManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                model.Changed = true;
                return View(model);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ViewBookings()
        {
            var user = await UserManager.GetUserAsync(User);
            if (user is null)
            {
                return RedirectToAction(nameof(Login));
            }

            var bookings = Context.Bookings.Where(b => b.UserId == user.Id).ToList();

            return View(bookings);
        }
    }
}
