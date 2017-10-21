using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TampaBayCoders.Models
{

	public class Profile
	{
		[JsonProperty("user_id"), Required]
		public string UserId { get; set; }

		[JsonProperty("user_name"), Required]
		public string UserName { get; set; }

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
