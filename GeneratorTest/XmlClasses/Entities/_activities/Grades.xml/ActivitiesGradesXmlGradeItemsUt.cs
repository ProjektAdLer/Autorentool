﻿using Generator.XmlClasses.Entities._activities.Grades.xml;
using NSubstitute;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities._activities.Grades.xml;

[TestFixture]
public class ActivitiesGradesXmlGradeItemsUt
{
    [Test]
    public void ActivitiesGradesXmlGradeItems_StandardConstructor_AllParametersSet()
    {
        //Arrange
        var gradeitem = Substitute.For<ActivitiesGradesXmlGradeItem>();
        var systemUnderTest = new ActivitiesGradesXmlGradeItems();
        
        //Act
        systemUnderTest.GradeItem= gradeitem;

        //Assert
        Assert.That(systemUnderTest.GradeItem, Is.EqualTo(gradeitem));

    }
}