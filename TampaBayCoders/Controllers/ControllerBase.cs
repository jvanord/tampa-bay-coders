using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;

namespace TampaBayCoders.Controllers
{
    public abstract class ControllerBase : Controller
    {
		[Obsolete("Use Singleton Connection")]
		protected CosmosDbSettings ConnectionSettings;

		protected static DocumentClient DatabaseConnection { get; private set; }

		public ControllerBase(IOptions<CosmosDbSettings> cosmosDbSettings)
		{
			ConnectionSettings = cosmosDbSettings.Value;
		}
	}
}