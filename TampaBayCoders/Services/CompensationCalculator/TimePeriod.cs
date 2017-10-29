using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TampaBayCoders.Services.CompensationCalculator
{
	public class TimePeriod
	{
		private TimePeriod(decimal factor) { Factor = factor; }

		public static TimePeriod Yearly { get; } = new TimePeriod(1);

		public static TimePeriod Monthly { get; } = new TimePeriod(12);

		public static TimePeriod Weekly { get; } = new TimePeriod(365.25m / 7m);

		public static TimePeriod Daily { get; } = new TimePeriod(365.25m);

		public decimal Factor { get; private set; }

		public static decimal Convert(decimal amount, TimePeriod from, TimePeriod to) => amount * from.Factor / to.Factor;
	}
}
