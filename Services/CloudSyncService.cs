using MySqlConnector;
using SilHmiApi.Interfaces;
using SilHmiApi.Models;
using System.Security.Cryptography.X509Certificates;

public class CloudSyncService : ICloudSyncService
{
    private readonly string _connectionString;

    public CloudSyncService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("CloudMySql")!;
    }

    public async Task SyncDailySummaryAsync(IEnumerable<MachineSummaryDto> summaries)
    {
        if (string.IsNullOrWhiteSpace(_connectionString))
            throw new Exception("CloudMySql connection string is NULL");

        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        foreach (var item in summaries)
        {
            using var cmd = new MySqlCommand(@"
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
    public async Task SyncTimeSnapshotsAsync(IEnumerable<MachineTimeSnapshots> snapshots)
    {
        if (string.IsNullOrWhiteSpace(_connectionString))
            throw new Exception("CloudMySql connection string is NULL");
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        foreach (var item in snapshots)
        {
            using var cmd = new MySqlCommand(@"
            INSERT INTO machine_time_snapshots
            (serial_no, snapshot_time, total_on_seconds, total_laser_on_seconds, actual_process_seconds, total_error_seconds)
            VALUES
            (@serial, @time, @on, @laser, @process, @error)
            ON DUPLICATE KEY UPDATE
                total_on_seconds = VALUES(total_on_seconds),
                total_laser_on_seconds = VALUES(total_laser_on_seconds),
                actual_process_seconds = VALUES(actual_process_seconds),
                total_error_seconds = VALUES(total_error_seconds);
        ", connection);
            cmd.Parameters.AddWithValue("@serial", item.SerialNo);
            cmd.Parameters.AddWithValue("@time", item.snapshot_time);
            cmd.Parameters.AddWithValue("@on", item.total_on_seconds);
            cmd.Parameters.AddWithValue("@laser", item.total_laser_on_seconds);
            cmd.Parameters.AddWithValue("@process", item.actual_process_seconds);
            cmd.Parameters.AddWithValue("@error", item.total_error_seconds);
            await cmd.ExecuteNonQueryAsync();
        }
        
    }

    public async Task<DateTime> GetLastSnapshotTimeFromCloudAsync(string serialNo)
    {
        if (string.IsNullOrWhiteSpace(_connectionString))
            throw new Exception("CloudMySql connection string is NULL");
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        using var cmd = new MySqlCommand(@"
            SELECT 
                IFNULL(MAX(snapshot_time), '2000-01-01 00:00:00') 
            FROM 
                machine_time_snapshots 
            WHERE 
                serial_no = @serial;
        ", connection);
        cmd.Parameters.AddWithValue("@serial", serialNo);
        var result = await cmd.ExecuteScalarAsync();
        return Convert.ToDateTime(result);
    }
}
