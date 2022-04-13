using System.IO.Abstractions;

namespace AuthoringTool.DataAccess.XmlClasses;

public class XmlEntityManager
{

    public void GetFactories()
    {
        var xmlCourseFactory = new XmlCourseFactory();
        xmlCourseFactory.CreateXmlCourseFactory();
        
        var xmlSectionFactory = new XmlSectionFactory();
        xmlSectionFactory.CreateXmlSectionFactory();
        
        var xmlBackupFactory = new XmlBackupFactory();
        xmlBackupFactory.CreateXmlBackupFactory();
        
    }
}