using System.Security.Claims;

namespace Ecommerse_shoes_backend.Middleware
{
    public class GetUserIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GetUserIdMiddleware> _logger;

        public GetUserIdMiddleware(RequestDelegate next,ILogger<GetUserIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if(!context.Request.Headers.ContainsKey("Authorization"))
            {
                await _next(context);
                return;
            }

            if (context.User.Identity?.IsAuthenticated == true)
            {
                _logger.LogInformation($"{context.User.Identity?.IsAuthenticated}checking");
                var UserIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
                _logger.LogInformation($"{UserIdClaim} userId");
                if (UserIdClaim != null)
                {
                    _logger.LogInformation($"user id is {UserIdClaim.Value}");
                    context.Items["Id"] = UserIdClaim.Value;
                }
            }
            await _next(context);
        }

    }
}
