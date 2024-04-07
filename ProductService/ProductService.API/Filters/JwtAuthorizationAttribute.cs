using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

public class JwtAuthorizationAttribute : Attribute, IAuthorizationFilter
{

    public void OnAuthorization(AuthorizationFilterContext context)
    {

        // Extract JWT token from request headers
        string token = context.HttpContext.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(token))
        {
            context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            return;
        }

        // Make request to authentication service to validate token
        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", token);
            var authServiceUrl = "http://localhost:5255" + "/api/TokenValidator";
            var authResponse =  httpClient.GetAsync(authServiceUrl).Result;

            if (!authResponse.IsSuccessStatusCode)
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                return;
            }
        }

    }

}
