namespace Generator.XmlClasses.Entities._activities.Adleradaptivity.xml;

public interface IActivitiesAdleradaptivityXmlActivity : IXmlSerializablePath
{
    ActivitiesAdleradaptivityXmlActivityAdleradaptivity Adleradaptivity { get; set; }
    string Id { get; set; }
    string ModuleId { get; set; }
    string ModuleName { get; set; }
    string ContextId { get; set; }
}