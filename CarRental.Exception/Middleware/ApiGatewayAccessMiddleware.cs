using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CarRental.Exception.Middleware
{
    public class ApiGatewayAccessMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public ApiGatewayAccessMiddleware(
            RequestDelegate next,
            IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var gatewaySecret = _configuration["GatewayAccess:InternalSecret"];

            if (!context.Request.Headers.TryGetValue("X-Internal-Gateway", out var headerValue) ||
                string.IsNullOrWhiteSpace(gatewaySecret) ||
                headerValue != gatewaySecret)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Forbidden",
                    message = "Request rejected. Access is allowed only through the API Gateway."
                });

                return;
            }

            await _next(context);
        }
    }
}