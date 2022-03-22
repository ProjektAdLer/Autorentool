using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses
{
    [XmlRoot(ElementName="role")]
    public class RolesXmlRole
    {
        [XmlElement(ElementName = "name")] 
        public string Name=string.Empty;
        
        [XmlElement(ElementName = "shortname")]
        public string Shortname=string.Empty;
        
        [XmlElement(ElementName="nameincourse")]
        public string Nameincourse=string.Empty;

        [XmlElement(ElementName = "description")]
        public string Description=string.Empty;

        [XmlElement(ElementName = "sortorder")]
        public string Sortorder=string.Empty;

        [XmlElement(ElementName = "archetype")]
        public string Archetype=string.Empty;

        [XmlAttribute(AttributeName = "id")] 
        public string Id=string.Empty;
    }

    [XmlRoot(ElementName="roles_definition")]
    public class RolesXmlRolesDefinition {
        [XmlElement(ElementName="role")]
        public RolesXmlRole Role { get; set; }
    }
    
    public class RolesXmlInit
    {
        public RolesXmlRolesDefinition Init()
        {
            //roles.xml
            var role = new RolesXmlRole();
            role.Name="";
            role.Description="";
            role.Id = "5";
            role.Shortname = "student";
            role.Nameincourse = "$@NULL@$";
            role.Sortorder = "5";
            role.Archetype = "student";
            
            var roleDef = new RolesXmlRolesDefinition();
            roleDef.Role=role;

            var xml = new XmlSer();
            xml.serialize(roleDef, "roles.xml");
            
            return roleDef;
        }
    }
    
}

