using System.IO.Abstractions;

namespace AuthoringTool.DataAccess.XmlClasses;

public class XmlEntityManager
{

    public void GetFactories()
    {
        new XmlCourseFactory();
        new XmlSectionFactory();
        var xmlBackupFactory = new XmlBackupFactory();
        xmlBackupFactory.CreateXmlBackupFactory();
        
    }
}