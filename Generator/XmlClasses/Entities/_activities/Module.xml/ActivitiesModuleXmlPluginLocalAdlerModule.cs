using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.Module.xml;

public class ActivitiesModuleXmlPluginLocalAdlerModule : IActivitiesModuleXmlPluginLocalAdlerModule
{
    public ActivitiesModuleXmlPluginLocalAdlerModule()
    {
        AdlerModule = new ActivitiesModuleXmlAdlerModule();
    }

    [XmlElement(ElementName = "adler_module")]
    public ActivitiesModuleXmlAdlerModule? AdlerModule { get; set; }
}