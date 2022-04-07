using System.IO.Abstractions;

namespace AuthoringTool.DataAccess.WorldExport;

public static class XmlSerializeFileSystemProvider {
    //static constructor
    
    static XmlSerializeFileSystemProvider() 
    {
        FileSystem = new FileSystem();
    }

    public static IFileSystem FileSystem {get;set;}

}