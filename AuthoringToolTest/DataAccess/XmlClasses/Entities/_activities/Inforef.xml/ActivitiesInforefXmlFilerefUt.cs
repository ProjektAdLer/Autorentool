using System.Collections.Generic;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.Inforef.xml;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.Activities.Inforef.xml;

[TestFixture]
public class ActivitiesInforefXmlFilerefUt
{
    [Test]
    public void ActivitiesInforefXmlFileref_StandardConstructor_AllParametersSet()
    {
        //Arrange
        var inforefFile = Substitute.For<List<ActivitiesInforefXmlFile>>();
        
        var systemUnderTest = new ActivitiesInforefXmlFileref();
            
        //Act
        systemUnderTest.File = inforefFile;

        //Assert
        Assert.That(systemUnderTest.File, Is.EqualTo(inforefFile));
    }

}