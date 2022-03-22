using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses;

public class QuestionsXmlInit
{ 
    public void XmlInit()
    {
            var question = new QuestionsXmlQuestionsCategories();
            
            var xml = new XmlSer();
            xml.serialize(question, "questions.xml");
    }
}