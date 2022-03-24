using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="question_categories")]
public partial class QuestionsXmlQuestionsCategories : IXmlSerializable{

    public QuestionsXmlQuestionsCategories()
    {
        
    }

    public void Serialize()
    {
        var xml = new XmlSer();
        xml.Serialize(this, "questions.xml");
    }
    
}