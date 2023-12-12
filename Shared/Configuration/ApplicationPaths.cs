namespace Shared.Configuration;

public static class ApplicationPaths
{
    public static string RootAppDataFolder =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AdLerAuthoring");

    public static string RootTempFolder =>
        Path.Combine(Path.GetTempPath(), "AdLerAuthoring");

    public static string ContentFolder => Path.Combine(RootAppDataFolder, "ContentFiles");
    public static string SavedWorldsFolder => Path.Combine(RootAppDataFolder, "SavedWorlds");
    public static string LogsFolder => Path.Combine(RootAppDataFolder, "Logs");
    public static string TempFolder => RootTempFolder;
}