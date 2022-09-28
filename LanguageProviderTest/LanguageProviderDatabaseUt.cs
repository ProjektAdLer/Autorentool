using LanguageProvider;
using LanguageProvider.Exceptions;
using NUnit.Framework;

namespace LanguageProviderTest;
[TestFixture]
public class LanguageProviderDatabaseUt
{
    private string _pathToDatabaseDirectory = $".\\Database\\";
    [Test]
    public void LanguageProvider_GetContent_NullException_ForMissingConstructorArguments()
    {
        Assert.Throws<DatabaseNullException>(() =>
        {
            var systemUnderTest = new LanguageProvider.LanguageProviderDatabase();
        }, "");
    }
    
    [TestCase("skazzle")]
    [TestCase("bazzle")]
    [TestCase("root")]
    public void LanguageProvider_SetLanguage_ThrowsException_ForNotExistingFileName(string databaseFileName)
    {
        var pathToDatabase = $"{_pathToDatabaseDirectory}{databaseFileName}.toml";
        Assert.Throws<DatabaseNotFoundException>(() => { var systemUnderTest = new LanguageProviderDatabase(pathToDatabase);}, $"DATABASE_NOT_FOUND: Failed to find Database in '{pathToDatabase}'!");
    }

    [TestCase("ThirdTestDatabase")]
    public void LanguageProvider_SetLanguage_ThrowsParsingException_ForInvalidDatabaseFormatting(string databaseFileName)
    {
        var pathToDatabase = $"{_pathToDatabaseDirectory}{databaseFileName}.toml";
        Assert.Throws<DatabaseParseException>(() => { var systemUnderTest = GetLanguageProvider_ForTests(pathToDatabase);}, $"DATABASE_PARSING_ERROR:Failed to Load Database '{pathToDatabase}'!");
    }
    
    private LanguageProviderDatabase GetLanguageProvider_ForTests(string databasePath)
    {
        return new LanguageProviderDatabase(databasePath);
    }
}