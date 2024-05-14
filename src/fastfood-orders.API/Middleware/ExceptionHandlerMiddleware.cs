using fastfood_orders.Application.Shared.BaseResponse;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mime;

namespace fastfood_orders.API.Middleware;

public class ExceptionHandlerMiddleware(
    RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.InnerException);
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(
                new ErrorResponse<Error>(new Error("OIE999"))
            ));
        }
    }
}
