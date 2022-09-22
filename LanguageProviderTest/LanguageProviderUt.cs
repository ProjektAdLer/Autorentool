using System.Net;
using System.Reflection.Metadata;
using NUnit.Framework;
using LanguageProvider;
using LanguageProvider.Exceptions;
using Tommy;

namespace LanguageProviderTest;
[TestFixture]
public class LanguageProviderUt
{
    private string _pathToDatabaseDirectory = $"{Directory.GetCurrentDirectory()}/LanguageProvider/";

    [Test]
    public void LanguageProvider_SetLanguage_SetsCorrectDatabase_And_GetsCorrectContent_ForCorrectKeys()
    {   
        var firstDatabase = "FirstTestDatabase";
        var secondDatabase = "SecondTestDatabase";
        
        Directory.SetCurrentDirectory(_pathToDatabaseDirectory);

        LanguageProvider.LanguageProvider.SetLanguage(firstDatabase);
        Assert.That(LanguageProvider.LanguageProvider.GetContent("FirstTable", "FirstKey"), Is.EqualTo("First Database, First Table, First Key"));
        Assert.That(LanguageProvider.LanguageProvider.GetContent("FirstTable", "SecondKey"), Is.EqualTo("First Database, First Table, Second Key"));
        
        Assert.That(LanguageProvider.LanguageProvider.GetContent("SecondTable", "FirstKey"), Is.EqualTo("First Database, Second Table, First Key"));
        Assert.That(LanguageProvider.LanguageProvider.GetContent("SecondTable", "SecondKey"), Is.EqualTo("First Database, Second Table, Second Key"));

        LanguageProvider.LanguageProvider.SetLanguage(secondDatabase);
        Assert.That(LanguageProvider.LanguageProvider.GetContent("FirstTable", "FirstKey"), Is.EqualTo("Second Database, First Table, First Key"));
        Assert.That(LanguageProvider.LanguageProvider.GetContent("FirstTable", "SecondKey"), Is.EqualTo("Second Database, First Table, Second Key"));
        
        Assert.That(LanguageProvider.LanguageProvider.GetContent("SecondTable", "FirstKey"), Is.EqualTo("Second Database, Second Table, First Key"));
        Assert.That(LanguageProvider.LanguageProvider.GetContent("SecondTable", "SecondKey"), Is.EqualTo("Second Database, Second Table, Second Key"));
        LanguageProvider.LanguageProvider.Unload();
    }

    [TestCase(".FirstTestDatabase")]
    [TestCase("FirstTestD,atabase")]
    [TestCase("F//irstTestDatabase")]
    [TestCase("F/irstTestDatabase")]
    [TestCase("\\FirstTestDatabase")]
    [TestCase("$FirstTestD%atabase")]
    public void LanguageProvider_SetLanguage_ThrowsException_ForInvalidFormattedFileName(string databaseName)
    {
        Directory.SetCurrentDirectory(_pathToDatabaseDirectory);

        Assert.Throws<DatabaseNameFormatException>(() => { LanguageProvider.LanguageProvider.SetLanguage(databaseName);}, "Name Included a not alphanumeric character");
        LanguageProvider.LanguageProvider.Unload();
    }

    [TestCase("skazzle")]
    [TestCase("bazzle")]
    [TestCase("root")]
    public void LanguageProvider_SetLanguage_ThrowsException_ForInvalidFileName_WithCorrectFileNameFormatting(string databaseName)
    {
        Directory.SetCurrentDirectory(_pathToDatabaseDirectory);
        var pathToDatabase = $"{Directory.GetCurrentDirectory()}/LanguageProvider/Database/{databaseName}.toml";
        Assert.Throws<DatabaseNotFoundException>(() => { LanguageProvider.LanguageProvider.SetLanguage(databaseName);}, $"DATABASE_NOT_FOUND: Failed to find Database in '{pathToDatabase}'!");
        Directory.SetCurrentDirectory("../");
        LanguageProvider.LanguageProvider.Unload();
    }
    
    [TestCase("ThirdTestDatabase")]
    public void LanguageProvider_SetLanguage_ThrowsException_ForInvalidDatabaseFormatting(string databaseName)
    {
        Directory.SetCurrentDirectory(_pathToDatabaseDirectory);
        Assert.Throws<DatabaseParseException>(() => { LanguageProvider.LanguageProvider.SetLanguage(databaseName);}, $"DATABASE_PARSING_ERROR:Failed to Load Database '{databaseName}.toml'!");
        LanguageProvider.LanguageProvider.Unload();
    }

    [Test]
    public void LanguageProvider_GetContent_ThrowsInnerException_ForNoLoadedDatabase()
    {
        Directory.SetCurrentDirectory(_pathToDatabaseDirectory);
        Assert.That(LanguageProvider.LanguageProvider.GetContent("test", "test") , Is.EqualTo("DATABASE_NULL_EXCEPTION: No Database loaded into memory!"));
        LanguageProvider.LanguageProvider.Unload();
    }
    
    [TestCase("FourthTestDatabase")]
    public void LanguageProvider_GetContent_ThrowsInnerException_ForLookingUpEmptyTable(string databaseName)
    {
        Directory.SetCurrentDirectory(_pathToDatabaseDirectory);
        var spaceName = "test";
        LanguageProvider.LanguageProvider.SetLanguage(databaseName);
        Assert.That(LanguageProvider.LanguageProvider.GetContent(spaceName, "test") , Is.EqualTo($"DATABASE_EMPTY_TABLE_EXCEPTION: '{spaceName}' in '{databaseName}' was empty!"));
        LanguageProvider.LanguageProvider.Unload();
    }
    
    [TestCase("FourthTestDatabase")]
    public void LanguageProvider_GetContent_ThrowsInnerException_(string databaseName)
    {
        Directory.SetCurrentDirectory(_pathToDatabaseDirectory);
        var spaceName = "FirstTable";
        var tagName = "test";
        LanguageProvider.LanguageProvider.SetLanguage(databaseName);
        Assert.That(LanguageProvider.LanguageProvider.GetContent(spaceName, tagName) , Is.EqualTo($"DATABASE_LOOKUP_EXCEPTION: '{tagName}' was not found in '{spaceName}'"));
        LanguageProvider.LanguageProvider.Unload();
    }
}