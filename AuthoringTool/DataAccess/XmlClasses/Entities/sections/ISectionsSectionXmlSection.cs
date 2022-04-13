namespace AuthoringTool.DataAccess.XmlClasses.sections;

public interface ISectionsSectionXmlSection : IXmlSerializable
{
    void SetParameters(string id, string number);
}