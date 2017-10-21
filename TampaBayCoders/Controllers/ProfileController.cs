using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TampaBayCoders.Configuration;
using TampaBayCoders.Services;

namespace TampaBayCoders.Controllers
{
	[Route("[controller]")]
	public class ProfileController : ControllerBase
	{
		public ProfileController(IOptions<CosmosDbSettings> cosmosDbSettings) : base(cosmosDbSettings) { }

		// GET: /Profile
		[Authorize, Route("")]
		public async Task<IActionResult> IndexAsync()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
			var myProfile = await ProfileService.Connect(SharedDataConnection).FindByUserId(userId);
			//if (myProfile == null)
			//	return RedirectToAction("Create");
			return View("ViewProfile", myProfile);
		}

		[Route("{id}")]
		public IActionResult ViewProfile(string id)
		{
			return Content("Profile View For: " + id);
		}

		[Authorize, Route("create")]
		public IActionResult Create(string id)
		{
			return Content("Create a New Profile");
		}
	}
}