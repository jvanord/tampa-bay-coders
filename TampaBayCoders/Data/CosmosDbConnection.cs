using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using TampaBayCoders.Configuration;

namespace TampaBayCoders.Data
{
	public enum CollectionName { Profiles }

	public class CosmosDbConnection
	{
		private static CosmosDbConnection _singletonConnection;

		private Dictionary<CollectionName, Uri> _collectionUris = new Dictionary<CollectionName, Uri>();

		internal CosmosDbSettings ConnectionSettings { get; private set; }

		internal DocumentClient DocumentClient { get; private set; }

		private CosmosDbConnection(CosmosDbSettings connectionSettings)
		{
			if (connectionSettings == null) throw new Exception("Database Settings Not Found");
			if (string.IsNullOrWhiteSpace(connectionSettings.Endpoint)) throw new Exception("Database Endpoint Not Found");
			if (string.IsNullOrWhiteSpace(connectionSettings.Key)) throw new Exception("Database Authorization Key Not Found");
			ConnectionSettings = connectionSettings;
			DocumentClient = new DocumentClient(new Uri(connectionSettings.Endpoint), connectionSettings.Key);
			DocumentClient.OpenAsync(); // to avoid startup latency
		}

		internal static CosmosDbConnection Singleton(CosmosDbSettings connectionSettings)
		{
			if (_singletonConnection == null || _singletonConnection.ConnectionSettings.Endpoint != connectionSettings.Endpoint)
				_singletonConnection = new CosmosDbConnection(connectionSettings);
			return _singletonConnection;
		}

		internal async Task<Uri> InitializeCollection(CollectionName collection)
		{
			if (_collectionUris.ContainsKey(collection)) return _collectionUris[collection];
			var databaseUri = UriFactory.CreateDatabaseUri(ConnectionSettings.DatabaseId);
			await DocumentClient.CreateDatabaseIfNotExistsAsync(new Database { Id = ConnectionSettings.DatabaseId });
			await DocumentClient.CreateDocumentCollectionIfNotExistsAsync(databaseUri, new DocumentCollection { Id = collection.ToString() });
			lock (_collectionUris)
			{
				var uri = UriFactory.CreateDocumentCollectionUri(ConnectionSettings.DatabaseId, collection.ToString());
				_collectionUris[collection] = uri;
				return uri;
			}
		}

		internal Uri CollectionUri(CollectionName collection) => _collectionUris[collection];
	}
}
