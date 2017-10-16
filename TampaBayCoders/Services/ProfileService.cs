using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;

namespace TampaBayCoders.Services
{
	public class ProfileService
	{
		private DocumentClient _azureDocumentClient;

		internal ProfileService(CosmosDbSettings settings)
		{
			if (settings == null) throw new Exception("Database Settings Not Found");
			if (string.IsNullOrWhiteSpace(settings.Endpoint)) throw new Exception("Database Endpoint Not Found");
			if (string.IsNullOrWhiteSpace(settings.Key)) throw new Exception("Database Authorization Key Not Found");
			_azureDocumentClient = new DocumentClient(new Uri(settings.Endpoint), settings.Key); 
		}

	}
}
