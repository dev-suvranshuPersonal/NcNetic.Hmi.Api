using NcNetic.Hmi.Api.Interfaces;
using NcNetic.Hmi.Api.Services;
using NcNetic.Hmi.Api.Workers;

namespace NcNetic.Hmi.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSingleton<TimeLoggerService>();
            builder.Host.UseWindowsService();
            builder.Services.AddScoped<ITimeLoggerService, TimeLoggerService>();
            builder.Services.AddScoped<IMachineInfoService, MachineInfoService>();
            builder.Services.AddScoped<ICloudSyncService, CloudSyncService>();
            builder.Services.AddHostedService<CloudSyncWorker>();



            //✅ Specific IP binding
            //builder.WebHost.UseUrls("http://192.168.1.200:7033");

            //// ✅ Local-only binding
            builder.WebHost.UseUrls("http://localhost:7033");

            var app = builder.Build();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
