using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.Questions.xml;

[XmlRoot(ElementName="question_categories")]
public class QuestionsXmlQuestionsCategories : IQuestionsXmlQuestionsCategories{
    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "questions.xml");
    }
    
}