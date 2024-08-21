using IronDomeApi.Middlewares.Attack;
using IronDomeApi.Middlewares.Global;
namespace IronDomeApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseMiddleware<GlobalLoggingMiddleware>();

            app.UseWhen(
                context =>
                context.Request.Path.StartsWithSegments("/api/attacks"),
                appBuilder =>
                {
                    appBuilder.UseMiddleware<JwtValidationMiddleware>();
                    appBuilder.UseMiddleware<AttackLoggingMiddleware>();


                });


            app.MapControllers();

            app.Run();
        }
    }
}
