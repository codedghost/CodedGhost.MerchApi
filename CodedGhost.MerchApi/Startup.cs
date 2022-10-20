using CodedGhost.Config;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Database;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context;
using CoreCodedChatbot.Logging;
using CoreCodedChatbot.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;

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

        services.AddRouting();

        services.AddControllers().AddNewtonsoftJson(x =>
            x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env, IServiceProvider serviceProvider)
    {
        using (var context = (ChatbotContext)serviceProvider.GetService<IChatbotContextFactory>().Create())
        {
            context.Database.Migrate();
        }

        app.UseMiddleware<ErrorHandlingMiddleware>();

        if (env.IsDevelopment() || env.IsEnvironment("Local"))
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });
    }
}