using AuthoringTool.DataAccess.XmlClasses;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities;

[TestFixture]
public class FilesXmlUt
{
    //There aren´t any Parameters to set yet.
    //Will be needed later, then the Test gets changed. 
    [Test]
    public void FilesXmlFiles_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var filesFiles = new FilesXmlFiles();
        
        //Act
        filesFiles.SetParameters();
        
        //Assert
        Assert.That(filesFiles, Is.EqualTo(filesFiles));
    }
    
    
}