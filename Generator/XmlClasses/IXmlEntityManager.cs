using Generator.ATF;
using Generator.XmlClasses.XmlFileFactories;

namespace Generator.XmlClasses;

public interface IXmlEntityManager
{
    void GetFactories(IReadAtf readAtf, IXmlResourceFactory? xmlFileFactory = null,
        IXmlH5PFactory? xmlH5PFactory = null, IXmlCourseFactory? xmlCourseFactory = null,
        IXmlBackupFactory? xmlBackupFactory = null,
        IXmlSectionFactory? xmlSectionFactory = null, IXmlLabelFactory? xmlLabelFactory = null,
        IXmlUrlFactory? xmlUrlFactory = null, IXmlAdaptivityFactory? xmlAdaptivityFactory = null);
}