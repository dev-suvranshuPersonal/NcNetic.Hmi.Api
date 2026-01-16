namespace SilHmiApi.Interfaces
{
    public interface IMachineInfoService
    {
        Task<string> GetMachineSerialNoAsync();
    }
}
