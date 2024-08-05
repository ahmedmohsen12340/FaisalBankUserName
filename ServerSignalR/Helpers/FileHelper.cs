using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace ServerSignalR.Helpers
{
    public static class FileHelper
    {
        public static async Task<bool> WriteJsonToFileAsync(string filePath, JObject data, int maxRetries = 3, int delayMilliseconds = 1000)
        {
            bool success = false;
            int retries = maxRetries;

            while (retries > 0 && !success)
            {
                try
                {
                    // Serialize the JObject to JSON
                    string json = data.ToString(Newtonsoft.Json.Formatting.Indented);

                    // Write the JSON to the file asynchronously
                    await File.WriteAllTextAsync(filePath, json);

                    success = true; // Indicate success
                }
                catch (IOException ioEx)
                {
                    // Log the exception if needed
                    Console.WriteLine($"IOException occurred: {ioEx.Message}");

                    // Decrement retries and wait before retrying
                    retries--;
                    await Task.Delay(delayMilliseconds); // Wait before retrying
                }
            }

            return success;
        }
    }
}