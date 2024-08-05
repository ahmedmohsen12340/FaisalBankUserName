using System.Xml.Serialization;
using ServerSignalR.Models;
using ServerSignalR.ServicesContracts;

namespace ServerSignalR.Services
{
    public class ReadXmlFileService : IReadXmlFileService
    {
        public List<ScreenInfo> ScreenInfos = [];
        public readonly IConfiguration _config;
        public ReadXmlFileService(IConfiguration configuration)
        {
            _config = configuration;

        }

        public async Task<List<ScreenInfo>> ConvertToListAsync(string xmlFilePath)
        {
            ScreenInfos = [];
            int retryNumber = int.Parse(_config["RetryNumber"]);
            QueuingInfo queuingInfo = null;

            XmlSerializer serializer = new XmlSerializer(typeof(ScreenInfo));

            using (FileStream fileStream = new FileStream(xmlFilePath, FileMode.Open))
            {
                queuingInfo = await DeserializeXmlAsync<QueuingInfo>(xmlFilePath);
            }

            foreach (var screenInfo in queuingInfo.screenInfoList)
            {
                var item = new ScreenInfo()
                {
                    WindowName = screenInfo.WindowName,
                    WindowNumber = screenInfo.WindowNumber,
                    SlipNumber = screenInfo.SlipNumber,
                    CallTime = screenInfo.CallTime,
                    IsOpened = screenInfo.IsOpened
                };
                ScreenInfos.Add(item);
            }
            return ScreenInfos;
        }

        private static async Task<T> DeserializeXmlAsync<T>(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            int retries = 3;
            while (retries > 0)
            {
                try
                {
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        // Read the file asynchronously
                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            string xmlContent = await reader.ReadToEndAsync();

                            // Deserialize the XML from the content string
                            using (StringReader stringReader = new StringReader(xmlContent))
                            {
                                return (T)serializer.Deserialize(stringReader);
                            }
                        }
                    }

                }
                catch (IOException ioEx)
                {
                    // Log the exception if needed
                    Console.WriteLine($"IOException occurred: {ioEx.Message}");

                    // Decrement retries and wait before retrying
                    retries--;
                    Thread.Sleep(1000); // Wait for 1 second before retrying
                }
                if (retries == 0)
                {
                    Console.WriteLine("Failed to access the file after 3 attempts, wait for 15sec");
                    Thread.Sleep(10000); // Wait for 1 second before retrying
                    retries = 3;
                }
            }
            return (T)serializer.Deserialize(new StringReader(""));
        }
    }
}