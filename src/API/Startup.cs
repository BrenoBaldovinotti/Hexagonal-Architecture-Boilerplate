using API._Extensions;
using Microsoft.Extensions.Configuration;

namespace API;

public class Startup(IConfiguration configuration)
{
    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCustomLogging();
        services.AddCustomCORS();
        services.AddCustomControllers();
        services.AddCustomAuthentication(configuration["Jwt:Secret"]);
        services.AddCustomFluentValidation();
        services.AddCustomCaching();
        services.AddCustomCQRS();
        services.AddCustomDomainServices();
        services.AddCustomInfrastructure(configuration);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app)
    {
        app.UseCustomPipeline();
    }
}
