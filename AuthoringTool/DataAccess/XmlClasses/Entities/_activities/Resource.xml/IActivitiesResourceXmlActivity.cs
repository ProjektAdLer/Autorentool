namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

public interface IActivitiesResourceXmlActivity : IXmlSerializablePath
{
    ActivitiesResourceXmlResource Resource { get; set; }
        
    string Id { get; set; }
        
    string ModuleId { get; set; }
        
    string ModuleName { get; set; }
        
    string ContextId { get; set; }
}