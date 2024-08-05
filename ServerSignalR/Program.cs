using ServerSignalR.Hubs;
using ServerSignalR.Services;
using ServerSignalR.ServicesContracts;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(@"Data\UserConnectionIds.json", optional: false, reloadOnChange: true);
builder.Services.AddSignalR();
builder.Services.AddSingleton<FileWatcherService>(); // Register the FileWatcherService
builder.Services.AddSingleton<IMainLogicService, MainLogicService>();
builder.Services.AddSingleton<IReadXmlFileService, ReadXmlFileService>();
builder.Services.AddSingleton<IMonitorChangedItemsService, MonitorChangedItemsService>();
builder.Services.AddSingleton<ISendingToClientService, SendingToClientService>();

var app = builder.Build();

app.UseRouting();
app.MapHub<TriggerHub>("/TriggerHub");



// Start the FileWatcherService
var fileWatcherService = app.Services.GetRequiredService<FileWatcherService>();

app.Run();