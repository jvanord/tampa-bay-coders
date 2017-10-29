using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using TampaBayCoders.Configuration;
using TampaBayCoders.Data;
using TampaBayCoders.Models;

namespace TampaBayCoders.Controllers
{
	[Route("[controller]")]
	public class ToolsController : Controller
	{
		private CosmosDbSettings _dbSettings;

		public ToolsController(IOptions<CosmosDbSettings> cosmosDbSettings) : base() { _dbSettings = cosmosDbSettings.Value; }

		[Route("compensation-calculator")]
		public IActionResult Compensation()
		{
			return View();
		}

		[Route("site-diagnostics")]
		public async Task<IActionResult> Diagnostics()
		{
			var results = await SiteDiagnostics.RunAsync(_dbSettings);
			return View(results);
		}

	}
}