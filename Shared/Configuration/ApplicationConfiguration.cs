using System.Collections.Specialized;
using System.IO.Abstractions;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Shared.Configuration;

public class ApplicationConfiguration : IApplicationConfiguration
{
    private readonly string _folderPath;
    private readonly string _filePath;
    internal ILogger<ApplicationConfiguration> Logger { get; }
    internal IFileSystem FileSystem { get; }
    public IObservableDictionary<string, string> Configuration { get; }

    public string this[string key]
    {
        get => Configuration[key];
        set => Configuration[key] = value;
    }

    public ApplicationConfiguration(ILogger<ApplicationConfiguration> logger, IFileSystem fileSystem)
    {
        Logger = logger;
        FileSystem = fileSystem;
        _folderPath = FileSystem.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AdLerAuthoring");
        _filePath = FileSystem.Path.Combine(_folderPath, "ApplicationConfig.json");
        Configuration = TryLoadConfiguration();
        CheckIfAllKeysExist();
        SaveConfiguration();
        Configuration.CollectionChanged += OnCollectionChanged;
    }

    private ObservableDictionary<string, string> TryLoadConfiguration()
    {
        if (!FileSystem.File.Exists(_filePath))
        {
            Logger.LogInformation("no config file found, generating default config");
            return new ObservableDictionary<string, string>();
        }

        var json = FileSystem.File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<ObservableDictionary<string, string>>(json) ??
               throw new InvalidOperationException();
    }

    private ObservableDictionary<string, string> GetDefaultConfig()
    {
        return new ObservableDictionary<string, string>
        {
            {IApplicationConfiguration.BackendBaseUrl, ""},
            {IApplicationConfiguration.BackendUsername, ""},
            {IApplicationConfiguration.BackendToken, ""}
        };
    }

    private void CheckIfAllKeysExist()
    {
        var keys = new[]
        {
            IApplicationConfiguration.BackendBaseUrl,
            IApplicationConfiguration.BackendUsername,
            IApplicationConfiguration.BackendToken
        };
        var defaultConfig = GetDefaultConfig();
        foreach (var key in keys)
        {
            if (Configuration.ContainsKey(key)) continue;
            Logger.LogInformation("config is missing key {Key}, adding it", key);
            if (defaultConfig.TryGetValue(key, out var value))
            {
                Configuration.Add(key, value);
                continue;
            }

            Configuration.Add(key, "");
        }
    }

    private void SaveConfiguration()
    {
        Logger.LogInformation("saving config file to {Filepath}", _filePath);
        var json = JsonSerializer.Serialize(Configuration);
        //ensure directory exists
        FileSystem.Directory.CreateDirectory(_folderPath);
        FileSystem.File.WriteAllText(_filePath, json);
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => SaveConfiguration();
}