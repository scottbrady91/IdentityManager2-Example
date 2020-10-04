using IdentityManager2.AspNetIdentity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ScottBrady91.IdentityManager2.Example
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IdentityDbContext>(opt => opt.UseInMemoryDatabase("test"));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityManager()
                .AddIdentityMangerService<AspNetCoreIdentityManagerService<IdentityUser, string, IdentityRole, string>>();
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
