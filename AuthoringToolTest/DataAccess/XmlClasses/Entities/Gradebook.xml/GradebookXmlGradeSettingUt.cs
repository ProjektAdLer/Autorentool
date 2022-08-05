﻿using AuthoringTool.DataAccess.XmlClasses.Entities.Gradebook.xml;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.Gradebook.xml;

[TestFixture]
public class GradebookXmlGradeSettingUt
{
    
    [Test]
    public void GradebookXmlGradeSetting_StandardConstructor_AllParametersSet()
    {
        var systemUnderTest = new GradebookXmlGradeSetting();
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Id, Is.EqualTo(""));
            Assert.That(systemUnderTest.Name, Is.EqualTo("minmaxtouse"));
            Assert.That(systemUnderTest.Value, Is.EqualTo("1"));
        });
        
    }

}