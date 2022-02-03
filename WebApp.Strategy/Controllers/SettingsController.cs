using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Strategy.Models;

namespace WebApp.Strategy.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public SettingsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            Settings settings = new();
            var claim = User.Claims.Where(p => p.Type == Settings.claimDBType).FirstOrDefault();
            if (claim != null)
            {
                settings.DataBaseType = (EDbType)int.Parse(claim.Value);
            }
            else 
                settings.DataBaseType = settings.GetDefaultDBType;
            
            return View(settings);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeDatabase(int databaseType)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var newClaim = new Claim(Settings.claimDBType, databaseType.ToString());

            var calims = await _userManager.GetClaimsAsync(user);

            var hasDBTypeClaim = calims.FirstOrDefault(p => p.Type == newClaim.Type);

            if (hasDBTypeClaim != null)
            {
                await _userManager.ReplaceClaimAsync(user, hasDBTypeClaim, newClaim);
            }
            else
            {
                await _userManager.AddClaimAsync(user, newClaim);
            }

            await _signInManager.SignOutAsync();
            var authResult = await HttpContext.AuthenticateAsync();

            await _signInManager.SignInAsync(user, authResult.Properties);

            return RedirectToAction(nameof(Index));
        }
    }
}
