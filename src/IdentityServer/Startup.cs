using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentityServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var admin = new TestUser
            {
                SubjectId = Guid.NewGuid().ToString(),
                Username = "scott",
                Password = "scott",
                Claims = {new Claim("role", "IdentityManagerAdministrator") }
            };

            var client = new Client
            {
                ClientId = "identitymanager2",
                ClientName = "IdentityManager2",
                AllowedGrantTypes = GrantTypes.Implicit,
                RedirectUris = {"http://localhost:5000/signin-oidc"},
                AllowedScopes = {"openid", "profile", "roles"}
            };

            var roles = new IdentityResource("roles", new List<string> {"role"});

            services.AddIdentityServer()
                .AddTestUsers(new List<TestUser> {admin})
                .AddInMemoryIdentityResources(new List<IdentityResource> {new IdentityResources.OpenId(), new IdentityResources.Profile(), roles})
                .AddInMemoryApiResources(new List<ApiResource>())
                .AddInMemoryClients(new List<Client> {client})
                .AddDeveloperSigningCredential(false);
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            app.UseDeveloperExceptionPage();

            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}