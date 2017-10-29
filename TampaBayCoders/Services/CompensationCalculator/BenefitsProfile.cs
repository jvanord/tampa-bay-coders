using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TampaBayCoders.Services.CompensationCalculator
{
	public class BenefitsProfile
	{
		public decimal RetirementContributionRate { get; set; }

		public decimal RetirementMatchingContributionRate { get; set; }

		public decimal EmployerHealthAndDentalCostsPaid { get; set; }

		public static BenefitsProfile Typical() => new BenefitsProfile
		{
			RetirementContributionRate = .03m,
			RetirementMatchingContributionRate = .03m,
			EmployerHealthAndDentalCostsPaid = 1000
		};

		public static BenefitsProfile None() => new BenefitsProfile
		{
			RetirementContributionRate = 0m,
			RetirementMatchingContributionRate = 0m,
			EmployerHealthAndDentalCostsPaid = 0
		};
	}
}
