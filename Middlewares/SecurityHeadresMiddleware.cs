using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HRBackEndApi.Middlewares
{
    public class SecurityHeadersMiddleware(RequestDelegate next)
    {
        public async Task Invoke(HttpContext httpContext)
        {
            var headers = httpContext.Response.Headers;
            headers["X-Content-Type-Options"] = "nosniff";
            headers["X-Frame-Options"] = "DENY";
            headers["Referrer-Policy"] = "no-referrer";
            headers["X-XSS-Protection"] = "1; mode=block";
            headers["Content-Security-Policy"] = "default-src 'self'";
            await next(httpContext);
        }
    }
}
