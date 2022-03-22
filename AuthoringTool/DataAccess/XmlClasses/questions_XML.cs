using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses

{
    [XmlRoot(ElementName="question_categories")]
    public class QuestionsXmlQuestionsCategories {
        
    }

    public class QuestionsXmlInit
    {
        public QuestionsXmlQuestionsCategories Init()
        {
            var question = new QuestionsXmlQuestionsCategories();
            
            var xml = new XmlSer();
            xml.serialize(question, "questions.xml");
            
            return question;
        }
    }
}