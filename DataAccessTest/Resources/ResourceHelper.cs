using System.IO.Abstractions.TestingHelpers;
using System.Reflection;

namespace DataAccessTest.Resources;

public static class ResourceHelper
{
    public static MockFileSystem PrepareWindowsFileSystemWithResources()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var fs = new MockFileSystem();
        fs.AddFileFromEmbeddedResource("C:\\zips\\import_test.zip", assembly,
            "DataAccessTest.Resources.import_test.zip");
        var resources = assembly.GetManifestResourceNames()
            .Except(new[] { "DataAccessTest.Resources.import_test.zip" });
        foreach (var resource in resources)
        {
            fs.AddFileFromEmbeddedResource(Path.Combine("C:", resource.Replace("DataAccessTest.Resources.", "")),
                assembly, resource);
        }

        //fs.AddFilesFromEmbeddedNamespace(ApplicationPaths.ContentFolder, assembly, "DataAccessTest.Resources");
        return fs;
    }

    public static MockFileSystem PrepareUnixFileSystemWithResources()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var fs = new MockFileSystem();
        fs.AddFileFromEmbeddedResource("/zips/import_test.zip", assembly, "DataAccessTest.Resources.import_test.zip");
        var resources = assembly.GetManifestResourceNames()
            .Except(new[] { "DataAccessTest.Resources.import_test.zip" });
        foreach (var resource in resources)
        {
            fs.AddFileFromEmbeddedResource(Path.Combine("/", resource.Replace("DataAccessTest.Resources.", "")),
                assembly, resource);
        }

        //fs.AddFilesFromEmbeddedNamespace(ApplicationPaths.ContentFolder, assembly, "DataAccessTest.Resources");
        return fs;
    }
}