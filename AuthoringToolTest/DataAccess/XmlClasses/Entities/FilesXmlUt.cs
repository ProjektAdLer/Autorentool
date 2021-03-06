using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses.Entities;
using AuthoringTool.DataAccess.XmlClasses.XmlFileFactories;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities;

[TestFixture]
public class FilesXmlUt
{
    //There aren´t any Parameters to set yet.
    //Will be needed later, then the Test gets changed. 
    [Test]
    public void FilesXmlFile_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var filesFile = new FilesXmlFile();

        //Act
        filesFile.SetParameters("hashCheckSum", "h5pElementId", "mod_h5pactivity",
            "package",
            "0", "h5pElementName", "filesize", "application/zip.h5p", "/", "currentTime",
            "currentTime", "$@NULL@$", "0", "1000");
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(filesFile.Contenthash, Is.EqualTo("hashCheckSum"));
            Assert.That(filesFile.Contextid, Is.EqualTo("h5pElementId"));
            Assert.That(filesFile.Component, Is.EqualTo("mod_h5pactivity"));
            Assert.That(filesFile.Filearea, Is.EqualTo("package"));
            Assert.That(filesFile.Itemid, Is.EqualTo("0"));
            Assert.That(filesFile.Filename, Is.EqualTo("h5pElementName"));
            Assert.That(filesFile.Filesize, Is.EqualTo("filesize"));
            Assert.That(filesFile.Mimetype, Is.EqualTo("application/zip.h5p"));
            Assert.That(filesFile.Filepath, Is.EqualTo("/"));
            Assert.That(filesFile.Timecreated, Is.EqualTo("currentTime"));
            Assert.That(filesFile.Timemodified, Is.EqualTo("currentTime"));
            Assert.That(filesFile.Author, Is.EqualTo("$@NULL@$"));
            Assert.That(filesFile.Sortorder, Is.EqualTo("0"));
            Assert.That(filesFile.Id, Is.EqualTo("1000"));
        });
    }

    [Test]
    public void FilesXmlFiles_Serialize_ObjectsAreEqual()
    {
        //Arrange
        var filesFiles = new FilesXmlFiles();
        
        var file1 = new FilesXmlFile();
        var file2 = new FilesXmlFile();
        file1.SetParameters("hashCheckSum", "h5pElementId", "mod_h5pactivity",
            "package",
            "0", "h5pElementName", "filesize", "application/zip.h5p", "/", "currentTime",
            "currentTime", "$@NULL@$", "0", "1000");
        file2.SetParameters("hashCheckSum", "h5pElementId", "mod_h5pactivity",
            "package",
            "0", "h5pElementName", "filesize", "application/zip.h5p", "/", "currentTime",
            "currentTime", "$@NULL@$", "0", "1001");
        var list = new List<FilesXmlFile>
        {
            file1,
            file2 
        };


        //Act
        filesFiles.SetParameters(list);

        //Assert
        Assert.That(filesFiles.File, Is.EqualTo(list));
    }

    [Test]
    public void FilesXmlFiles_Serialize_XmlFileWritten()
    {
        //Arrange 
        var mockFileSystem = new MockFileSystem();
        var backupGenerator = new BackupFileGenerator(mockFileSystem);
        backupGenerator.CreateBackupFolders();
        var curWorkDir = mockFileSystem.Directory.GetCurrentDirectory();
        
        //Arrange
        var filesFiles = new FilesXmlFiles();
    
        var file1 = new FilesXmlFile();
        var file2 = new FilesXmlFile();
        file1.SetParameters("hashCheckSum", "h5pElementId", "mod_h5pactivity",
            "package",
            "0", "h5pElementName", "filesize", "application/zip.h5p", "/", "currentTime",
            "currentTime", "$@NULL@$", "0", "1000");
        file2.SetParameters("hashCheckSum", "h5pElementId", "mod_h5pactivity",
            "package",
            "0", "h5pElementName", "filesize", "application/zip.h5p", "/", "currentTime",
            "currentTime", "$@NULL@$", "0", "1001");
        var list = new List<FilesXmlFile>
        {
            file1,
            file2 
        };
        filesFiles.SetParameters(list);
        
        //Act
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;
        filesFiles.Serialize();
        
        //Assert
        var pathXmlFile = Path.Join(curWorkDir, "XMLFilesForExport", "files.xml");
        Assert.That(mockFileSystem.FileExists(pathXmlFile), Is.True);
    }
    
    
}