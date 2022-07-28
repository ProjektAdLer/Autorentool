using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

[XmlRoot(ElementName="roles")] 
public class ActivitiesRolesXmlRoles : IActivitiesRolesXmlRoles{

    public ActivitiesRolesXmlRoles()
    {
        RoleOverrides = "";
        RoleAssignments = "";
    }
    
    public void Serialize(string? activityName, string? moduleId)
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, Path.Join("activities", activityName + "_" + moduleId, "roles.xml"));
    }
    
    [XmlElement(ElementName="role_overrides")]
    public string RoleOverrides { get; set; }
        
    [XmlElement(ElementName="role_assignments")]
    public string RoleAssignments { get; set; }
    }
