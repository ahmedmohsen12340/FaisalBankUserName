using Microsoft.AspNetCore.SignalR;
using ServerSignalR.Hubs;
using ServerSignalR.ServicesContracts;

namespace ServerSignalR.Services;
public class FileWatcherService
{
    private string _lastChangePath;
    private readonly string path;
    private readonly FileSystemWatcher _fileWatcher;
    private readonly Timer _debounceTimer;
    private readonly IConfiguration _config;
    private readonly IMainLogicService _mainLogic;

    public FileWatcherService(IConfiguration configuration, IMainLogicService mainLogicService)
    {
        _config = configuration;
        _mainLogic = mainLogicService;
        path = _config["xmlpath"];
        _fileWatcher = new FileSystemWatcher()
        {
            Path = Path.GetDirectoryName(path),
            Filter = Path.GetFileName(path),
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName
        };

        _fileWatcher.Changed += OnChanged;
        _fileWatcher.Created += OnChanged;
        _fileWatcher.Deleted += OnChanged;
        _fileWatcher.Renamed += OnRenamed;

        _debounceTimer = new Timer(DebounceCallback, null, Timeout.Infinite, Timeout.Infinite);

        _fileWatcher.EnableRaisingEvents = true;
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        _lastChangePath = e.FullPath;
        _debounceTimer.Change(500, Timeout.Infinite); // Debounce for 500ms
    }

    private void OnRenamed(object sender, RenamedEventArgs e)
    {
        _lastChangePath = e.FullPath;
        _debounceTimer.Change(500, Timeout.Infinite); // Debounce for 500ms
    }

    private async void DebounceCallback(object state)
    {
        //invoke the main logic i want to do when file changes
        await _mainLogic.Process();
        //await _hubContext.Clients.Client(_config["user1"]).SendAsync("ReceiveMessage", $"File changed: {_lastChangePath}");
    }
}

