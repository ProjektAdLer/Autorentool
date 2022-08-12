using AuthoringTool.DataAccess.XmlClasses.Entities.Gradebook.xml;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.Gradebook.xml;

[TestFixture]
public class GradebookXmlGradeCategoriesUt
{
    [Test]
    public void GradebookXmlGradeCategories_StandardConstructor_AllParametersSet()
    {
        // Arrange
        var gradeCategory = new GradebookXmlGradeCategory();
        
        // Act
        var systemUnderTest = new GradebookXmlGradeCategories();
        systemUnderTest.GradeCategory = gradeCategory;
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.GradeCategory, Is.EqualTo(gradeCategory));
        });
    }

}