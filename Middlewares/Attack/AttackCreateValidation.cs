using IronDomeApi.Models;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
using System.Text.Json;


namespace IronDomeApi.Middlewares.Attack
{
    public class CreateAttackValidation
    {
        private readonly RequestDelegate _next;

        public CreateAttackValidation(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext httpContext)
        {
            var request = httpContext.Request;
            string body = GetRequestBodyAsync(request.Body);
            if (!string.IsNullOrEmpty(body))
            {
                var document = JsonDocument.Parse(body);
                //if (!document.RootElement.TryGetProperty("origin"))
                //{

                //}
            }

        }

        private string GetRequestBodyAsync(object body)
        {
            return "";
        }
    }
}

