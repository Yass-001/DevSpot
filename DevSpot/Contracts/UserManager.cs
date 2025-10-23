using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DevSpot.Contracts
{
    public class UserManager : IUserManager
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserManager(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public string GetUserId(ClaimsPrincipal principal)
        {
            return _userManager.GetUserId(principal);
        }
    }
}
