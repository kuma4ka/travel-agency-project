using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Models.UserRelated;

public class ChangePasswordViewModel
{
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Current password")]
    public string? CurrentPassword { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    [Display(Name = "New password")]
    public string? NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm new password")]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match!")]
    public string? ConfirmPassword { get; set; }
    
    public bool Changed { get; set; } = false;
}