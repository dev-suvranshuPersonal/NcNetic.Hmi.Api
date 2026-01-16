namespace SilHmiApi.Models
{
    public class MachineTimeSnapshots
    {
        public string SerialNo { get; set; } = string.Empty;
        public DateTime snapshot_time { get; set; }
        public double total_on_seconds { get; set; }
        public double total_laser_on_seconds { get; set; }
        public double actual_process_seconds { get; set; }
        public double total_error_seconds { get; set; }

    }
}
