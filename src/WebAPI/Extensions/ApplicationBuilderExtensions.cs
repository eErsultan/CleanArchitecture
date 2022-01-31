using WebAPI.Middleware;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHttpException(this IApplicationBuilder app)
        {
            return app.UseMiddleware<HttpExceptionMiddleware>();
        }

        public static IApplicationBuilder UseHttpLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<HttpLoggingMiddleware>();
        }
    }
}
