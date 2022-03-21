using System.Xml.Serialization;

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
            return question;
        }
    }
}