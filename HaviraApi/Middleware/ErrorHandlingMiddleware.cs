using System;
namespace HaviraApi.Middleware;

public class ErrorHandlingMiddleware : IMiddleware
{
    public ErrorHandlingMiddleware()
    {
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(e.Message);
        }
    }
}

