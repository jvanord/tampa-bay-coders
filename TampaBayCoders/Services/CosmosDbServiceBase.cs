using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;

namespace TampaBayCoders.Services
{
	public abstract class CosmosDbServiceBase
	{
		private Task _databaseReadyTask;

		internal CosmosDbServiceBase(CosmosDbSettings settings)
		{
			if (settings == null) throw new Exception("Database Settings Not Found");
			if (string.IsNullOrWhiteSpace(settings.Endpoint)) throw new Exception("Database Endpoint Not Found");
			if (string.IsNullOrWhiteSpace(settings.Key)) throw new Exception("Database Authorization Key Not Found");
			ConnectionSettings = settings;
			DocumentDb = new DocumentClient(new Uri(settings.Endpoint), settings.Key);
			_databaseReadyTask = Initialize();
		}

		protected CosmosDbSettings ConnectionSettings { get; private set; }

		protected DocumentClient DocumentDb { get; private set; }

		public bool DatabaseReady => (_databaseReadyTask?.IsCompleted).GetValueOrDefault();

		protected virtual async Task Initialize() { }
	}
}
