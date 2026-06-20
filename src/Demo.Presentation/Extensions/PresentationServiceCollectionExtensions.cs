using Demo.Presentation.ExceptionHandling;

namespace Demo.Presentation.Extensions;

public static class PresentationServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddScoped<ValidationExceptionHandler>();
        return services;
    }

    public static WebApplication UsePresentationPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ValidationExceptionHandler>();
        app.UseHttpsRedirection();
        return app;
    }
}
