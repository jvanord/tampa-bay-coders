using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using TampaBayCoders.Configuration;

namespace TampaBayCoders.Models
{
	public class SiteDiagnostics
	{
		public enum ResultStatus { Success, Fail }
		
		public class TestResult
		{
			public string Name { get; set; }
			public ResultStatus Status { get; set; }
			public string Message { get; set; }
			public static TestResult Success(string name, string message = null) => new TestResult { Name = name, Status = ResultStatus.Success, Message = message };
			public static TestResult Fail(string name, string message) => new TestResult { Name = name, Status = ResultStatus.Fail, Message = message };
			public override string ToString() => string.IsNullOrWhiteSpace(Message) ? Status.ToString() : Status + " - " + Message;
		}

		private SiteDiagnostics() { }

		public List<TestResult> Tests { get; set; } = new List<TestResult>();

		public static async Task<SiteDiagnostics> RunAsync(CosmosDbSettings settings)
		{
			var results = new SiteDiagnostics();
			var client = new DocumentClient(new Uri(settings.Endpoint), settings.Key);;
			try
			{
				await client.OpenAsync();
				results.Tests.Add(TestResult.Success("Connection Test", $"Connected to {settings.Endpoint}."));
			}
			catch (Exception ex)
			{
				results.Tests.Add(TestResult.Fail("Connection Test", ex.Message));
			}
			try
			{
				var response = await client.CreateDatabaseIfNotExistsAsync(new Database { Id = settings.DatabaseId });
				results.Tests.Add(TestResult.Success("Database Test", $"Database '{response.Resource.Id}' Exists."));
			}
			catch (Exception ex)
			{
				results.Tests.Add(TestResult.Fail("Database Test", ex.Message));
			}
			return results;
		}

		//public string DatabaseEndpoint { get; set; }
		//public string DatabaseId { get; set; }
		//public TestResult ConnectionTest { get; set; }
		//public TestResult DatabaseTest { get; set; }
	}
}
