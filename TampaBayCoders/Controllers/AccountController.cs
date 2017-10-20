using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using TampaBayCoders.Services;
using TampaBayCoders.Configuration;

namespace TampaBayCoders.Controllers
{
	public class AccountController : ControllerBase
	{
		public AccountController(IOptions<CosmosDbSettings> cosmosDbSettings) : base(cosmosDbSettings) { }

		// Login with Auth0
		public IActionResult Login(string returnUrl = "/")
		{
			return new ChallengeResult("Auth0", new AuthenticationProperties() { RedirectUri = returnUrl });
		}

		// Logout through Auth0
		public async Task Logout()
		{
			await HttpContext.Authentication.SignOutAsync("Auth0", new AuthenticationProperties
			{
				RedirectUri = Url.Action("Index", "Home")
			});
			await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
		}

		// View Your Own Claims
		[Authorize]
		public IActionResult Claims()
		{
			return View();
		}

		[Authorize]
		public async Task<IActionResult> Test()
		{
			var profile = await ProfileService.Connect(SharedDataConnection).Create(User);
			//var profile = await profileService.Create(new Services.Profile
			//{
			//	DisplayName = "test display name",
			//	Summary = "test summary",
			//	UserId = User.Identity.Name,
			//	Email = User.Claims.FirstOrDefault(c => c.Type == "profile")?.Value
			//});
			return Content("Created Profile " + profile.UserId);
		}
	}
}