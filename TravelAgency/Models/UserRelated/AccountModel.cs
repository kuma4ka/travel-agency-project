using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using TravelAgency.Utils.Enumerations;

namespace TravelAgency.Models.UserRelated
{
    public class AccountModel : IdentityUser
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, ErrorMessage = "First name cannot be longer than 100 characters")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, ErrorMessage = "Last name cannot be longer than 100 characters")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "User role is required")]
        public UserRole Role { get; set; } = UserRole.User;
    }
}