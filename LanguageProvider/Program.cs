using LanguageProvider;

public class Program
{
    public static void Main()
    {
        var provider = new LanguageProvider.LanguageProvider("testDatabase");
        provider.Debug();
    }
}

