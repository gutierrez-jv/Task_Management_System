using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Task_Management_System.Utils
{
    // Just reused the code from the previous project
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyClassAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var configuration = context.HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;

            if (!context.HttpContext.Request.Headers.TryGetValue("X-API-KEY", out var extractedApiKey))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 401,
                    Content = "API Key was not provided."
                };
                return;
            }

            if (configuration == null || string.IsNullOrEmpty(configuration["Security:ApiKey"]))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 500,
                    Content = "Configuration error."
                };
                return;
            }

            var apiKey = configuration["Security:ApiKey"];

            if (!string.Equals(apiKey, extractedApiKey, StringComparison.Ordinal))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Unauthorized client."
                };
                return;
            }
        }
    }
}
