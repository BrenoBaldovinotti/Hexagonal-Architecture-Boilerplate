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

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        return app;
    }
}
