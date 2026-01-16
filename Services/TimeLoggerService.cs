using System.Data.OleDb;
using SilHmiApi.Interfaces;
using SilHmiApi.Models;

namespace SilHmiApi.Services
{
    public class TimeLoggerService : ITimeLoggerService
    {
        private readonly string _connectionString;
        private readonly IMachineInfoService _machineInfoService;

        public TimeLoggerService(IConfiguration configuration, IMachineInfoService machineInfoService)
        {
            _connectionString = configuration.GetConnectionString("TimeLoggerDb")
                ?? throw new InvalidOperationException("TimeLoggerDb connection string not found.");
            _machineInfoService=machineInfoService;
        }

        public async Task<IReadOnlyList<MachineSummaryDto>> GetDailySummaryAsync()
        {
            var result = new List<MachineSummaryDto>();
            var serialNo = await _machineInfoService.GetMachineSerialNoAsync();

            const string query = @"
                SELECT 
                    SummaryDate,
                    OnSeconds,
                    LaserOnSeconds,
                    CuttingSeconds,
                    ErrorSeconds,
                    UtilizationPct
                FROM MachineDailySummary
                ORDER BY SummaryDate DESC";

            try
            {
                using var connection = new OleDbConnection(_connectionString);
                using var command = new OleDbCommand(query, connection);

                await connection.OpenAsync();

                using var reader = await command.ExecuteReaderAsync();

                if (reader == null)
                    return result;

                while (await reader.ReadAsync())
                {
                    result.Add(new MachineSummaryDto
                    {
                        SerialNo = serialNo,
                        SummaryDate = DateOnly.FromDateTime(reader.GetDateTime(0)),
                        OnSeconds = reader.IsDBNull(1) ? 0 : Convert.ToDouble(reader[1]),
                        LaserOnSeconds = reader.IsDBNull(2) ? 0 : Convert.ToDouble(reader[2]),
                        CuttingSeconds = reader.IsDBNull(3) ? 0 : Convert.ToDouble(reader[3]),
                        ErrorSeconds = reader.IsDBNull(4) ? 0 : Convert.ToDouble(reader[4]),
                        UtilizationPct = reader.IsDBNull(5) ? 0 : Convert.ToDouble(reader[5])
                    });
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to load machine daily summary.", ex);
            }

            return result;
        }
        public async Task<IReadOnlyList<MachineTimeSnapshots>> GetMachineTimeSnapshotsAsync(DateTime lastSnapshotTime)
        {
            var result = new List<MachineTimeSnapshots>();
            var serialNo = await _machineInfoService.GetMachineSerialNoAsync();
            const string query = @"
                SELECT 
                    SnapshotTime,
                    TotalOnSeconds,
                    TotalLaserOnSeconds,
                    ActualProcessSeconds,
                    TotalErrorSeconds
                FROM MachineTimeSnapshots
                WHERE SnapshotTime >= ?
                ORDER BY SnapshotTime ASC";
            try
            {
                using var connection = new OleDbConnection(_connectionString);
                using var command = new OleDbCommand(query, connection);
                command.Parameters.AddWithValue("?", lastSnapshotTime);
                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();
                if (reader == null)
                    return result;
                while (await reader.ReadAsync())
                {
                    result.Add(new MachineTimeSnapshots
                    {
                        SerialNo = serialNo,
                        snapshot_time = reader.GetDateTime(0),
                        total_on_seconds = reader.IsDBNull(1) ? 0 : Convert.ToDouble(reader[1]),
                        total_laser_on_seconds = reader.IsDBNull(2) ? 0 : Convert.ToDouble(reader[2]),
                        actual_process_seconds = reader.IsDBNull(3) ? 0 : Convert.ToDouble(reader[3]),
                        total_error_seconds = reader.IsDBNull(4) ? 0 : Convert.ToDouble(reader[4])
                    });
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to load machine time snapshots.", ex);
            }
            return result;
        }
    }
}
