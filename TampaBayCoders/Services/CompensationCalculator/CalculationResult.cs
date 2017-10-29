using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TampaBayCoders.Services.CompensationCalculator
{
	public class CalculationResult
	{
		public decimal ActualHoursWorked { get; set; }
		public decimal CashCompensation { get; set; }
		public decimal TaxesPaidByEmployer { get; set; }
		public decimal TaxesPaidByEmployee { get; internal set; }
		public decimal RetirementContributionByEmployer { get; set; }
		public decimal RetirementContributionByEmployee { get; set; }
		public decimal HealthAndDentalPaidByEmployer { get; internal set; }
		public decimal TotalRetirementContribution => RetirementContributionByEmployer + RetirementContributionByEmployee;
		public decimal TotalBenefitsPaidByEmployer => HealthAndDentalPaidByEmployer + RetirementContributionByEmployer;
		public decimal TotalTaxPaid => TaxesPaidByEmployer + TaxesPaidByEmployee;
		public decimal TotalCompensation => CashCompensation + TaxesPaidByEmployer + TotalBenefitsPaidByEmployer;
		public decimal NetCompensation => CashCompensation - TaxesPaidByEmployee - RetirementContributionByEmployee;
		public decimal CompensationPerHour => TotalCompensation / ActualHoursWorked;
	}

	public class SalaryCalculationResult : CalculationResult
	{
		public decimal EquivalentWageRate { get; set; }
		public decimal EquivalentContractRate { get; set; }
	}

	public class WageCalculationResult : CalculationResult
	{
		public decimal EquivalentContractRate { get; set; }
		public decimal EquivalentSalary { get; set; }
	}

	public class ContractCalculationResult : CalculationResult
	{
		public decimal EquivalentWageRate { get; set; }
		public decimal EquivalentSalary { get; set; }
	}
}
