using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using TampaBayCoders.Data;
using TampaBayCoders.Models;

namespace TampaBayCoders.Services
{
	public class ProfileService : CosmosDbServiceBase
	{
		private ProfileService(CosmosDbConnection dataConnection) : base(dataConnection) { }

		public static ProfileService Connect(CosmosDbConnection dataConnection) => new ProfileService(dataConnection);

		public async Task<Profile> FindByUserId(string userId)
		{
			var collectionUri = await DataConnection.InitializeCollection(CollectionName.Profiles); // don't call a database that isn't ready
			var options = new FeedOptions { MaxItemCount = 1 };
			var query = DataConnection.DocumentClient.CreateDocumentQuery<Profile>(collectionUri, options).Where(p => p.UserId == userId); ;
			return await query.ToAsyncEnumerable().FirstOrDefault(); //; // this will execute the query asynchronously then get the first (hopefully only) result
		}

		public async Task<Profile> Create(Profile profile)
		{
			var collectionUri = await DataConnection.InitializeCollection(CollectionName.Profiles); // don't call a database that isn't ready
			var response = await DataConnection.DocumentClient.CreateDocumentAsync(collectionUri, profile);
			profile.Id = response.Resource.Id;
			return profile;
		}

		public Profile StubForUser(ClaimsPrincipal user)
		{
			var nameId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var nickname = user.FindFirst("nickname")?.Value;
			var profile = new Profile
			{
				UserId = nameId,
				DisplayName = (user.FindFirst(ClaimTypes.Name) ?? user.FindFirst(ClaimTypes.GivenName))?.Value,
				Email = user.FindFirst(ClaimTypes.Email)?.Value,
				PhotoUrl = user.FindFirst("picture")?.Value
			};
			var authenticationSource = nameId?.ToLower().Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
			switch (authenticationSource)
			{
				case "github": if (!string.IsNullOrWhiteSpace(nickname)) profile.GitHubUrl = $"https://github.com/{nickname}"; break;
				default: break;
			}
			return profile;
		}

		public async Task<Profile> ReadAsync(string id)
		{
			var collectionUri = await DataConnection.InitializeCollection(CollectionName.Profiles); // don't call a database that isn't ready
			var documentUri = DataConnection.CreateDocumentUri(CollectionName.Profiles, id);
			var response = await DataConnection.DocumentClient.ReadDocumentAsync<Profile>(documentUri);
			return response;
		}

		public async Task<Profile> UpdateAsync(Profile profile)
		{
			await DataConnection.InitializeCollection(CollectionName.Profiles); // don't call a database that isn't ready
			throw new NotImplementedException();
		}

		public async Task DeleteAsync(string id)
		{
			await DataConnection.InitializeCollection(CollectionName.Profiles); // don't call a database that isn't ready
			throw new NotImplementedException();
		}

		public async Task<List<Profile>> ListAsync(Expression<Func<Profile, bool>> predicate = null)
		{
			await DataConnection.InitializeCollection(CollectionName.Profiles); // don't call a database that isn't ready
			throw new NotImplementedException();
		}
	}
}
