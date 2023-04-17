using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.Module.xml;

public class ActivitiesModuleXmlPluginLocalAdlerModule : IActivitiesModuleXmlPluginLocalAdlerModule
{
    public ActivitiesModuleXmlPluginLocalAdlerModule()
    {
        AdlerScore = new ActivitiesModuleXmlAdlerScore();
    }

    [XmlElement(ElementName = "adler_score")]
    public ActivitiesModuleXmlAdlerScore? AdlerScore { get; set; }
}