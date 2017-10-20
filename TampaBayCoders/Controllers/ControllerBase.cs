using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using TampaBayCoders.Configuration;
using TampaBayCoders.Data;

namespace TampaBayCoders.Controllers
{
    public abstract class ControllerBase : Controller
    {
		protected static CosmosDbConnection SharedDataConnection { get; private set; }

		public ControllerBase(IOptions<CosmosDbSettings> cosmosDbSettings)
		{
			SharedDataConnection = CosmosDbConnection.Singleton(cosmosDbSettings.Value);
		}
	}
}