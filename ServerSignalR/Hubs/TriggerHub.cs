using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ServerSignalR.Helpers;
using System.Text.Json.Nodes;

namespace ServerSignalR.Hubs
{
    public class TriggerHub : Hub
    {
        private readonly ILogger<TriggerHub> _logger;

        public TriggerHub(ILogger<TriggerHub> logger)
        {
            _logger = logger;
        }
        public async Task Register(string userName)
        {


            // Load JSON from the file
            string json = await File.ReadAllTextAsync(@"Data\UserConnectionIds.json");
            JObject jsonObj = JObject.Parse(json);
            // var name = GeneralMethods.FirstAndLastLetter(jsonObj[userName].ToString());
            // Modify the configuration
            //var connectionDetails = new { ConnectionId = Context.ConnectionId };
            jsonObj[userName] = Context.ConnectionId;


            // Write back to the file
            // string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            // await File.WriteAllTextAsync(@"Data\UserConnectionIds.json", output);

            bool isSuccess = await FileHelper.WriteJsonToFileAsync(@"Data\UserConnectionIds.json", jsonObj);
            if (isSuccess)
            {
                _logger.LogInformation("JSON data written successfully.");
            }
            else
            {
                _logger.LogInformation("Failed to write JSON data after multiple attempts.");
            }
        }
    }
}