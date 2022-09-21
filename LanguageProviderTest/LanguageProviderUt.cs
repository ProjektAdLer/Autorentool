using System.Net;
using System.Reflection.Metadata;
using NUnit.Framework;
using LanguageProvider;
using LanguageProvider.Exceptions;

namespace LanguageProviderTest;
[TestFixture]
public class LanguageProviderUt
{
    [Test]
    public void LanguageProvider_SetLanguage_SetsCorrectDatabase_And_GetsCorrectContent_ForCorrectKeys()
    {   
        var firstDatabase = "FirstTestDatabase";
        var secondDatabase = "SecondTestDatabase";
        
        Directory.SetCurrentDirectory("./LanguageProvider/");

        LanguageProvider.LanguageProvider.SetLanguage(firstDatabase);
        Assert.That(LanguageProvider.LanguageProvider.GetContent("FirstTable", "FirstKey"), Is.EqualTo("First Database, First Table, First Key"));
        Assert.That(LanguageProvider.LanguageProvider.GetContent("FirstTable", "SecondKey"), Is.EqualTo("First Database, First Table, Second Key"));
        Assert.That(LanguageProvider.LanguageProvider.GetContent("FirstTable", "table_empty"), Is.EqualTo(false.ToString()));
        
        Assert.That(LanguageProvider.LanguageProvider.GetContent("SecondTable", "FirstKey"), Is.EqualTo("First Database, Second Table, First Key"));
        Assert.That(LanguageProvider.LanguageProvider.GetContent("SecondTable", "SecondKey"), Is.EqualTo("First Database, Second Table, Second Key"));
        Assert.That(LanguageProvider.LanguageProvider.GetContent("SecondTable", "table_empty"), Is.EqualTo(false.ToString()));

        LanguageProvider.LanguageProvider.SetLanguage(secondDatabase);
        Assert.That(LanguageProvider.LanguageProvider.GetContent("FirstTable", "FirstKey"), Is.EqualTo("Second Database, First Table, First Key"));
        Assert.That(LanguageProvider.LanguageProvider.GetContent("FirstTable", "SecondKey"), Is.EqualTo("Second Database, First Table, Second Key"));
        Assert.That(LanguageProvider.LanguageProvider.GetContent("FirstTable", "table_empty"), Is.EqualTo(false.ToString()));
        
        Assert.That(LanguageProvider.LanguageProvider.GetContent("SecondTable", "FirstKey"), Is.EqualTo("Second Database, Second Table, First Key"));
        Assert.That(LanguageProvider.LanguageProvider.GetContent("SecondTable", "SecondKey"), Is.EqualTo("Second Database, Second Table, Second Key"));
        Assert.That(LanguageProvider.LanguageProvider.GetContent("SecondTable", "table_empty"), Is.EqualTo(false.ToString()));
        Directory.SetCurrentDirectory("../");
    }

    [TestCase(".FirstTestDatabase")]
    [TestCase("FirstTestD,atabase")]
    [TestCase("F//irstTestDatabase")]
    [TestCase("F/irstTestDatabase")]
    [TestCase("\\FirstTestDatabase")]
    [TestCase("$FirstTestD%atabase")]
    public void LanguageProvider_SetLanguage_ThrowsException_ForInvalidFileName(string databaseName)
    {
        Directory.SetCurrentDirectory("./LanguageProvider/");

        Assert.Throws<DatabaseNameFormatException>(() => { LanguageProvider.LanguageProvider.SetLanguage(databaseName);}, "Name Included a not alphanumeric character");
        Directory.SetCurrentDirectory("../");
    }
    
}


