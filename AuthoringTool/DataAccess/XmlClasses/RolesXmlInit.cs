using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses;

public class RolesXmlInit : IXMLInit
{
    public void XmlInit()
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

    }
}


