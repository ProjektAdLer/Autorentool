using System.IO.Abstractions;

namespace AuthoringTool.DataAccess.WorldExport;

//This class makes it possible to test the Method "Serialize" in the class "XmlSerialize.cs"
public static class XmlSerializeFileSystemProvider {
    //static constructor
    
    static XmlSerializeFileSystemProvider() 
    {
        FileSystem = new FileSystem();
    }

    public static IFileSystem FileSystem {get;set;}

}