using System.IO.Abstractions.TestingHelpers;
using System.Reflection;
using Shared.Configuration;

namespace DataAccessTest.Resources;

public static class ResourceHelper
{
    public static MockFileSystem PrepareFileSystemWithResources()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var fs = new MockFileSystem();
        fs.AddFileFromEmbeddedResource("C:\\zips\\import_test.zip", assembly, "DataAccessTest.Resources.import_test.zip");
        var resources = assembly.GetManifestResourceNames()
            .Except(new[] { "DataAccessTest.Resources.import_test.zip" });
        foreach (var resource in resources)
        {
            fs.AddFileFromEmbeddedResource(Path.Combine("C:", resource.Replace("DataAccessTest.Recources.", "")),
                assembly, resource);
        }
        //fs.AddFilesFromEmbeddedNamespace(ApplicationPaths.ContentFolder, assembly, "DataAccessTest.Resources");
        return fs;
    }
}