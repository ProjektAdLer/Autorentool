using NUnit.Framework;
using LanguageProvider.Exceptions;
using LanguageProvider;

namespace LanguageProviderTest;
[TestFixture]
public class LanguageProviderUt
{
    private string _pathToDatabaseDirectory = $".\\Database\\";

    [Test]
    public void LanguageProvider_SetLanguage_SetsCorrectDatabase_And_GetsCorrectContent_ForCorrectKeys()
    {   
        var firstDatabase = "FirstTestDatabase";
        var secondDatabase = "SecondTestDatabase";
        
        var systemUnderTest = GetLanguageProvider_ForTests(firstDatabase);
        Assert.That(systemUnderTest.GetContent("FirstTable", "FirstKey"), Is.EqualTo("First Database, First Table, First Key"));
        Assert.That(systemUnderTest.GetContent("FirstTable", "SecondKey"), Is.EqualTo("First Database, First Table, Second Key"));
        
        Assert.That(systemUnderTest.GetContent("SecondTable", "FirstKey"), Is.EqualTo("First Database, Second Table, First Key"));
        Assert.That(systemUnderTest.GetContent("SecondTable", "SecondKey"), Is.EqualTo("First Database, Second Table, Second Key"));

        systemUnderTest.SetLanguage(secondDatabase);
        Assert.That(systemUnderTest.GetContent("FirstTable", "FirstKey"), Is.EqualTo("Second Database, First Table, First Key"));
        Assert.That(systemUnderTest.GetContent("FirstTable", "SecondKey"), Is.EqualTo("Second Database, First Table, Second Key"));
        
        Assert.That(systemUnderTest.GetContent("SecondTable", "FirstKey"), Is.EqualTo("Second Database, Second Table, First Key"));
        Assert.That(systemUnderTest.GetContent("SecondTable", "SecondKey"), Is.EqualTo("Second Database, Second Table, Second Key"));
    }

    [TestCase(".FirstTestDatabase")]
    [TestCase("FirstTestD,atabase")]
    [TestCase("F//irstTestDatabase")]
    [TestCase("F/irstTestDatabase")]
    [TestCase("\\FirstTestDatabase")]
    [TestCase("$FirstTestD%atabase")]
    public void LanguageProvider_SetLanguage_ThrowsNameFormatException_ForInvalidFormattedFileName(string databaseName)
    {
        
        Assert.Throws<DatabaseNameFormatException>(() => { var systemUnderTest = GetLanguageProvider_ForTests(databaseName);;}, "Name Included a not alphanumeric character");
    }

    [TestCase("FourthTestDatabase")]
    public void LanguageProvider_GetContent_ThrowsInnerException_ForLookingUpEmptyTable(string databaseName)
    {
        var systemUnderTest = GetLanguageProvider_ForTests(databaseName);
        var spaceName = "test";
        Assert.Throws<DatabaseEmptyTableException>(() => { systemUnderTest.GetContent(spaceName, "test"); },
            $"DATABASE_EMPTY_TABLE_EXCEPTION: '{spaceName}' in '{databaseName}' was empty!");
    }
    
    [TestCase("FourthTestDatabase")]
    public void LanguageProvider_GetContent_ThrowsDatabaseLookUpException_ForWrongTag_InCorrectTable(string databaseName)
    {
        var systemUnderTest = GetLanguageProvider_ForTests(databaseName);
        var spaceName = "FirstTable";
        var tagName = "test";
        Assert.Throws<DatabaseLookupException>(() => { systemUnderTest.GetContent(spaceName, tagName); },
            $"DATABASE_LOOKUP_EXCEPTION: '{tagName}' was not found in '{spaceName}'");
    }

    private LanguageProvider.LanguageProvider GetLanguageProvider_ForTests(string fileName)
    {
        return new LanguageProvider.LanguageProvider(fileName, _pathToDatabaseDirectory);
    }
}