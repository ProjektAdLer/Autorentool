using Generator.XmlClasses.Entities.Gradebook.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities.Gradebook.xml;

[TestFixture]
public class GradebookXmlGradeCategoryUt
{
    [Test]
    public void GradebookXmlGradeCategory_StandardConstructor_AllParametersSet()
    {
        // Arrange
        
        // Act
        var systemUnderTest = new GradebookXmlGradeCategory();
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Parent, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.Depth, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Path, Is.EqualTo("/1/"));
            Assert.That(systemUnderTest.Fullname, Is.EqualTo("?"));
            Assert.That(systemUnderTest.Aggregation, Is.EqualTo("13"));
            Assert.That(systemUnderTest.Keephigh, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Droplow, Is.EqualTo("0"));
            Assert.That(systemUnderTest.AggregateOnlyGraded, Is.EqualTo("1"));
            Assert.That(systemUnderTest.AggregateOutcomes, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Timecreated, Is.EqualTo(""));
            Assert.That(systemUnderTest.Timemodified, Is.EqualTo(""));
            Assert.That(systemUnderTest.Hidden, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Id, Is.EqualTo("1"));
        });
    }
}