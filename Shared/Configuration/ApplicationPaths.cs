namespace Shared.Configuration;

public static class ApplicationPaths
{
    public static string RootFolder =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AdLerAuthoring");

    public static string ContentFolder => Path.Combine(RootFolder, "ContentFiles");
    public static string SavedWorldsFolder => Path.Combine(RootFolder, "SavedWorlds");
    public static string LogsFolder => Path.Combine(RootFolder, "Logs");
}