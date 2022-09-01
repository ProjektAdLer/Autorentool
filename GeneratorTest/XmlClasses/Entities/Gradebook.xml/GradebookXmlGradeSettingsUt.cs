using Generator.XmlClasses.Entities.Gradebook.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities.Gradebook.xml;

[TestFixture]
public class GradebookXmlGradeSettingsUt
{
    
    [Test]
    public void GradebookXmlGradeSettings_StandardConstructor_AllParametersSet()
    {
        // Arrange
        var gradeSetting = new GradebookXmlGradeSetting();
        
        // Act
        var systemUnderTest = new GradebookXmlGradeSettings();
        systemUnderTest.GradeSetting = gradeSetting;
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.GradeSetting, Is.EqualTo(gradeSetting));

        });
    }
}