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
using TampaBayCoders.Services.CompensationCalculator;

namespace TampaBayCoders.Controllers
{
	[Route("[controller]")]
	public class ToolsController : Controller
	{
		private CosmosDbSettings _dbSettings;

		public ToolsController(IOptions<CosmosDbSettings> cosmosDbSettings) : base() { _dbSettings = cosmosDbSettings.Value; }

		[HttpGet, Route("compensation-calculator")]
		public IActionResult Compensation()
		{
			return View(new CompensationCalculatorViewModel());
		}

		[HttpPost, Route("compensation-calculator/calculate")]
		public IActionResult CompensationResult(CompensationCalculatorViewModel model)
		{
			var scenario = CompensationScenario.TypicalFullTimeJob();
			model.TaxProfile = scenario.TaxProfile;
			model.HoursOffPerYear = scenario.DaysOffPerYear * 8;
			switch (model.Calculation)
			{
				case CompensationCalculatorViewModel.CalculationType.Salary:
					if (!model.Salary.HasValue) ModelState.AddModelError("Salary", "Cannot calculate for this Salary value.");
					model.Result = scenario.CalculateSalary(model.Salary.Value);
					model.ResultComparison = scenario.SalaryRequired(model.Result.TotalCompensation);
					model.ResultCheck = Math.Abs(model.ResultComparison - model.Salary.Value) < 0.01m;
					break;
				case CompensationCalculatorViewModel.CalculationType.Wage:
					if (!model.Wage.HasValue) ModelState.AddModelError("Wage", "Cannot calculate for this Wage value.");
					model.Result = scenario.CalculateHourlyWage(model.Wage.Value);
					model.ResultComparison = scenario.WageRequired(model.Result.TotalCompensation);
					model.ResultCheck = Math.Abs(model.ResultComparison - model.Wage.Value) < 0.01m;
					break;
				case CompensationCalculatorViewModel.CalculationType.Rate:
					if (!model.Rate.HasValue) ModelState.AddModelError("Rate", "Cannot calculate for this Rate value.");
					model.Result = scenario.CalculateContractRate(model.Rate.Value);
					model.ResultComparison = scenario.ContractRateRequired(model.Result.TotalCompensation, model.Result.ActualHoursWorked);
					model.ResultCheck = Math.Abs(model.ResultComparison - model.Rate.Value) < 0.01m;
					break;
				default: throw new InvalidOperationException("Invalid Calculation Type");
			}
			return PartialView(model);
		}

		[Route("site-diagnostics")]
		public async Task<IActionResult> Diagnostics()
		{
			var results = await SiteDiagnostics.RunAsync(_dbSettings);
			return View(results);
		}

	}
}