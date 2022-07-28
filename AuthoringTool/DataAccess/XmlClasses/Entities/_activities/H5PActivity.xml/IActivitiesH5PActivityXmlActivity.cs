namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

public interface IActivitiesH5PActivityXmlActivity : IXmlSerializablePath
{
    ActivitiesH5PActivityXmlH5PActivity H5pactivity { get; set; }
    
    string Id { get; set; }
    
    string ModuleId { get; set; }
    
    string ModuleName { get; set; }
    
    string ContextId { get; set; }
}