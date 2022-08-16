using AuthoringTool.DataAccess.XmlClasses.Entities.Gradebook.xml;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.Gradebook.xml;

[TestFixture]
public class GradebookXmlGradeItemsUt
{
    [Test]
    public void GradebookXmlGradeItems_StandardConstructor_AllParametersSet()
    {
        // Arrange
        var gradeItem = new GradebookXmlGradeItem();
        
        // Act
        var systemUnderTest = new GradebookXmlGradeItems();
        systemUnderTest.GradeItem = gradeItem;
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.GradeItem, Is.EqualTo(gradeItem));
        });
    }
}