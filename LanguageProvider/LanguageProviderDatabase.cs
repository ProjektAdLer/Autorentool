using System.Text.RegularExpressions;
using Tommy;
namespace LanguageProvider;

public class LanguageProviderDatabase
{
    private string? MPathToDatabase { get; }
    public TomlTable? Database;

    public LanguageProviderDatabase()
    {
        throw new Exceptions.DatabaseNullException("PATH_TO_DATABASE_NULL_EXCEPTION: No path to Database was set!");
    }
    
    public LanguageProviderDatabase(string pathToDatabase)
    {
        MPathToDatabase = pathToDatabase;
        LoadDatabase();
    }

    private void LoadDatabase()
    {
        try
        {
            if (File.Exists(MPathToDatabase))
            {
                using var reader = File.OpenText(MPathToDatabase);
                Database = TOML.Parse(reader);
            }
            else
            {
                throw new Exceptions.DatabaseNotFoundException(
                    $"DATABASE_NOT_FOUND: Failed to find Database in '{Directory.GetCurrentDirectory()}\\{MPathToDatabase}'!");
            }
        }
        catch (TomlParseException error)
        {
            throw new Exceptions.DatabaseParseException($"DATABASE_PARSING_ERROR:Failed to Load Database '{Directory.GetCurrentDirectory()}\\{ MPathToDatabase}'!"); 
        }
    }
}