using Newtonsoft.Json.Linq;
using ServerSignalR.Models;
using ServerSignalR.ServicesContracts;
using Newtonsoft.Json;
using ServerSignalR.Helpers;

namespace ServerSignalR.Services
{
    public class MonitorChangedItemsService : IMonitorChangedItemsService
    {
        private List<ScreenInfo> ScreensChanged;
        private JObject jsonObj;
        private readonly ILogger<MonitorChangedItemsService> _logger;
        public MonitorChangedItemsService(ILogger<MonitorChangedItemsService> logger)
        {
            _logger = logger;
            // Load JSON from the file
            string json = File.ReadAllText(@"Data\LastValues.json");
            jsonObj = JObject.Parse(json);
            ScreensChanged = [];
        }
        public async Task<List<ScreenInfo>> GetChangedScreenInfo(List<ScreenInfo> allScreens)
        {
            ScreensChanged = [];
            foreach (var ScreenInfo in allScreens)
            {
                if (jsonObj[ScreenInfo.WindowName] == null)
                {
                    jsonObj[ScreenInfo.WindowName] = null;
                }
                if (ScreenInfo.SlipNumber.Trim() != jsonObj[ScreenInfo.WindowName].ToString().Trim())
                {
                    ScreensChanged.Add(ScreenInfo);
                    jsonObj[ScreenInfo.WindowName] = ScreenInfo.SlipNumber.Trim();
                }
                else
                {
                    continue;
                }
            }
            // Write back to the file
            // string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            // await File.WriteAllTextAsync(@"Data\LastValues.json", output);


            bool isSuccess = await FileHelper.WriteJsonToFileAsync(@"Data\LastValues.json", jsonObj);
            if (isSuccess)
            {
                _logger.LogInformation("JSON data written successfully.");
            }
            else
            {
                _logger.LogInformation("Failed to write JSON data after multiple attempts.");
            }
            return ScreensChanged;
        }
    }
}