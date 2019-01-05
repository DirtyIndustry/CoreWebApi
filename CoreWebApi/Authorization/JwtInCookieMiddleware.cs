using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Authorization
{
    public class JwtInCookieMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtInCookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string name = "Authorization";
            string cookie = context.Request.Cookies[name];
            if (cookie != null)
            {
                if (!context.Request.Headers.ContainsKey(name))
                {
                    context.Request.Headers.Append(name, "Bearer " + cookie);
                }
            }
            await _next.Invoke(context);
        }
    }
}
