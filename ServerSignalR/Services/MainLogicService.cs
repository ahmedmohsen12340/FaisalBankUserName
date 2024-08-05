using ServerSignalR.ServicesContracts;

namespace ServerSignalR.Services
{
    public class MainLogicService : IMainLogicService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<MainLogicService> _logger;
        private readonly IReadXmlFileService _readXmlFileService;
        private readonly IMonitorChangedItemsService _monitorChangedItemsService;
        private readonly ISendingToClientService _sendingToClientService;

        public MainLogicService(IConfiguration config,
            ILogger<MainLogicService> logger,
            IReadXmlFileService readXmlFileService,
            IMonitorChangedItemsService monitorChangedItemsService,
            ISendingToClientService sendingToClientService)
        {
            _sendingToClientService = sendingToClientService;
            _monitorChangedItemsService = monitorChangedItemsService;
            _readXmlFileService = readXmlFileService;
            _logger = logger;
            _config = config;
        }

        public async Task Process()
        {
            _logger.LogInformation(DateTime.Now + "File Is Changed");
            var allScreenInfo = await _readXmlFileService.ConvertToListAsync(_config["xmlpath"]);
            var changedScreens = await _monitorChangedItemsService.GetChangedScreenInfo(allScreenInfo);
            _sendingToClientService.Send(changedScreens);
        }
    }
}