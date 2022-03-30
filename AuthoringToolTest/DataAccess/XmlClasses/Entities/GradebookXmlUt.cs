using AuthoringTool.DataAccess.XmlClasses;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities;

[TestFixture]
public class GradebookXmlUt
{

    [Test]
    public void GradebookXmlGradebook_SetParameters_ObjectsAreEqual()
    {
        //Arrange 
        var gradeSetting = new GradebookXmlGradeSetting();
        gradeSetting.SetParameters("minmaxtouse", "1");
        
        var gradeSettings = new GradebookXmlGradeSettings();
        gradeSettings.SetParameters(gradeSetting);
        
        var gradebook = new GradebookXmlGradebook();
        
        //Act (What to test)
        gradebook.SetParameters(gradeSettings);
        
        //Assert
        Assert.That(gradebook.Grade_settings, Is.EqualTo(gradeSettings));
    }

    [Test]
    public void GradebookXmlSettings_SetParameters_ObjectsAreEqual()
    {
        //Arrange 
        var gradeSetting = new GradebookXmlGradeSetting();
        gradeSetting.SetParameters("minmaxtouse", "1");
        
        var gradeSettings = new GradebookXmlGradeSettings();
        
        
        //Act (What to test)
        gradeSettings.SetParameters(gradeSetting);
        
        //Assert
        Assert.That(gradeSettings.Grade_setting, Is.EqualTo(gradeSetting));
    }

    [Test]
    public void GradebookXmlSetting_SetParameters_ObjectsAreEqual()
    {
        //Arrange 
        var gradeSetting = new GradebookXmlGradeSetting();

        //Act (What to test)
        gradeSetting.SetParameters("minmaxtouse", "1");
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(gradeSetting.Name, Is.EqualTo("minmaxtouse"));
            Assert.That(gradeSetting.Value, Is.EqualTo("1"));
        });
    }
}