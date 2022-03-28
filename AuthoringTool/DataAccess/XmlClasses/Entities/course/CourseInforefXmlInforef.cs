using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.course;

[XmlRoot(ElementName="inforef")]
public partial class CourseInforefXmlInforef : IXmlSerializable {

    
    public void SetParameters(CourseInforefXmlRoleref inforefRoleref)
    {
         Roleref = inforefRoleref;
    }

    public void Serialize()
    {
        var xml = new XmlSer();
        xml.Serialize(this, "course/inforef.xml"); 
    }
    
    [XmlElement(ElementName="roleref")]
    public CourseInforefXmlRoleref Roleref;
}