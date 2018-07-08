using IdentityServer;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ScottBrady91.IdentityManager2.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
