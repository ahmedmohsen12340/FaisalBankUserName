using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Get the value of an environment variable
            string userName = Environment.GetEnvironmentVariable("userName");
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5255/TriggerHub")
                .Build();

            connection.On<string>("SlipNumber", (message) =>
            {
                Console.WriteLine($"Message from server: {message}");
            });

            try
            {
                await connection.StartAsync();
                Console.WriteLine("Connected to the server.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to the server: {ex.Message}");
                return;
            }
            await connection.InvokeAsync("Register", userName);


            // Check the connection state periodically
            while (true)
            {
                if (connection.State == HubConnectionState.Disconnected)
                {
                    Console.WriteLine("Connection Lost");
                    try
                    {
                        await connection.StartAsync();
                        Console.WriteLine("Connected to the server.");
                        await connection.InvokeAsync("Register", userName);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error connecting to the server: {ex.Message}");
                        continue;
                    }

                }

                await Task.Delay(5000); // Check every 5 seconds
            }
        }
    }
}