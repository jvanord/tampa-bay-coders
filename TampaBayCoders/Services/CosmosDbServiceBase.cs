using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using TampaBayCoders.Data;

namespace TampaBayCoders.Services
{
	public abstract class CosmosDbServiceBase
	{
		protected CosmosDbConnection DataConnection { get; private set; }

		internal CosmosDbServiceBase(CosmosDbConnection connection) { DataConnection = connection; }
	}
}
