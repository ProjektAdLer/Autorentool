namespace Generator.XmlClasses.Entities._activities.H5PActivity.xml;

public interface IActivitiesH5PActivityXmlActivity : IXmlSerializablePath
{
    ActivitiesH5PActivityXmlH5PActivity H5pactivity { get; set; }
    
    string Id { get; set; }
    
    string ModuleId { get; set; }
    
    string ModuleName { get; set; }
    
    string ContextId { get; set; }
}