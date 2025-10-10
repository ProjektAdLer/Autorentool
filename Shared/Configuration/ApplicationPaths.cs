namespace Shared.Configuration;


/// <summary>
/// Documentation: https://icons.projekt-adler.eu/Documentation/persisitierung-im-autorentool.html
/// </summary>
public static class ApplicationPaths
{
    /// <summary>
    /// example path: C:\Users\%USERPROFILE%\AppData\Roaming\AdLerAuthoring
    /// </summary>
    public static string RootAppDataFolder =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AdLerAuthoring");

    public static string RootTempFolder =>
        Path.Combine(Path.GetTempPath(), "AdLerAuthoring");

    /// <summary>
    /// example path: C:\Users\%USERPROFILE%\AppData\Roaming\AdLerAuthoring\ContentFiles
    /// </summary>
    public static string ContentFolder => Path.Combine(RootAppDataFolder, "ContentFiles");
    
    /// <summary>
    /// example path: C:\Users\%USERPROFILE%\AppData\Roaming\AdLerAuthoring\SavedWorlds
    /// </summary>
    public static string SavedWorldsFolder => Path.Combine(RootAppDataFolder, "SavedWorlds");
    
    /// <summary>
    /// example path: C:\Users\%USERPROFILE%\AppData\Roaming\AdLerAuthoring\Logs
    /// </summary>
    public static string LogsFolder => Path.Combine(RootAppDataFolder, "Logs");
    
    public static string TempFolder => RootTempFolder;
    
    public static string BackupFolder => Path.Combine(RootTempFolder, "Backup");
}