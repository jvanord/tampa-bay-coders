using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
			var results = await runDiagnosticsAsync();
			return View(results);
		}

		private async Task<SiteDiagnosticsViewModel> runDiagnosticsAsync()
		{
			var results = new SiteDiagnosticsViewModel();
			DocumentClient client;
			results.DatabaseEndpoint = _dbSettings.Endpoint;
			results.DatabaseId = _dbSettings.DatabaseId;
			try
			{
				client = new DocumentClient(new Uri(_dbSettings.Endpoint), _dbSettings.Key);
				await client.OpenAsync();
				results.ConnectionTest = new SiteDiagnosticsViewModel.TestResult();
			}
			catch (Exception ex) {
				results.ConnectionTest = new SiteDiagnosticsViewModel.TestResult
				{
					Status = SiteDiagnosticsViewModel.ResultStatus.Fail,
					Message = ex.Message
				};
			}
			return results;
		}
	}
}