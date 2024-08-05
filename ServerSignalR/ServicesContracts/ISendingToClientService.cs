using ServerSignalR.Models;

namespace ServerSignalR.ServicesContracts
{
    public interface ISendingToClientService
    {
        void Send(List<ScreenInfo> screenInfos);
    }
}