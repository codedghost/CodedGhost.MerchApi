using CodedGhost.Config;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Database;
using CoreCodedChatbot.Logging;
using CoreCodedChatbot.Secrets;

namespace CodedGhost.MerchApi;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var configService = new ConfigService();

        services.AddOptions();
        services.AddMemoryCache();

        services.AddChatbotConfigService();

        var secretService = new AzureKeyVaultService(
            configService.Get<string>("KeyVaultAppId"),
            configService.Get<string>("KeyVaultCertThumbPrint"),
            configService.Get<string>("KeyVaultBaseUrl"));

        secretService.Initialize().Wait();
        services.AddSingleton<ISecretService, AzureKeyVaultService>(provider => secretService);

        services
            .AddDbContextFactory()
            .AddChatbotNLog()
            .AddPrintfulClient(secretService);
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env, IServiceProvider serviceProvider)
    {

    }
}