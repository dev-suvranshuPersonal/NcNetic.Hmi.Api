using NcNetic.Hmi.Api.Interfaces;
using System.Data.OleDb;

namespace NcNetic.Hmi.Api.Services
{
    public class MachineInfoService : IMachineInfoService
    {
        private readonly string _connectionString;
        public MachineInfoService(IConfiguration configuration) 
        {
            _connectionString = configuration.GetConnectionString("MachineInfoDb")
            ?? throw new InvalidOperationException("MachineInfoDb connection string not found.");
        }
        public async Task<string> GetMachineSerialNoAsync()
        {
            const string query = @"SELECT TOP 1 [Serial No] FROM MachineInfo";

            try { 
                using var connection = new OleDbConnection(_connectionString);
                using var command = new OleDbCommand(query, connection);
                await connection.OpenAsync();
                var result = await command.ExecuteScalarAsync();
                return result?.ToString() ?? string.Empty;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to load machine serial number.", ex);
            }
           
        }
    }
}
