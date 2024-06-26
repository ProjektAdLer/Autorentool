﻿using System.IO.Abstractions.TestingHelpers;
using Generator.WorldExport;
using Generator.XmlClasses.Entities._activities.Module.xml;
using NUnit.Framework;
using Shared.Configuration;

namespace GeneratorTest.XmlClasses.Entities._activities.Module.xml;

[TestFixture]
public class ActivitiesModuleXmlModuleUt
{
    [Test]
    public void ActivitiesModuleXmlModule_StandardConstructor_AllParametersSet()
    {
        //Arrange


        //Act
        var systemUnderTest = new ActivitiesModuleXmlModule();

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.ModuleName, Is.EqualTo(""));
            Assert.That(systemUnderTest.SectionId, Is.EqualTo(""));
            Assert.That(systemUnderTest.SectionNumber, Is.EqualTo(""));
            Assert.That(systemUnderTest.IdNumber, Is.EqualTo(""));
            Assert.That(systemUnderTest.Added, Is.EqualTo(""));
            Assert.That(systemUnderTest.Score, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Indent, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Visible, Is.EqualTo("1"));
            Assert.That(systemUnderTest.VisibleOnCoursePage, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Visibleold, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Groupmode, Is.EqualTo("0"));
            Assert.That(systemUnderTest.GroupingId, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Completion, Is.EqualTo("1"));
            Assert.That(systemUnderTest.Completiongradeitemnumber, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.CompletionView, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Completionexpected, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Availability, Is.EqualTo("$@NULL@$"));
            Assert.That(systemUnderTest.ShowDescription, Is.EqualTo("0"));
            Assert.That(systemUnderTest.Tags, Is.EqualTo(""));
            Assert.That(systemUnderTest.Id, Is.EqualTo(""));
            Assert.That(systemUnderTest.Version, Is.EqualTo("2021051700"));
        });
    }

    [Test]
    // ANF-ID: [GHO11]
    public void ActivitiesModuleXmlModule_Serialize_XmlFileWritten()
    {
        //Arrange
        var mockFileSystem = new MockFileSystem();
        var currWorkDir = ApplicationPaths.BackupFolder;
        mockFileSystem.AddDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "activities", "h5pactivity_2"));

        var systemUnderTest = new ActivitiesModuleXmlModule();
        XmlSerializeFileSystemProvider.FileSystem = mockFileSystem;

        //Act
        systemUnderTest.Serialize("h5pactivity", "2");

        //Assert
        var path = Path.Join(currWorkDir, "XMLFilesForExport", "activities", "h5pactivity_2", "module.xml");
        Assert.That(mockFileSystem.FileExists(path), Is.True);
    }
}