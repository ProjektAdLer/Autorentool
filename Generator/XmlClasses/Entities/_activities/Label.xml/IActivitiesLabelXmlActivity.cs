namespace Generator.XmlClasses.Entities._activities.Label.xml;

public interface IActivitiesLabelXmlActivity : IXmlSerializablePath
{
    ActivitiesLabelXmlLabel Label { get; set; }
    
    string Id { get; set; }

    string ModuleId { get; set; }

    string ModuleName { get; set; }

    string ContextId { get; set; }
}