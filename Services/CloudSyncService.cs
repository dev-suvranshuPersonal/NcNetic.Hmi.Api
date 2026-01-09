using MySql.Data.MySqlClient;
using NcNetic.Hmi.Api.Interfaces;
using NcNetic.Hmi.Api.Models;

public class CloudSyncService : ICloudSyncService
{
    private readonly string _connectionString;

    public CloudSyncService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("CloudMySql")!;
    }

    public async Task SyncDailySummaryAsync(IEnumerable<MachineSummaryDto> summaries)
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        foreach (var item in summaries)
        {
            var cmd = new MySqlCommand(@"
                INSERT INTO machine_daily_summary
                (serial_no, summary_date, on_seconds, laser_on_seconds, cutting_seconds, error_seconds, utilization_pct)
                VALUES
                (@serial, @date, @on, @laser, @cut, @error, @util)
                ON DUPLICATE KEY UPDATE
                    on_seconds = VALUES(on_seconds),
                    laser_on_seconds = VALUES(laser_on_seconds),
                    cutting_seconds = VALUES(cutting_seconds),
                    error_seconds = VALUES(error_seconds),
                    utilization_pct = VALUES(utilization_pct);
            ", connection);

            cmd.Parameters.AddWithValue("@serial", item.SerialNo);
            cmd.Parameters.AddWithValue("@date", item.SummaryDate.ToDateTime(TimeOnly.MinValue));
            cmd.Parameters.AddWithValue("@on", item.OnSeconds);
            cmd.Parameters.AddWithValue("@laser", item.LaserOnSeconds);
            cmd.Parameters.AddWithValue("@cut", item.CuttingSeconds);
            cmd.Parameters.AddWithValue("@error", item.ErrorSeconds);
            cmd.Parameters.AddWithValue("@util", item.UtilizationPct);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
