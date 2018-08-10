using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Threading.Tasks;
using ZipUploader.Common.Core;

namespace ZipUploader.Common.Web.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly IConfiguration _configuration;
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(IConfiguration configuration, RequestDelegate next)
        {
            _configuration = configuration;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            const string usernameKey = "Users:0:Login";
            const string passwordKey = "Users:0:Password";

            var authHeader = context.Request.Headers[ApplicationConstants.AuthHeader].ToString();

            if (authHeader != null && authHeader.StartsWith(ApplicationConstants.BasicAuthScheme))
            {
                var encodedUsernamePassword = authHeader.Substring($"{ApplicationConstants.BasicAuthScheme} ".Length).Trim();
                var encoding = Encoding.GetEncoding(ApplicationConstants.IsoEncoding);
                var usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

                var seperatorIndex = usernamePassword.IndexOf(':');
                var username = usernamePassword.Substring(0, seperatorIndex);
                var password = usernamePassword.Substring(seperatorIndex + 1);

                if (username == _configuration.GetValue<string>(usernameKey)
                    && password == _configuration.GetValue<string>(passwordKey))
                {
                    await _next.Invoke(context);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }
    }
}
