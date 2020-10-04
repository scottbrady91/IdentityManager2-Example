using System.IdentityModel.Tokens.Jwt;
using IdentityManager2.AspNetIdentity;
using IdentityManager2.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ScottBrady91.IdentityManager2.Example
{
    public class StartupWithIdentityServer
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IdentityDbContext>(opt => opt.UseInMemoryDatabase("test"));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityManager(opt =>
                    opt.SecurityConfiguration =
                        new SecurityConfiguration
                        {
                            HostAuthenticationType = "cookie",
                            HostChallengeType = "oidc",
                            AdditionalSignOutType = "oidc"
                        })
                .AddIdentityMangerService<AspNetCoreIdentityManagerService<IdentityUser, string, IdentityRole, string>>();

            // To make role claim type == "role"
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication()
                .AddCookie("cookie")
                .AddOpenIdConnect("oidc", opt =>
                {
                    opt.Authority = "https://localhost:5001";
                    opt.ClientId = "identitymanager2";
                    
                    // default: openid & profile
                    opt.Scope.Add("roles");

                    opt.RequireHttpsMetadata = false; // dev only
                    opt.SignInScheme = "cookie";
                });
        }
        
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseIdentityManager();

            app.UseEndpoints(builder => builder.MapDefaultControllerRoute());
        }
    }
}
