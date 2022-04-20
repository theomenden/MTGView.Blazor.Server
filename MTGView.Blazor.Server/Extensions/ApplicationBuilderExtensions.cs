namespace MTGView.Blazor.Server.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseEnvironmentMiddleware(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseResponseCaching();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        return app;
    }
}