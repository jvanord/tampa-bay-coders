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
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("user_id"), Required]
		public string UserId { get; set; }

		[JsonProperty("display_name"), Display(Name = "Display Name"), MaxLength(256)]
		public string DisplayName { get; set; }

		[JsonProperty("email"), MaxLength(256)]
		public string Email { get; set; }

		[JsonProperty("photo_url"), MaxLength(256)]
		public string PhotoUrl { get; set; }

		[JsonProperty("website_url"), MaxLength(256)]
		public string WebsiteUrl { get; set; }

		[JsonProperty("github_url"), MaxLength(256)]
		public string GitHubUrl { get; set; }

		[JsonProperty("stackoverflow_cv_url"), MaxLength(256)]
		public string StackOverflowCvUrl { get; set; }

		[JsonProperty("linkedin_url"), MaxLength(256)]
		public string LinkedInUrl { get; set; }

		[JsonProperty("facebook_url"), MaxLength(256)]
		public string FacebookUrl { get; set; }

		[JsonProperty("twitter_handle"), MaxLength(256)]
		public string TwitterHandle { get; set; }

		[JsonProperty("summary"), Display(Name = ""), MaxLength(4096)]
		public string Summary { get; set; }
	}
}
