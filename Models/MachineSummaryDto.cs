namespace SilHmiApi.Models
{
    public class MachineSummaryDto
    {
        public string SerialNo { get; set; } = string.Empty;
        public DateOnly SummaryDate { get; set; }
        public double OnSeconds { get; set; }
        public double LaserOnSeconds { get; set; }
        public double CuttingSeconds { get; set; }
        public double ErrorSeconds { get; set; }
        public double UtilizationPct { get; set; }
    }
}
