using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;


[XmlRoot(ElementName="inforef")]
public class ActivitiesInforefXmlInforef : IActivitiesInforefXmlInforef{
    
    public void SetParameters(ActivitiesInforefXmlFileref? fileref, ActivitiesInforefXmlGradeItemref? gradeItemref)
    {
        Fileref = fileref;
        Grade_itemref = gradeItemref;
    }
    
    public void Serialize(string? moduleId)
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, Path.Join("activities", "h5pactivity_"+moduleId, "inforef.xml"));
    }
    
    [XmlElement(ElementName="fileref")]
    public ActivitiesInforefXmlFileref? Fileref { get; set; }
    
    [XmlElement(ElementName="grade_itemref")]
    public ActivitiesInforefXmlGradeItemref? Grade_itemref { get; set; }
}