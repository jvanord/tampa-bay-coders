using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace TampaBayCoders.Services
{
	public class ProfileService : CosmosDbServiceBase
	{

		public ProfileService(CosmosDbSettings settings) : base(settings) { }

		protected override async Task Initialize()
		{
			if (DatabaseReady) return;
			try
			{
				var databaseUri = UriFactory.CreateDatabaseUri(ConnectionSettings.DatabaseId);
				var database = await DocumentDb.CreateDatabaseIfNotExistsAsync(new Database { Id = ConnectionSettings.DatabaseId });
				var collection = await DocumentDb.CreateDocumentCollectionIfNotExistsAsync(databaseUri, new DocumentCollection { Id = ConnectionSettings.ProfileCollectionId });
			}
			catch (DocumentClientException ex)
			{
				// log data error
			}
			catch(Exception ex)
			{
				// log generic error
			}
		}

		public async Task<Profile> Create(Profile profile)
		{
			await Initialize(); // don't call a database that isn't ready
			var collectionUri = UriFactory.CreateDocumentCollectionUri(ConnectionSettings.DatabaseId, ConnectionSettings.ProfileCollectionId);
			var response = await DocumentDb.CreateDocumentAsync(collectionUri, profile);
			return profile;
		}

		[Obsolete("Don't create an automatically generated profile that the user hasn't reviewed first.")]

		internal async Task<Profile> Create(ClaimsPrincipal user)
		{
			var profile = new Profile
			{
				UserId = getClaim(user, true, "sub", "user_id", "nameidentifier"),
				DisplayName = getClaim(user, true, "name", "nickname", "givenname"),
				Email = getClaim(user, true, "email", "emailaddress"),
				PhotoUrl = getClaim(user, false, "picture"),
				Claims = user.Claims.Select(c => new KeyValuePair<string, string>(c.Type, c.Value))
			};
			return await Create(profile);
		}

		private string getClaim(ClaimsPrincipal user, bool errorIfNotFound, params string[] types)
		{
			if (user == null) throw new NullReferenceException("No user specified.");
			if (!types.Any()) throw new Exception("No claims type specified.");
			Claim match;
			foreach (var type in types)
			{
				match = user.Claims.FirstOrDefault(c => c.Type == type || c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/" + type);
				if (match != null) return match.Value;
			}
			if (errorIfNotFound)
				throw new Exception($"Could not find claim(s) for user: {string.Join(" | ", types)}");
			return null;
		}

		public async Task<Profile> ReadAsync(string id)
		{
			await Initialize(); // don't call a database that isn't ready
			throw new NotImplementedException();
		}

		public async Task<Profile> UpdateAsync(Profile profile)
		{
			await Initialize(); // don't call a database that isn't ready
			throw new NotImplementedException();
		}

		public async Task DeleteAsync(string id)
		{
			await Initialize(); // don't call a database that isn't ready
			throw new NotImplementedException();
		}

		public async Task<List<Profile>> ListAsync(Expression<Func<Profile, bool>> predicate = null)
		{
			await Initialize(); // don't call a database that isn't ready
			throw new NotImplementedException();
		}
	}

	public class Profile
	{
		[JsonProperty("user_id"), Required]
		public string UserId { get; set; }

		[JsonProperty("display_name"), Display(Name = "Display Name"), MaxLength(256)]
		public string DisplayName { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("photo_url")]
		public string PhotoUrl { get; set; }

		[JsonProperty("summary"), Display(Name = ""), MaxLength(256)]
		public string Summary { get; set; }

		[JsonProperty("linkedin_url")]
		public string LinkedInUrl { get; set; }

		[JsonProperty("facebook_url")]
		public string FacebookUrl { get; set; }

		[JsonProperty("stackoverflow_cv_url")]
		public string StackOverflowCvUrl { get; set; }

		[JsonProperty("github_url")]
		public string GitHubUrl { get; set; }

		[JsonProperty("twitter_handle")]
		public string TwitterHandle { get; set; }

		[JsonProperty("claims")]
		public IEnumerable<KeyValuePair<string, string>> Claims { get; internal set; }
	}
}
