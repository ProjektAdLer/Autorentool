using AuthoringTool.DataAccess.XmlClasses.Entities._activities.Grades.xml;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.Activities.Grades.xml;

[TestFixture]
public class ActivitiesGradesXmlGradeItemUt
{
    [Test]
    public void ActivitiesGradesXmlGradeItem_StandardConstructor_AllParametersSet()
    {
        //Arrange
        
        //Act
        var systemUnderTest = new ActivitiesGradesXmlGradeItem();
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.CategoryId, Is.EqualTo(""));
            Assert.That(systemUnderTest.ItemName, Is.EqualTo(""));
            Assert.That(systemUnderTest.ItemType, Is.EqualTo(""));
            Assert.That(systemUnderTest.ItemModule, Is.EqualTo(""));
            Assert.That(systemUnderTest.ItemInstance, Is.EqualTo("1"));
            Assert.That(systemUnderTest.ItemNumber, Is.EqualTo("0"));
            Assert.That(systemUnderTest.ItemInfo, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.IdNumber, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.Calculation, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.GradeType, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Grademax, Is.EqualTo("100.00000"));
            Assert.That(systemUnderTest.Grademin, Is.EqualTo("0.00000"));
            Assert.That(systemUnderTest.ScaleId, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.OutcomeId, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.Gradepass, Is.EqualTo("0.00000"));
            Assert.That(systemUnderTest.Multfactor, Is.EqualTo("1.00000"));
            Assert.That(systemUnderTest.Plusfactor, Is.EqualTo("0.00000"));
            Assert.That(systemUnderTest.Aggregationcoef, Is.EqualTo("0.00000"));
            Assert.That(systemUnderTest.Aggregationcoef2, Is.EqualTo("1.00000"));
            Assert.That(systemUnderTest.Weightoverride, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Sortorder, Is.EqualTo("2"));
            Assert.That(systemUnderTest.Display, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Decimals, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.Hidden, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Locked, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Locktime, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Needsupdate, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Timecreated, Is.EqualTo(""));
            Assert.That(systemUnderTest.Timemodified, Is.EqualTo(""));
            Assert.That(systemUnderTest.GradeGrades, Is.EqualTo(""));
            Assert.That(systemUnderTest.Id, Is.EqualTo(""));
        });
    }
}