using System.Runtime.CompilerServices;

namespace IronDomeApi.Middlewares.Global
{
    public class GlobalLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        
        public GlobalLoggingMiddleware(RequestDelegate next)
        {
            this._next = next;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            Console.WriteLine($"Got Request to server: method = {request.Method}, path =  {request.Path}." +
                $"\nFrom IP => {request.HttpContext.Connection.RemoteIpAddress}");
            
            await this._next(context);
        }
    }
        


}
