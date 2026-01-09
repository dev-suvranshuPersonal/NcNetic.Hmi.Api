namespace NcNetic.Hmi.Api.Interfaces
{
    public interface IMachineInfoService
    {
        Task<string> GetMachineSerialNoAsync();
    }
}
