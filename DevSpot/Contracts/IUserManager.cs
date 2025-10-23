using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DevSpot.Contracts
{
    public interface IUserManager
    {
      string GetUserId(ClaimsPrincipal principal);
        // Add other methods ?
    }
}
