using Generator.XmlClasses.Entities._activities.Inforef.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities._activities.Inforef.xml;

[TestFixture]
public class ActivitiesInforefXmlGradeItemrefUt
{
    [Test]
        public void ActivitiesInforefXmlGradeItemref_StandardConstructor_AllParametersSet()
        {
            //Arrange
            var gradeitem = new ActivitiesInforefXmlGradeItem();
            var systemUnderTest = new ActivitiesInforefXmlGradeItemref();
    
            //Act
            systemUnderTest.GradeItem = gradeitem;
            
            //Assert
            Assert.That(systemUnderTest.GradeItem, Is.EqualTo(gradeitem));
    
        }
}