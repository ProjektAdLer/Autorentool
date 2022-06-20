using System.IO.Abstractions;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.XmlClasses.XmlFileFactories;

namespace AuthoringTool.DataAccess.XmlClasses;

public class XmlEntityManager
{
    //both ints are required for the xml files, they need the ids of the previous 2 elements.
    public static int FileIdBlock1 = 1;
    public static int FileIdBlock2 = 2;
    
    //run all factorys that are available, to set the parameters and create the xml files
    public void GetFactories(ReadDSL readDsl, string dslpath)
    {
        XmlFileManager filemanager = new XmlFileManager();

        var xmlFileFactory = new XmlFileFactory(readDsl, dslpath, filemanager);
        xmlFileFactory.CreateFileFactory();
        
        var xmlH5PFileFactory = new XmlH5PFactory(readDsl, filemanager);
        xmlH5PFileFactory.CreateH5PFileFactory();
        
        var xmlCourseFactory = new XmlCourseFactory(readDsl);
        xmlCourseFactory.CreateXmlCourseFactory();

        /*var xmlSectionFactory = new XmlSectionFactory();
        xmlSectionFactory.CreateXmlSectionFactory();*/
        
        var xmlBackupFactory = new XmlBackupFactory(readDsl);
        xmlBackupFactory.CreateXmlBackupFactory();
        
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