namespace Generator.XmlClasses.Entities._activities.Resource.xml;

public interface IActivitiesResourceXmlActivity : IXmlSerializablePath
{
    ActivitiesResourceXmlResource Resource { get; set; }
        
    string Id { get; set; }
        
    string ModuleId { get; set; }
        
    string ModuleName { get; set; }
        
    string ContextId { get; set; }
}