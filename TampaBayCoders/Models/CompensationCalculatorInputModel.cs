using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TampaBayCoders.Services.CompensationCalculator;

namespace TampaBayCoders.Models
{
	public class CompensationCalculatorViewModel
	{
		public enum CalculationType
		{
			[Display(Name = "Annual Salary")]
			Salary,

			[Display(Name = "Hourly Wage")]
			Wage,

			[Display(Name = "Contract Rate")]
			Rate
		}
		public CalculationType Calculation { get; set; }
		public decimal? Salary { get; set; }
		public decimal? Wage { get; set; }
		public decimal? Rate { get; set; }
		public CalculationResult Result { get; set; }
		public bool ResultCheck { get; set; }
		public TaxProfile TaxProfile { get; internal set; }
		public int HoursOffPerYear { get; internal set; }
	}
}
