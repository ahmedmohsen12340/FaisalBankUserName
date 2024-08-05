using Microsoft.AspNetCore.SignalR;
using ServerSignalR.Hubs;
using ServerSignalR.Models;
using ServerSignalR.ServicesContracts;

namespace ServerSignalR.Services
{
    public class SendingToClientService : ISendingToClientService
    {
        private readonly IHubContext<TriggerHub> _hubContext;
        private readonly IConfiguration _config;
        public SendingToClientService(IHubContext<TriggerHub> hubContext, IConfiguration config)
        {
            _config = config;
            _hubContext = hubContext;
        }
        public void Send(List<ScreenInfo> screenInfos)
        {
            foreach (var ScreenInfo in screenInfos)
            {
                if (_config[ScreenInfo.WindowName] != null)
                {
                    _hubContext.Clients.Client(_config[ScreenInfo.WindowName].Trim()).SendAsync("SlipNumber", ScreenInfo.SlipNumber);
                    Console.WriteLine(ScreenInfo.WindowName + ": " + ScreenInfo.SlipNumber);
                }
            }
        }
    }
}