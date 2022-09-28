using LanguageProvider;
namespace LanguageProvider;

public static class LanguageProviderForRazor
{
    private static LanguageProvider _languageProvider;
    private static string? _pathToDatabaseDirectory = "../LanguageProvider/Database/";
    private static string _databaseFileName = "de";
    static LanguageProviderForRazor()
    {
        _languageProvider = new LanguageProvider(_databaseFileName, _pathToDatabaseDirectory);
    }

    public static string GetContent(string spaceName, string tagName)
    {
        var output = "Something went Wrong in LanguageProviderForRazor!";
        try
        {
            output = _languageProvider.GetContent(spaceName, tagName);
        }
        catch (Exceptions.LanguageProviderBaseException error)
        {
            output = error.Message;
        }

        return output;
    }

    public static void SetLanguage(string databaseFileName)
    {
        _languageProvider.SetLanguage(databaseFileName);
    }
    
}