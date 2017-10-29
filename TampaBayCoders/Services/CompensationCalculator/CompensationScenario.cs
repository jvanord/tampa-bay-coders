using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TampaBayCoders.Services.CompensationCalculator
{
	public class CompensationScenario
	{
		private static readonly decimal _weeksPerYear = 365.25m / 7;

		public int DaysOffPerYear { get; private set; }

		public TaxProfile TaxProfile { get; private set; }

		public BenefitsProfile BenefitsProfile { get; private set; }

		public CompensationScenario(int daysOffPerYear, TaxProfile taxProfile, BenefitsProfile benefitsProfile)
		{
			DaysOffPerYear = daysOffPerYear;
			TaxProfile = taxProfile;
			BenefitsProfile = benefitsProfile;
		}

		public static CompensationScenario TypicalFullTimeJob()
			=> new CompensationScenario(25, TaxProfile.Current(), BenefitsProfile.Typical());

		public static CompensationScenario Current()
			=> new CompensationScenario(25, TaxProfile.Current(), BenefitsProfile.None());

		public SalaryCalculationResult CalculateSalary(decimal salary, decimal bonus = 0)
		{
			if (salary < 1)
				throw new Exception("Salary too low to calculate.");

			var hoursAvailable = 40 * _weeksPerYear;
			var hoursOff = 8 * DaysOffPerYear;
			var hoursWorked = hoursAvailable - hoursOff;
			var hoursPaid = hoursAvailable;

			var taxableAmount = salary + bonus;
			var federalTax = TaxProfile.FixedBaseAmount + TaxProfile.TaxBracketRate * (taxableAmount - TaxProfile.TaxBracketThreshold);
			var employerTax = (TaxProfile.MedicareRate + TaxProfile.SocialSecurityRate) * taxableAmount;
			var employeeTax = federalTax + employerTax;

			var employerRetirement = Math.Min(BenefitsProfile.RetirementContributionRate, BenefitsProfile.RetirementMatchingContributionRate) * salary;
			var employeeRetirement = BenefitsProfile.RetirementContributionRate * salary;

			var result = new SalaryCalculationResult
			{
				ActualHoursWorked = hoursWorked,
				CashCompensation = taxableAmount,
				TaxesPaidByEmployer = employerTax,
				TaxesPaidByEmployee = employeeTax,
				RetirementContributionByEmployer = employerRetirement,
				RetirementContributionByEmployee = employeeRetirement,
				HealthAndDentalPaidByEmployer = BenefitsProfile.EmployerHealthAndDentalCostsPaid
			};

			result.EquivalentContractRate = ContractRateRequired(result.TotalCompensation, result.ActualHoursWorked);
			result.EquivalentWageRate = WageRequired(result.TotalCompensation);
			return result;
		}

		public decimal SalaryRequired(decimal totalCompensation, decimal bonus = 0)
		{
			/*
				For salary, total compensation includes bonus and benefits.
				Total Compensation
					= salary
					+ bonus
					+ (TaxProfile.MedicareRate + TaxProfile.SocialSecurityRate) * wagesPerYear 
					+ Math.Min(BenefitsProfile.RetirementContributionRate, BenefitsProfile.RetirementMatchingContributionRate) * wagesPerYear 
					+ BenefitsProfile.EmployerHealthAndDentalCostsPaid;
				C = s + b + (TM + TS)(s + b) + rs + BE = (1 + TM + TS + r)s + (TM + TS)b + (TM)(TS) + BE
				s = (C - BE - (TM + TS)b) / (1 + TM + TS + r)
			*/
			var r = Math.Min(BenefitsProfile.RetirementContributionRate, BenefitsProfile.RetirementMatchingContributionRate);
			var num = totalCompensation - BenefitsProfile.EmployerHealthAndDentalCostsPaid - bonus * (TaxProfile.MedicareRate + TaxProfile.SocialSecurityRate);
			var den = 1 + TaxProfile.MedicareRate + TaxProfile.SocialSecurityRate + r;
			return num / den; // Paid for Time Off so No Adjustment for Hours
		}

		public WageCalculationResult CalculateHourlyWage(decimal wagesPerHour)
		{
			if (wagesPerHour < 1)
				throw new Exception("Wages too low to calculate.");

			var hoursAvailable = 40 * _weeksPerYear;
			var hoursOff = 8 * DaysOffPerYear;
			var hoursWorked = hoursAvailable - hoursOff;
			var hoursPaid = hoursAvailable;

			var wagesPerYear = hoursPaid * wagesPerHour;
			var taxableAmount = wagesPerYear;
			var federalTax = TaxProfile.FixedBaseAmount + TaxProfile.TaxBracketRate * (taxableAmount - TaxProfile.TaxBracketThreshold);
			var employerTax = (TaxProfile.MedicareRate + TaxProfile.SocialSecurityRate) * taxableAmount;
			var employeeTax = federalTax + employerTax;

			var employerRetirement = Math.Min(BenefitsProfile.RetirementContributionRate, BenefitsProfile.RetirementMatchingContributionRate) * wagesPerYear;
			var employeeRetirement = BenefitsProfile.RetirementContributionRate * wagesPerYear;

			var result = new WageCalculationResult
			{
				ActualHoursWorked = 40 * _weeksPerYear - 8 * DaysOffPerYear,
				CashCompensation = taxableAmount,
				TaxesPaidByEmployer = employerTax,
				TaxesPaidByEmployee = employeeTax,
				RetirementContributionByEmployer = employerRetirement,
				RetirementContributionByEmployee = employeeRetirement,
				HealthAndDentalPaidByEmployer = BenefitsProfile.EmployerHealthAndDentalCostsPaid
			};

			result.EquivalentSalary = SalaryRequired(result.TotalCompensation, 0);
			result.EquivalentContractRate = ContractRateRequired(result.TotalCompensation, result.ActualHoursWorked);
			return result;
		}

		public decimal WageRequired(decimal totalCompensation)
		{
			/*
				For wages, total compensation includes benefits.
				Total Compensation
					= wagesPerYear 
					+ (TaxProfile.MedicareRate + TaxProfile.SocialSecurityRate) * wagesPerYear 
					+ Math.Min(BenefitsProfile.RetirementContributionRate, BenefitsProfile.RetirementMatchingContributionRate) * wagesPerYear 
					+ BenefitsProfile.EmployerHealthAndDentalCostsPaid;
				C = w + (TM + TS)w + rw + BE = (1 + TM + TS + r)w + BE
				w = (C - BE) / (1 + TM + TS + r)
			*/
			var contributionRate = Math.Min(BenefitsProfile.RetirementContributionRate, BenefitsProfile.RetirementMatchingContributionRate);
			var wagesPerYear = (totalCompensation - BenefitsProfile.EmployerHealthAndDentalCostsPaid)
				/ (1 + TaxProfile.MedicareRate + TaxProfile.SocialSecurityRate + contributionRate);

			return wagesPerYear / (40 * _weeksPerYear); // Paid for Time Off
		}

		public ContractCalculationResult CalculateContractRate(decimal ratePerHour)
		{
			if (ratePerHour < 1)
				throw new Exception("Rate too low to calculate.");

			var hoursAvailable = 40 * _weeksPerYear;
			var hoursOff = 8 * DaysOffPerYear;
			var hoursWorked = hoursAvailable - hoursOff;
			var hoursPaid = hoursWorked;

			var totalMoneyPaid = hoursPaid * ratePerHour;
			var taxableAmount = totalMoneyPaid;
			var federalTax = TaxProfile.FixedBaseAmount + TaxProfile.TaxBracketRate * (taxableAmount - TaxProfile.TaxBracketThreshold);
			var employerTax = 0;
			var employeeTax = federalTax + 2 * (TaxProfile.MedicareRate + TaxProfile.SocialSecurityRate) * taxableAmount;

			var employerRetirement = 0;
			var employeeRetirement = BenefitsProfile.RetirementContributionRate * totalMoneyPaid;

			var result = new ContractCalculationResult
			{
				ActualHoursWorked = 40 * _weeksPerYear - 8 * DaysOffPerYear,
				CashCompensation = taxableAmount,
				TaxesPaidByEmployer = employerTax,
				TaxesPaidByEmployee = employeeTax,
				RetirementContributionByEmployer = employerRetirement,
				RetirementContributionByEmployee = employeeRetirement,
				HealthAndDentalPaidByEmployer = 0
			};

			result.EquivalentSalary = SalaryRequired(result.TotalCompensation, 0);
			result.EquivalentWageRate = WageRequired(result.TotalCompensation);
			return result;
		}

		public decimal ContractRateRequired(decimal totalCompensation, decimal hoursWorked)
		{
			/*
				For contract rate, total compensation *does not* include bonus or benefits and must be adjusted for hours worked
				Total Compensation = hoursPaid * ratePerHour
				C = h * r
				r = C / h
			*/
			return totalCompensation / hoursWorked;
		}
	}
}
