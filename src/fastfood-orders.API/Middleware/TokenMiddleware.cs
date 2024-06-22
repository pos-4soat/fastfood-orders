using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace fastfood_orders.API.Middleware;

[ExcludeFromCodeCoverage]
public class TokenMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var tokenRequest = context.Request.Headers.Authorization.ToString().Replace("Bearer ", "");

        if (!tokenRequest.IsNullOrEmpty())
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(tokenRequest);

                var claims = token.Claims.ToList();
                var identity = new ClaimsIdentity(claims, "jwt");
                var claimsPrincipal = new ClaimsPrincipal(identity);

                var userIdClaim = claimsPrincipal.FindFirst("cognito:username");
                var userId = userIdClaim?.Value;

                var emailClaim = claimsPrincipal.FindFirst("email");
                var email = emailClaim?.Value;

                context.Items["UserId"] = userId;
                context.Items["Email"] = email;
            }
            catch
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
            }
        }

        await next(context);
    }
}
