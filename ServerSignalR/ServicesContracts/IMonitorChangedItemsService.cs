using ServerSignalR.Models;

namespace ServerSignalR.ServicesContracts
{
    public interface IMonitorChangedItemsService
    {
        Task<List<ScreenInfo>> GetChangedScreenInfo(List<ScreenInfo> allScreens);
    }
}