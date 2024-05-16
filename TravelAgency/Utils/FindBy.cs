using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Models.UserRelated;

namespace TravelAgency.Utils;

public static class FindBy
{
    public static async Task<AccountModel?> FindByEmailAsync(string? email, UserManager<AccountModel> userManager)
    {
        return await userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}