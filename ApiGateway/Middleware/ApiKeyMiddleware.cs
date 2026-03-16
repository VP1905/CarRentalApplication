namespace ApiGateway.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public ApiKeyMiddleware(
            RequestDelegate next,
            IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var expectedApiKey = _configuration["ApiKeySettings:ApiKey"];

            if (!context.Request.Headers.TryGetValue("X-Api-Key", out var providedApiKey) ||
                string.IsNullOrWhiteSpace(expectedApiKey) ||
                providedApiKey != expectedApiKey)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Unauthorized",
                    message = "Missing or invalid API key."
                });

                return;
            }

            await _next(context);
        }
    }
}