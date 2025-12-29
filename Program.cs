using NcNetic.Hmi.Api.Services;

namespace NcNetic.Hmi.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
           // builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<TimeLoggerService>();
            builder.Host.UseWindowsService();

            // ✅ Specific IP binding
            //builder.WebHost.UseUrls("http://192.168.1.200:7033");

            // ✅ Local-only binding
            builder.WebHost.UseUrls("http://localhost:7033");

            var app = builder.Build();

            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
