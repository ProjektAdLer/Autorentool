using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="sections")]
public partial class MoodleBackupXmlSections : IMoodleBackupXmlSections{

    
    public void SetParameters(List<MoodleBackupXmlSection>? moodleBackupSection)
    {
        Section = moodleBackupSection;
    }
    
    [XmlElement(ElementName="section")]
    public List<MoodleBackupXmlSection>? Section;
}