using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses.Entities.activities;
using AuthoringTool.DataAccess.XmlClasses.XmlFileFactories;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities.activities;

[TestFixture]
public class ActivitiesRolesXmlUt
{
    [Test]
    public void ActivitiesRolesXmlRoles_SetParameters_ObjectAreEqual()
    {
        //Arrange
        var roles = new ActivitiesRolesXmlRoles();
        
        //Act
        roles.SetParameterts("","");
        
        //Assert
        Assert.That(roles.Role_assignments, Is.EqualTo(""));
        Assert.That(roles.Role_overrides, Is.EqualTo(""));
    }

    [Test]
    public void ActivitiesRolesXmlRoles_Serialize_XmlFileWritten()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var readDsl = new ReadDSL();
        var h5pfactory = new XmlH5PFactory(readDsl, mockFileSystem, null, null, null, null,
            null, null, null, null, null, null, null, null, null,
            null, null, null, null);
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        var roles = new ActivitiesRolesXmlRoles();
        roles.SetParameterts("","");
        
        //Act 
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        h5pfactory.CreateActivityFolder("2");
        roles.Serialize("2");
        
        //Assert
        var path = Path.Join(currWorkDir, "XMLFilesForExport","activities", "h5pactivity_2", "roles.xml");
        Assert.That(mockFileSystem.FileExists(path), Is.True);
    }
}