using Microsoft.Extensions.Primitives;
using Serilog.Context;


namespace WEB_APP.Extensions
{
    public class LogEnrichMiddleware
    {
        private readonly RequestDelegate _next;
        public LogEnrichMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            string sessionId = string.Empty;
            StringValues request_ids = string.Empty;
            if (context.Items.TryGetValue("request_id", out object correlation_id))
            {
                sessionId = Convert.ToString(correlation_id);
            }
            else if (context.Request.Headers.ContainsKey("request_id"))
            {
                if (context.Request.Headers.TryGetValue("request_id", out request_ids))
                {
                    sessionId = request_ids.ToString();
                    if (!context.Items.ContainsKey("request_id"))
                    {
                        context.Items.Add("request_id", Convert.ToString(request_ids) ?? string.Empty);
                    }
                    else
                    {
                        context.Items["request_id"] = Convert.ToString(request_ids);
                    }
                }
            }
            else
            {
                sessionId = Guid.NewGuid().ToString();
                context.Items.Add("request_id", sessionId);
            }
            using (LogContext.PushProperty("request_id", sessionId))
            {
                await _next(context);
            }
        }
    }
}
