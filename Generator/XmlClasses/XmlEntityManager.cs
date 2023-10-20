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
    private IXmlAdaptivityFactory? _xmlAdaptivityFactory;
    private IXmlBackupFactory? _xmlBackupFactory;
    private IXmlCourseFactory? _xmlCourseFactory;
    private IXmlH5PFactory? _xmlH5PFactory;
    private IXmlLabelFactory? _xmlLabelFactory;
    private IXmlResourceFactory? _xmlResourceFactory;
    private IXmlSectionFactory? _xmlSectionFactory;
    private IXmlUrlFactory? _xmlUrlFactory;

    public XmlEntityManager(IFileSystem? fileSystem = null)
    {
        _fileSystem = fileSystem ?? new FileSystem();
    }

    //run all factories that are available, to set the parameters and create the xml files
    public void GetFactories(IReadDsl readDsl, IXmlResourceFactory? xmlFileFactory = null,
        IXmlH5PFactory? xmlH5PFactory = null, IXmlCourseFactory? xmlCourseFactory = null,
        IXmlBackupFactory? xmlBackupFactory = null,
        IXmlSectionFactory? xmlSectionFactory = null, IXmlLabelFactory? xmlLabelFactory = null,
        IXmlUrlFactory? xmlUrlFactory = null, IXmlAdaptivityFactory? xmlAdaptivityFactory = null)
    {
        var filemanager = new XmlFileManager();
        var contextId = new Random().Next(1000, 9999);

        _xmlSectionFactory = xmlSectionFactory ?? new XmlSectionFactory(readDsl);
        _xmlSectionFactory.CreateSectionFactory();

        _xmlLabelFactory = xmlLabelFactory ?? new XmlLabelFactory(readDsl);
        _xmlLabelFactory.CreateLabelFactory();

        _xmlUrlFactory = xmlUrlFactory ?? new XmlUrlFactory(readDsl);
        _xmlUrlFactory.CreateUrlFactory();

        _xmlAdaptivityFactory = xmlAdaptivityFactory ?? new XmlAdaptivityFactory(readDsl);
        _xmlAdaptivityFactory.CreateXmlAdaptivityFactory();

        _xmlResourceFactory = xmlFileFactory ?? new XmlResourceFactory(readDsl, filemanager, _fileSystem);
        _xmlResourceFactory.CreateResourceFactory();

        _xmlH5PFactory = xmlH5PFactory ?? new XmlH5PFactory(readDsl, filemanager, _fileSystem);
        _xmlH5PFactory.CreateH5PFileFactory();

        _xmlCourseFactory = xmlCourseFactory ?? new XmlCourseFactory(readDsl, contextId);
        _xmlCourseFactory.CreateXmlCourseFactory();

        _xmlBackupFactory = xmlBackupFactory ?? new XmlBackupFactory(readDsl, contextId);
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
    //The reason for this is that the id´s are used in the files.xml file, every Resource gets 2 Blocks and therefore 2 id´s are needed.
    public static void IncreaseFileId()
    {
        FileIdBlock1 = FileIdBlock1 + 2;
        FileIdBlock2 = FileIdBlock2 + 2;
    }
}