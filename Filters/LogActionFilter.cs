using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using MyWebApiApp.Models.DTOs;
using MyWebApiApp.Services.Interfaces;

namespace MyWebApiApp.Filters
{
    // Usage: [LogAction("Product Updated")]
    public class LogActionAttribute : ActionFilterAttribute
    {
        private readonly string _actionName;

        public LogActionAttribute(string actionName)
        {
            _actionName = actionName;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var http = context.HttpContext;
            var logService = http.RequestServices.GetService(typeof(ILogServices)) as ILogServices;

            string? userId = http.Session.GetInt32("UserID")?.ToString();
            string userName = http.Session.GetString("UserName") ?? "Unknown";

            

            if (context.Exception == null)
            {
                // ✅ Success log
                logService?.InsertLog(_actionName, $"{_actionName} by {userName}", userId);
            }
            else
            {
                // ❌ Failure log
                logService?.InsertLog(_actionName, $"{_actionName} failed for {userName}: {context.Exception.Message}", userId);
            }
        }
    }
}
