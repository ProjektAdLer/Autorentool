using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using H5pPlayer.BusinessLogic.Api.CleanupH5pPlayer;
using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;

namespace H5pPlayer.BusinessLogic.BusinessRules;

public class TemporaryH5psInWwwrootManager : ICleanupH5pPlayerPort
{
    private IFileSystemDataAccess FileSystemDataAccess { get; }

    internal TemporaryH5psInWwwrootManager(IFileSystemDataAccess fileSystemDataAccess)
    {
        FileSystemDataAccess = fileSystemDataAccess;
    }

    public void CleanDirectoryForTemporaryH5psInWwwroot()
    {
        if (FileSystemDataAccess.DirectoryExists(BuildTemporaryDirectoryFullNameForAllH5ps()))
        {
            FileSystemDataAccess.DeleteAllFilesAndDirectoriesIn(BuildTemporaryDirectoryFullNameForAllH5ps());
        }
    }

    public bool EnsureWritePermissions()
    {
        if (IsTestEnvironment())
        {
            return true;
        }
        try
        {
            var h5pFolder = BuildTemporaryDirectoryFullNameForAllH5ps();

            // Ensure the directory exists
            if (!Directory.Exists(h5pFolder))
            {
                Directory.CreateDirectory(h5pFolder);
            }

            // Get the current user
            string userName = Environment.UserName;

            // Build the icacls command
            string command = $@"/C icacls ""{h5pFolder}"" /grant ""{userName}"":(OI)(CI)F /T";

            // Start the process
            var processInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = command,
                Verb = "runas", // Run as administrator
                UseShellExecute = true
            };

            using (var process = Process.Start(processInfo))
            {
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    Console.WriteLine("Berechtigungen wurden erfolgreich erteilt.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Fehler beim Erteilen der Berechtigungen.");
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler: {ex.Message}");
        }

        return false;
    }
    
    private static bool IsTestEnvironment()
    {
        // Check if we're running in a test environment
        var testAssemblies = new[] { "TEST", "XUNIT", "NUNIT", "MBUNIT", "TESTRUNNER" };
        var entryAssembly = Assembly.GetEntryAssembly();
        var executingAssembly = Assembly.GetExecutingAssembly();
    
        return 
            AppDomain.CurrentDomain.GetAssemblies()
                .Any(a => testAssemblies.Any(name => a.FullName?.ToUpper().Contains(name) == true)) ||
            entryAssembly == null || 
            entryAssembly.FullName?.ToUpper().Contains("TEST") == true ||
            executingAssembly.FullName?.ToUpper().Contains("TEST") == true;
    }


    /// <summary>
    /// to reach the wwwroot-directory we need a path like that:
    /// C:\Users\%USERPROFILE%\Documents\GitHub\Autorentool\AuthoringTool\
    ///     We get this from: <see cref="Environment.CurrentDirectory"/>
    /// And this:
    ///    wwwroot\H5pStandalone\h5p-folder
    /// which is currently hard coded.
    ///
    /// To get a Path like this
    /// C:\Users\%USERPROFILE%\Documents\GitHub\Autorentool\AuthoringTool\wwwroot\H5pStandalone\h5p-folder\h5pFileNameWithoutExtension
    /// </summary>
    public string BuildTemporaryDirectoryFullNameForOneH5p(string h5pFileNameWithoutExtension)
    {
        return Path.Combine(BuildTemporaryDirectoryFullNameForAllH5ps(), h5pFileNameWithoutExtension);
    }


    private string BuildTemporaryDirectoryFullNameForAllH5ps()
    {
        var paths = new[]
        {
            Environment.CurrentDirectory,
            "wwwroot",
            "H5pStandalone",
            "h5p-folder",
        };
        var temporaryDirectoryFullName = Path.Combine(paths);
        return temporaryDirectoryFullName;
    }
}