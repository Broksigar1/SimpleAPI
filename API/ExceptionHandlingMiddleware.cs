using System.Net;

namespace API
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private const string Content = "text/plain";

        async Task IMiddleware.InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ArgumentException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = Content;
                await context.Response.WriteAsync(ex.Message);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = Content;
                await context.Response.WriteAsync(ex.Message);
            }
        }
    }
}
