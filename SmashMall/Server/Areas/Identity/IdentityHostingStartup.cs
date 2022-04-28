using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(SmashMall.Server.Areas.Identity.IdentityHostingStartup))]
namespace SmashMall.Server.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}