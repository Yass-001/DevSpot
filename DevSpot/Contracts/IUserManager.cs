using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DevSpot.Contracts
{
    public interface IUserManager
    {
        string GetUserId(ClaimsPrincipal principal);
        // Add other methods ?

        // Change user password
        Task<IdentityResult> ChangePasswordAsync(IdentityUser user, string currentPassword, string newPassword);

    }
}
