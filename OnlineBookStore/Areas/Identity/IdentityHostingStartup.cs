using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(OnlineBookStore.Areas.Identity.IdentityHostingStartup))]
namespace OnlineBookStore.Areas.Identity
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