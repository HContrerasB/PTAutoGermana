using Microsoft.Extensions.Options;
using ZombieDefenseSystem.Api.Configuration;

namespace ZombieDefenseSystem.Api.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ApiKeyOptions _apiKeyOptions;

        public ApiKeyMiddleware(
            RequestDelegate next,
            IOptions<ApiKeyOptions> apiKeyOptions)
        {
            _next = next;
            _apiKeyOptions = apiKeyOptions.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Options)
            {
                await _next(context);
                return;
            }
            if (!context.Request.Headers.TryGetValue(_apiKeyOptions.HeaderName, out var extractedApiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = $"No se proporcionó el header requerido '{_apiKeyOptions.HeaderName}'."
                });
                return;
            }

            if (string.IsNullOrWhiteSpace(_apiKeyOptions.Key))
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = "La API Key no está configurada correctamente en el servidor."
                });
                return;
            }

            if (!string.Equals(extractedApiKey, _apiKeyOptions.Key, StringComparison.Ordinal))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = "API Key inválida."
                });
                return;
            }

            await _next(context);
        }
    }
}
