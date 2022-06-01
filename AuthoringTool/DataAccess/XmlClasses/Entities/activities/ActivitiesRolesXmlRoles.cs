using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

[XmlRoot(ElementName="roles")] 
public class ActivitiesRolesXmlRoles : IActivitiesRolesXmlRoles{
    
    public void SetParameterts(string? roleOverrides, string? roleAssignments)
    {
        Role_overrides = roleOverrides;
        Role_assignments = roleAssignments;
    }
    
    public void Serialize(string? moduleId)
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, Path.Join("activities", "h5pactivity_"+moduleId, "roles.xml"));
    }
    
    [XmlElement(ElementName="role_overrides")]
    public string? Role_overrides { get; set; }
        
    [XmlElement(ElementName="role_assignments")]
    public string? Role_assignments { get; set; }
    }
