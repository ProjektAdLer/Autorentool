namespace AuthoringTool.DataAccess.XmlClasses;

public class XmlEntityManager
{
    public void GetFactories()
    {
        new XmlCourseFactory();
        new XmlSectionFactory();
        var backupFactory = new XmlBackupFactory();
    }
}