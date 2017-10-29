using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TampaBayCoders.Services.CompensationCalculator
{
	public class TaxProfile
	{
		public decimal FixedBaseAmount { get; set; }
		public decimal TaxBracketThreshold { get; set; }
		public decimal TaxBracketRate { get; set; }
		public decimal SocialSecurityRate { get; set; }
		public decimal MedicareRate { get; set; }
		public static TaxProfile Current()
		{
			return new TaxProfile
			{
				FixedBaseAmount = 1865m,
				TaxBracketThreshold = 18650,
				TaxBracketRate = 0.15m, // employee only
				SocialSecurityRate = .062m, // each
				MedicareRate = .0145m // each
			};
		}
	}
}
