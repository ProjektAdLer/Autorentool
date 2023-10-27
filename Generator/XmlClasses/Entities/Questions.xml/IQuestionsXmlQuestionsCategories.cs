namespace Generator.XmlClasses.Entities.Questions.xml;

public interface IQuestionsXmlQuestionsCategories : IXmlSerializable
{
    List<QuestionsXmlQuestionsCategory> QuestionCategory { get; set; }
}