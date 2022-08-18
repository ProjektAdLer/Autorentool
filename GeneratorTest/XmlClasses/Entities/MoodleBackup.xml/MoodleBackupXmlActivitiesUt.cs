using System.Collections.Generic;
using AuthoringTool.DataAccess.XmlClasses.Entities.MoodleBackup.xml;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.MoodleBackup.xml;

[TestFixture]
public class MoodleBackupXmlActivitiesUt
{
    
    [Test]
    public void MoodleBackupXmlActivities_StandardConstructor_AllParametersSet()
    {
        //Arrange
        var moodlebackupActivity = new MoodleBackupXmlActivity();
        var systemUnderTest = new MoodleBackupXmlActivities();
        
        //Act
        systemUnderTest.Activity.Add(moodlebackupActivity);
        
        //Assert
        Assert.That(systemUnderTest.Activity, Is.EquivalentTo(new List<MoodleBackupXmlActivity> { moodlebackupActivity }));
    }

}