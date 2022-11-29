namespace Generator.XmlClasses.Entities._activities.Url.xml;

public interface IActivitiesUrlXmlActivity : IXmlSerializablePath
{
    ActivitiesUrlXmlUrl Url { get; set; }
    string Id { get; set; }
    string Moduleid { get; set; }
    string Modulename { get; set; }
    string Contextid { get; set; }
}