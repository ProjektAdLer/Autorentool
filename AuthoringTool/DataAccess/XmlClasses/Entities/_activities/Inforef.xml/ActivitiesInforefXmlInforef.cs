using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;


[XmlRoot(ElementName="inforef")]
public class ActivitiesInforefXmlInforef : IActivitiesInforefXmlInforef{

    public ActivitiesInforefXmlInforef()
    {
        Fileref = new ActivitiesInforefXmlFileref();
        GradeItemref = new ActivitiesInforefXmlGradeItemref();
    }
    
   
    public void Serialize(string? activityName, string? moduleId)
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, Path.Join("activities", activityName + "_" + moduleId, "inforef.xml"));
    }
    
    [XmlElement(ElementName="fileref")]
    public ActivitiesInforefXmlFileref Fileref { get; set; }
    
    [XmlElement(ElementName="grade_itemref")]
    public ActivitiesInforefXmlGradeItemref GradeItemref { get; set; }
}