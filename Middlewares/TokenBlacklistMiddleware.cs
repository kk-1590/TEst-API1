using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace AdvanceAPI.Middlewares
{
    public class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;

        public TokenBlacklistMiddleware(RequestDelegate next, IMemoryCache cache)
        {
            _next = next;
            _cache = cache;
        }
        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token) && _cache.TryGetValue(token, out _))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Sorry!! Access has been revoked");
                return;
            }
            await _next(context);
        }
    }
}
