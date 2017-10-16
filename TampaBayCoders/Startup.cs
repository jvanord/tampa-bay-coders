﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace TampaBayCoders
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile("secrets.json", optional: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
				.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		public IConfigurationRoot Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// Add Authentication Services
			services.AddAuthentication(options => options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);

			// Add Framework Services
			services.AddMvc();
			services.AddRouting(option => option.LowercaseUrls = true);

			// Enable IOptions Injection (see Configure method)
			services.AddOptions();

			// Add Auth0 Configuration Settings
			services.Configure<Auth0Settings>(Configuration.GetSection("Auth0"));

			// Add CosmosDB Settings
			services.Configure<CosmosDbSettings>(Configuration.GetSection("CosmosDb"));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<Auth0Settings> auth0Settings)
		{
			// Configure Logging
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			// Configure Friendly Errors
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseBrowserLink();
			}
			else
			{
				app.UseExceptionHandler("/home/error");
			}

			// Configure Static File Serving
			app.UseStaticFiles();

			// Enable Cookie Authentication Middleware
			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AutomaticAuthenticate = true,
				AutomaticChallenge = true
			});

			// Add OpenID Connect (OIDC) Configuration
			var options = new OpenIdConnectOptions("Auth0")
			{
				// Set the authority to your Auth0 domain
				Authority = $"https://{auth0Settings.Value.Domain}",

				// Configure the Auth0 Client ID and Client Secret
				ClientId = auth0Settings.Value.ClientId,
				ClientSecret = auth0Settings.Value.ClientSecret,

				// Do not automatically authenticate and challenge
				AutomaticAuthenticate = false,
				AutomaticChallenge = false,

				// Set response type to code
				ResponseType = "code",

				// Set the callback path (redirect destination after auth0 login)
				CallbackPath = new PathString("/signin"),

				// Configure the Claims Issuer to be Auth0
				ClaimsIssuer = "Auth0",

				Events = new OpenIdConnectEvents
				{
					// Configure Access Tokens for any APIs secured by Auth0
					//OnRedirectToIdentityProvider = context =>
					//{
					//	context.ProtocolMessage.SetParameter("audience", "{external api, e.g. https://www.indasysllc.com/api}");
					//	return Task.CompletedTask;
					//},

					// Handle Logout
					OnRedirectToIdentityProviderForSignOut = context =>
					{
						var logoutUri = $"https://{auth0Settings.Value.Domain}/v2/logout?client_id={auth0Settings.Value.ClientId}";
						var postLogoutUri = context.Properties.RedirectUri;
						if (!string.IsNullOrEmpty(postLogoutUri))
						{
							if (postLogoutUri.StartsWith("/"))
							{
								// transform to absolute
								var request = context.Request;
								postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
							}
							logoutUri += $"&returnTo={ Uri.EscapeDataString(postLogoutUri)}";
						}
						context.Response.Redirect(logoutUri);
						context.HandleResponse();
						return Task.CompletedTask;
					}
				}
			};
			options.Scope.Clear();
			options.Scope.Add("openid");
			app.UseOpenIdConnectAuthentication(options);

			// Configure MVC
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
