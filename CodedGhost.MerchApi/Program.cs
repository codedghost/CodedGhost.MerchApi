using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CodedGhost.MerchApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", optional: true)
                .Build();

            CreateHostBuilder(args, config).Run();
        }

        public static IHost CreateHostBuilder(string[] args, IConfigurationRoot config) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseUrls(config["server.urls"]);
                    builder.PreferHostingUrls(true);
                    builder.UseStartup<Startup>();
                })
                .Build();
    }
}