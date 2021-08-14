using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(AdminPanel.Areas.Identity.IdentityHostingStartup))]
namespace AdminPanel.Areas.Identity
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