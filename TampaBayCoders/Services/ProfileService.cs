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

		public async Task<IAsyncEnumerable<Profile>> FindByUserNameAsync(string name)
		{
			var collectionUri = await DataConnection.InitializeCollection(CollectionName.Profiles); // don't call a database that isn't ready
			var options = new FeedOptions { MaxItemCount = 1 };
			var query = DataConnection.DocumentClient.CreateDocumentQuery<Profile>(collectionUri, options).Where(p => p.UserName == name);
			return query.ToAsyncEnumerable(); // this will execute the query asynchronously
		}

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
			return profile;
		}

		[Obsolete("Don't create an automatically generated profile that the user hasn't reviewed first.")]
		internal async Task<Profile> Create(ClaimsPrincipal user)
		{
			var profile = new Profile
			{
				UserId = getClaim(user, true, "sub", "user_id", "nameidentifier"),
				UserName = user.Identity.Name,
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
			await DataConnection.InitializeCollection(CollectionName.Profiles); // don't call a database that isn't ready
			throw new NotImplementedException();
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
