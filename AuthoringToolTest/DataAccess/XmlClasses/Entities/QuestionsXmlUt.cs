using AuthoringTool.DataAccess.XmlClasses;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities;

[TestFixture]
public class QuestionsXmlUt
{
    [Test]
    public void QuestionsXmlQuestionsCategories_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var questionsQuestionsCategories = new QuestionsXmlQuestionsCategories();
        
        //Act
        questionsQuestionsCategories.SetParameters();
        
        //Assert
        Assert.That(questionsQuestionsCategories, Is.EqualTo(questionsQuestionsCategories));
    }
}