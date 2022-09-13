using Generator.DSL;
using Generator.XmlClasses.XmlFileFactories;

namespace Generator.XmlClasses;

public interface IXmlEntityManager
{
    void GetFactories(IReadDsl readDsl, IXmlResourceFactory? xmlFileFactory = null,
        IXmlH5PFactory? xmlH5PFactory = null, IXmlCourseFactory? xmlCourseFactory = null,
        IXmlBackupFactory? xmlBackupFactory = null, XmlSectionFactory? xmlSectionFactory=null, XmlLabelFactory? xmlLabelFactory=null);

}