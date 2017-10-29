using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TampaBayCoders.Models
{
	public class SiteDiagnosticsViewModel
	{
		public enum ResultStatus { Success, Fail }
		
		public class TestResult
		{
			public ResultStatus Status { get; set; }
			public string Message { get; set; }
		}

		public string DatabaseEndpoint { get; set; }
		public string DatabaseId { get; set; }
		public TestResult ConnectionTest { get; internal set; }
	}
}
