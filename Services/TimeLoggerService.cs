using System.Data.OleDb;
using NcNetic.Hmi.Api.Models;

namespace NcNetic.Hmi.Api.Services
{
    public class TimeLoggerService
    {
        private static readonly string ConnStr =
        @"Provider=Microsoft.ACE.OLEDB.12.0;
          Data Source=D:\Projects\HMI_Beta_NcNetic\TimeLogger.mdb;
          Jet OLEDB:Database Password=M1nTime$2025*LcM;
          Persist Security Info=False;";


        public List<MachineSummaryDto> GetDailySummary()
        {
            var list = new List<MachineSummaryDto>();

            using var conn = new OleDbConnection(ConnStr);
            using var cmd = new OleDbCommand(
                @"SELECT SummaryDate,
                         OnSeconds,
                         LaserOnSeconds,
                         CuttingSeconds,
                         ErrorSeconds,
                         UtilizationPct
                  FROM MachineDailySummary
                  ORDER BY SummaryDate DESC", conn);

            conn.Open();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new MachineSummaryDto
                {
                    SummaryDate = DateOnly.FromDateTime(reader.GetDateTime(0)),
                    OnSeconds = reader.IsDBNull(1) ? 0 : reader.GetDouble(1),
                    LaserOnSeconds = reader.IsDBNull(2) ? 0 : reader.GetDouble(2),
                    CuttingSeconds = reader.IsDBNull(3) ? 0 : reader.GetDouble(3),
                    ErrorSeconds = reader.IsDBNull(4) ? 0 : reader.GetDouble(4),
                    UtilizationPct = reader.IsDBNull(5) ? 0 : reader.GetDouble(5),
                });
            }

            return list;
        }
    }
}
