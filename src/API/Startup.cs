using API._Extensions;
using API.Middlewares;
using Serilog;

namespace API;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCustomControllers();
        services.AddCustomSwagger();
        services.AddCustomAuthentication(configuration);
        services.AddCustomAuthorization();
        services.AddCustomVersioning();
        services.AddCustomLogging();

        services.AddHealthChecks();
        
        services.AddCustomFluentValidation();
        
        services.AddCustomCORS();
        services.AddCustomCaching();
        
        services.AddCustomCQRS();

        services.AddCustomDomainServices();
        services.AddCustomInfrastructure(configuration);
        
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Global exception handling middleware
        app.UseMiddleware<GlobalExceptionMiddleware>();
        app.UseMiddleware<ValidationErrorMiddleware>();

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors("AllowAllOrigins");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health");
        });
    }
}
