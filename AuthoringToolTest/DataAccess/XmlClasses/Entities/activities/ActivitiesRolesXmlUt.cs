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
        var currWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        mockFileSystem.AddDirectory(Path.Join(currWorkDir, "XMLFilesForExport","activities", "h5pactivity_2"));
        
        var roles = new ActivitiesRolesXmlRoles();
        roles.SetParameterts("","");
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        
        //Act 
        roles.Serialize("h5pactivity", "2");
        
        //Assert
        var path = Path.Join(currWorkDir, "XMLFilesForExport","activities", "h5pactivity_2", "roles.xml");
        Assert.That(mockFileSystem.FileExists(path), Is.True);
    }
}