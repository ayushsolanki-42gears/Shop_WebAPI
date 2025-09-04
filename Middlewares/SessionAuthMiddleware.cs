namespace MyWebApiApp.Middlewares
{
    public class SessionAuthMiddleware
    {
        private readonly RequestDelegate _next;
        public SessionAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine("Auth Middleware "+context.Request.Path);
            if (context.Request.Path.StartsWithSegments("/api/user/LoginUser"))
            {
                await _next(context);
                return;
            }

            var UserID = context.Session.GetInt32("UserID");

            if (!UserID.HasValue)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("User Not Logged in");
                return;
            }

            await _next(context);
        }
    }
}