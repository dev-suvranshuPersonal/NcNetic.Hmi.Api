using SilHmiApi.Interfaces;
using SilHmiApi.Services;
using SilHmiApi.Workers;

namespace NcNetic.Hmi.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                Args = args,
                ContentRootPath = AppContext.BaseDirectory
            });


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSingleton<TimeLoggerService>();
            builder.Host.UseWindowsService();
            builder.Services.AddScoped<ITimeLoggerService, TimeLoggerService>();
            builder.Services.AddScoped<IMachineInfoService, MachineInfoService>();
            builder.Services.AddScoped<ICloudSyncService, CloudSyncService>();
            builder.Services.AddHostedService<CloudSyncWorker>();

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();



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
