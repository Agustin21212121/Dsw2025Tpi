using Microsoft.AspNetCore.Mvc;

namespace Dsw2025Tpi.Api.Extensions;

public static class ApiBehaviorExtensions
{
    public static IServiceCollection ConfigureCustomInvalidModelState(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(m => m.Value?.Errors.Count > 0)
                    .ToDictionary(
                        m => m.Key,
                        m => m.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? Array.Empty<string>()
                    );

                var result = new
                {
                    status = 400,
                    message = "Error de validación en los datos enviados."
                };

                return new BadRequestObjectResult(result);
            };
        });

        return services;
    }
}
