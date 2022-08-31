using Generator.XmlClasses.Entities.Files.xml;
using NUnit.Framework;

namespace GeneratorTest.XmlClasses.Entities.Files.xml;

[TestFixture]
public class FilesXmlFileUt
{
    [Test]
    public void FilesXmlFile_SetParameters_ObjectsAreEqual()
    {
        //Arrange

        //Act
        var filesFile = new FilesXmlFile();
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(filesFile.Id, Is.EqualTo(""));
            Assert.That(filesFile.ContentHash, Is.EqualTo(""));
            Assert.That(filesFile.ContextId, Is.EqualTo(""));
            Assert.That(filesFile.Component, Is.EqualTo("mod_resource"));
            Assert.That(filesFile.FileArea, Is.EqualTo("content"));
            Assert.That(filesFile.ItemId, Is.EqualTo("0"));
            Assert.That(filesFile.Filename, Is.EqualTo(""));
            Assert.That(filesFile.Filepath, Is.EqualTo("/"));
            Assert.That(filesFile.Filesize, Is.EqualTo(""));
            Assert.That(filesFile.Mimetype, Is.EqualTo("application/json"));
            Assert.That(filesFile.Timecreated, Is.EqualTo(""));
            Assert.That(filesFile.Timemodified, Is.EqualTo(""));
            Assert.That(filesFile.Source, Is.EqualTo(""));
            Assert.That(filesFile.Author, Is.EqualTo("$@NULL@$"));
            Assert.That(filesFile.Sortorder, Is.EqualTo("0"));
            Assert.That(filesFile.Userid, Is.EqualTo("$@NULL@$"));
            Assert.That(filesFile.Status, Is.EqualTo("0"));
            Assert.That(filesFile.License, Is.EqualTo("unknown"));
            Assert.That(filesFile.RepositoryType, Is.EqualTo("$@NULL@$"));
            Assert.That(filesFile.RepositoryId, Is.EqualTo("$@NULL@$"));
            Assert.That(filesFile.Reference, Is.EqualTo("$@NULL@$"));
        });
    }
}