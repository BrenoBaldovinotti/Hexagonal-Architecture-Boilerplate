using API.Middlewares;

namespace API._Extensions;

public static class AppExtensions
{
    public static IApplicationBuilder UseCustomPipeline(this IApplicationBuilder app)
    {
        var env = app.ApplicationServices.GetService<IWebHostEnvironment>();

        if (env != null && env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hexagonal Architecture API v1");
            c.RoutePrefix = string.Empty;
        });

        app.UseMiddleware<GlobalExceptionMiddleware>();
        app.UseMiddleware<ValidationErrorMiddleware>();

        app.UseCors("AllowAllOrigins");

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        return app;
    }
}
