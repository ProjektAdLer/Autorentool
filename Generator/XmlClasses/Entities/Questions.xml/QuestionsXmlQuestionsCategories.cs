using System.Xml.Serialization;
using Generator.WorldExport;

namespace Generator.XmlClasses.Entities.Questions.xml;

[XmlRoot(ElementName="question_categories")]
public class QuestionsXmlQuestionsCategories : IQuestionsXmlQuestionsCategories{
    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, "questions.xml");
    }
    
}