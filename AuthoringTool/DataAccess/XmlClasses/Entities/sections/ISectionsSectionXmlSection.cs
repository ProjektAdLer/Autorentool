namespace AuthoringTool.DataAccess.XmlClasses.sections;

public interface ISectionsSectionXmlSection : IXmlSerializablePath
{
    void SetParameters(string? number, string name, string summary, string summaryformat, string sequence,
        string visible, string availabilityjson, string? timemodified, string? id);
}