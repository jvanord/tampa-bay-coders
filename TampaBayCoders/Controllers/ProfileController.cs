using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TampaBayCoders.Configuration;
using TampaBayCoders.Models;
using TampaBayCoders.Services;

namespace TampaBayCoders.Controllers
{
	[Route("[controller]")]
	public class ProfileController : ControllerBase
	{
		public ProfileController(IOptions<CosmosDbSettings> cosmosDbSettings) : base(cosmosDbSettings) { }

		// GET: /Profile
		[Authorize, Route("")]
		public async Task<IActionResult> Index()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
			var myProfile = await ProfileService.Connect(SharedDataConnection).FindByUserId(userId);
			if (myProfile == null)
				return RedirectToAction("Create");
			return RedirectToAction("ViewProfile", new { id = myProfile.Id });
			return View("ViewProfile", myProfile);
		}

		[Route("{id}")]
		public async Task<IActionResult> ViewProfile(string id)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
			var myProfile = await ProfileService.Connect(SharedDataConnection).ReadAsync(id);
			if (myProfile == null)
				return RedirectToAction("Create");
			return View(myProfile);
		}

		[Authorize, HttpGet, Route("create")]
		public IActionResult Create()
		{
			var myProfile = ProfileService.Connect(SharedDataConnection).StubForUser(User);
			return View("EditProfile", myProfile);
		}

		[Authorize, HttpPost, Route("create")]
		public async Task<IActionResult> Create(Profile profile)
		{
			if (!ModelState.IsValid)
				return View("EditProfile", profile);
			var createdProfile = await ProfileService.Connect(SharedDataConnection).Create(profile);
			return RedirectToAction("ViewProfile", new { id = createdProfile.Id });
		}
	}
}