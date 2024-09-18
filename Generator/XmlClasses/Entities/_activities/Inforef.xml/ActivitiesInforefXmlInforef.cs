using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using Generator.WorldExport;

namespace Generator.XmlClasses.Entities._activities.Inforef.xml;

[XmlRoot(ElementName = "inforef")]
public class ActivitiesInforefXmlInforef : IActivitiesInforefXmlInforef
{
    public ActivitiesInforefXmlInforef()
    {
        Fileref = new ActivitiesInforefXmlFileref();
    }


    public void Serialize(string? activityName, string? moduleId)
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, Path.Join("activities", activityName + "_" + moduleId, "inforef.xml"));
    }

    [XmlElement(ElementName = "fileref")] public ActivitiesInforefXmlFileref Fileref { get; set; }

    [XmlElement(ElementName = "grade_itemref"), AllowNull]
    public ActivitiesInforefXmlGradeItemref GradeItemref { get; set; }
}