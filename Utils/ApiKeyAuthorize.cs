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
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<ApiKeyClassAuthorizeAttribute>>();

            if (!context.HttpContext.Request.Headers.TryGetValue("X-API-KEY", out var extractedApiKey))
            {
                logger.LogWarning("Failed API key attempt. No API key was provided for {Path}.",
                    context.HttpContext.Request.Path);

                context.Result = new ContentResult
                {
                    StatusCode = 401,
                    Content = "API Key was not provided."
                };
                return;
            }

            var apiKeys = configuration?.GetSection("Security:ApiKeys").Get<List<ApiKeySetting>>();

            if (apiKeys == null || apiKeys.Count == 0)
            {
                context.Result = new ContentResult
                {
                    StatusCode = 500,
                    Content = "Configuration error."
                };
                return;
            }

            var matchedApiKey = apiKeys.FirstOrDefault(apiKey =>
                string.Equals(apiKey.Value, extractedApiKey, StringComparison.Ordinal));

            if (matchedApiKey == null)
            {
                logger.LogWarning("Failed API key attempt. Invalid API key was provided for {Path}.",
                    context.HttpContext.Request.Path);

                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Unauthorized client."
                };
                return;
            }

            var expiresAt = matchedApiKey.CreatedAt.AddDays(matchedApiKey.ValidForDays);

            if (expiresAt <= DateTime.UtcNow)
            {
                logger.LogWarning("Failed API key attempt. Expired API key {ApiKeyName} was used for {Path}.",
                    matchedApiKey.Name,
                    context.HttpContext.Request.Path);

                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "API Key is expired."
                };
                return;
            }
        }
    }

    public class ApiKeySetting
    {
        public string Name { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public int ValidForDays { get; set; }
    }
}
