using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TampaBayCoders.Configuration;

namespace TampaBayCoders.Controllers
{
	[Route("[controller]")]
	public class ToolsController : ControllerBase
	{
		public ToolsController(IOptions<CosmosDbSettings> cosmosDbSettings) : base(cosmosDbSettings) { }

		[Route("compensation-calculator")]
		public IActionResult Compensation()
		{
			return View();
		}

		[Route("site-diagnostics")]
		public IActionResult Diagnostics()
		{
			return View();
		}
	}
}