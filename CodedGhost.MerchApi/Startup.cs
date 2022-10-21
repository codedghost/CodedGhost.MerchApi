using CodedGhost.Config;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Database;
using CoreCodedChatbot.Logging;
using CoreCodedChatbot.Secrets;
using Newtonsoft.Json;

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
            .AddPrintfulClient(secretService)
            .AddApplicationServices();

        services.AddRouting();

        services.AddControllers().AddNewtonsoftJson(x =>
            x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env, IServiceProvider serviceProvider)
    {
        app.UseMiddleware<ErrorHandlingMiddleware>();

        if (env.IsDevelopment() || env.IsEnvironment("Local"))
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors(builder => builder.WithOrigins("https://merch.codedghost.com", "http://localhost:3000")
            .AllowAnyHeader()
            .WithMethods("GET", "POST")
            .AllowCredentials());

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });
    }
}