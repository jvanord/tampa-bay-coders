using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TampaBayCoders.Models
{
	public enum ProfileType
	{
		[Display(Name ="Just Visiting")]
		Visitor,

		[Display(Name = "a Coder")]
		Coder,

		[Display(Name = "a Recruiter")]
		Recruiter,

		[Display(Name = "a Software Company or Team")]
		Team
	}

	public class Profile
	{
		[JsonProperty("id"), Display(Name = "Profile ID")]
		public string Id { get; set; }

		[JsonProperty("user_id"), Required, Display(Name = "User ID")]
		public string UserId { get; set; }

		[JsonProperty("type"), Display(Name = "Profile Type")]
		public ProfileType Type { get; set; }

		[JsonProperty("display_name"), Required, Display(Name = "Display Name"), MaxLength(256)]
		public string DisplayName { get; set; }

		[JsonProperty("email"), Required, MaxLength(256)]
		public string Email { get; set; }

		[JsonProperty("photo_url"), Display(Name = "Photo"), MaxLength(256)]
		[Url(ErrorMessage = "This needs to be a URL (staring with the protocol).")]
		public string PhotoUrl { get; set; }

		[JsonProperty("website_url"), Display(Name = "Website"), MaxLength(256)]
		[Url(ErrorMessage = "This needs to be a URL (staring with the protocol).")]
		public string WebsiteUrl { get; set; }

		[JsonProperty("github_url"), Display(Name = "Github"), MaxLength(256)]
		[Url(ErrorMessage = "This needs to be a URL (staring with the protocol).")]
		public string GitHubUrl { get; set; }

		[JsonProperty("stackoverflow_cv_url"), Display(Name = "Stack Overflow"), MaxLength(256)]
		[Url(ErrorMessage = "This needs to be a URL (staring with the protocol).")]
		//[RegularExpression("", ErrorMessage = "That doesn't look like a Stack Overflow URL")]
		public string StackOverflowUrl { get; set; }

		[JsonProperty("linkedin_url"), Display(Name = "Linked-In"), MaxLength(256)]
		[Url(ErrorMessage = "This needs to be a URL (staring with the protocol).")]
		public string LinkedInUrl { get; set; }

		[JsonProperty("facebook_url"), Display(Name = "Facebook"), MaxLength(256)]
		[Url(ErrorMessage = "This needs to be a URL (staring with the protocol).")]
		public string FacebookUrl { get; set; }

		[JsonProperty("twitter_handle"), Display(Name = "Twitter"), MaxLength(256)]
		public string TwitterHandle { get; set; }

		[JsonProperty("summary"), MaxLength(4096)]
		public string Summary { get; set; }
	}
}
