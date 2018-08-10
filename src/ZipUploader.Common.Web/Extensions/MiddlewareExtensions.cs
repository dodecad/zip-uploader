using Microsoft.AspNetCore.Builder;
using ZipUploader.Common.Web.Middlewares;

namespace ZipUploader.Common.Web.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseBasicAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }

        public static IApplicationBuilder UseErrorLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorLoggingMiddleware>();
        }
    }
}
