using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TampaBayCoders.Controllers
{
	public class AccountController : Controller
	{
		// Login with Auth0
		public IActionResult Login()
		{
			return new ChallengeResult("Auth0", new AuthenticationProperties() { RedirectUri = "/" });
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
	}
}