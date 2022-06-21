using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities;

[XmlRoot(ElementName="question_categories")]
public partial class QuestionsXmlQuestionsCategories : IQuestionsXmlQuestionsCategories{

    public void SetParameters()
    {
        
    }

    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "questions.xml");
    }
    
}