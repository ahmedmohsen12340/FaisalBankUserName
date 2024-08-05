using ServerSignalR.Models;

namespace ServerSignalR.ServicesContracts
{
    public interface IReadXmlFileService
    {
        Task<List<ScreenInfo>> ConvertToListAsync(string xmlFilePath);
    }
}