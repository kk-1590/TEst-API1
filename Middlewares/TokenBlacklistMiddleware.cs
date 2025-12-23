using Microsoft.Extensions.Caching.Distributed;
using System.Net;

namespace AdvanceAPI.Middlewares
{
    public class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDistributedCache _cache;

        public TokenBlacklistMiddleware(RequestDelegate next, IDistributedCache cache)
        {
            _next = next;
            _cache = cache;
        }
        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token) && await _cache.GetStringAsync(token) != null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Sorry!! Access has been revoked");
                return;
            }
            await _next(context);
        }
    }
}
