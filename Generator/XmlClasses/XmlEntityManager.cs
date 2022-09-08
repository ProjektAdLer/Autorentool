using System.IO.Abstractions;
using Generator.DSL;
using Generator.XmlClasses.XmlFileFactories;

namespace Generator.XmlClasses;

public class XmlEntityManager : IXmlEntityManager
{
    //both ints are required for the xml files, they need the ids of the previous 2 elements.
    public static int FileIdBlock1 = 1;
    public static int FileIdBlock2 = 2;
    private IFileSystem _fileSystem;
    private IXmlResourceFactory? _xmlResourceFactory;
    private IXmlH5PFactory? _xmlH5PFactory;
    private IXmlCourseFactory? _xmlCourseFactory;
    private IXmlBackupFactory? _xmlBackupFactory;
    private XmlSectionFactory? _xmlSectionFactory;

    public XmlEntityManager(IFileSystem? fileSystem=null)
    {
        _fileSystem = fileSystem ?? new FileSystem();
    }

    //run all factories that are available, to set the parameters and create the xml files
    public void GetFactories(IReadDsl readDsl, IXmlResourceFactory? xmlFileFactory=null, 
        IXmlH5PFactory? xmlH5PFactory=null, IXmlCourseFactory? xmlCourseFactory=null, IXmlBackupFactory? xmlBackupFactory=null)
    {
        XmlFileManager filemanager = new XmlFileManager();

        _xmlSectionFactory = new XmlSectionFactory(readDsl); ;
        _xmlSectionFactory.CreateSectionFactory();
        
        _xmlResourceFactory = xmlFileFactory ?? new XmlResourceFactory(readDsl, filemanager, _fileSystem);
        _xmlResourceFactory.CreateFileFactory();

        _xmlH5PFactory = xmlH5PFactory ?? new XmlH5PFactory(readDsl, filemanager, _fileSystem);
        _xmlH5PFactory.CreateH5PFileFactory();
        
        _xmlCourseFactory = xmlCourseFactory ??  new XmlCourseFactory(readDsl);
        _xmlCourseFactory.CreateXmlCourseFactory();
        
        _xmlBackupFactory = xmlBackupFactory ?? new XmlBackupFactory(readDsl);
        _xmlBackupFactory.CreateXmlBackupFactory();
        
    }

    public static int GetFileIdBlock1()
    {
        return FileIdBlock1;
    }

    public static int GetFileIdBlock2()
    {
        return FileIdBlock2;
    }
    
    //The id´s need to be incremental. Block1 and Block2 should not have the same values.
    public static void IncreaseFileId()
    {
        FileIdBlock1 = FileIdBlock1 + 2;
        FileIdBlock2 = FileIdBlock2 + 2;
    }
    
}