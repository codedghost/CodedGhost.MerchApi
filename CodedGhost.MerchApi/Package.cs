using CoreCodedChatbot.Secrets;
using PrintfulLib.ExternalClients;
using PrintfulLib.Interfaces.ExternalClients;

namespace CodedGhost.MerchApi;

public static class Package
{

    public static IServiceCollection AddPrintfulClient(this IServiceCollection services, ISecretService secretService)
    {
        var printfulClient = new PrintfulClient(secretService.GetSecret<string>("PrintfulAPIKey"));

        services.AddSingleton<IPrintfulClient>(printfulClient);

        return services;
    }
}