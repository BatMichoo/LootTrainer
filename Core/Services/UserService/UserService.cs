using Core.Utilities;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Core.Services.UserService
{
    public class UserService
    {
        private readonly UserManager<BlizzUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ClaimsPrincipal _claimsPrincipal;

        public UserService(UserManager<BlizzUser> userManager, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _claimsPrincipal = httpContextAccessor.HttpContext.User;
        }

        public int GetBattleNetId()
        {
            return int.Parse(_claimsPrincipal.Claims.FirstOrDefault(c => c.Type == Claims.BattleNetId)!.Value);
        }
    }
}
