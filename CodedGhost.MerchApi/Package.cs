using CodedGhost.MerchApi.Interfaces.Services;
using CodedGhost.MerchApi.Services;
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

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<ICategoryService, CategoryService>();
        services.AddSingleton<IProductService, ProductService>();

        return services;
    }
}