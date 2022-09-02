using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using Tomlet;
using Tomlet.Exceptions;
using Tomlet.Models;

namespace LanguageProvider
{
    public class LanguageProvider
    {
        private static string mLanguage = "en";
        private static string mPathToDatabaseDirectory = "/Database/";
        private static string mFileExtension = ".toml";
        private Dictionary<string, Dictionary<string, string>>? mDatabase = null;

        public LanguageProvider()
        {
            LoadDatabaseAndConvertToDictionary();
        }

        public LanguageProvider(string databaseName)
        {
            if (IsAlphaNumeric(databaseName))
            {
                mLanguage = databaseName;
            }

            LoadDatabaseAndConvertToDictionary();
        }

        public void Debug()
        {
            //var entries = TomletMain.To<string>(mDatabase);
            //Console.WriteLine(entries.ToString());
        }

        private static void LoadDatabaseAndConvertToDictionary()
        {
            TomlDocument? database = LoadDatabase();


            if (database is not null)
            {
                string databaseToString = database.ToString(); //Web request, read file, etc.
                var myClass = TomletMain.To<Dictionary<string, Dictionary<string, string>>>(databaseToString);
                Console.WriteLine("Debug ME");
                Console.WriteLine(myClass["AuthoringTool"]["table_empty"]); //Or whatever properties you define.
            }
        }

        private static TomlDocument? LoadDatabase()
        {
            TomlDocument? databaseDocument = null;
            string pathToDatabase = System.IO.Directory.GetCurrentDirectory() + mPathToDatabaseDirectory + mLanguage + mFileExtension;
            try
            {
                if (File.Exists(pathToDatabase))
                {
                    Console.WriteLine(pathToDatabase);
                    Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
                    databaseDocument = TomlParser.ParseFile(@pathToDatabase);
                }
                else
                {
                    throw new LanguageProviderException(
                        $"LanguageProviderException: Failed to find '{mLanguage + mFileExtension}'");
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }

            return databaseDocument;
        }

        public static string GetTag(string spaceName, string tagName)
        {
            return "m_Database.";
        }

        public class LanguageProviderException : Exception
        {
            public LanguageProviderException() : base()
            {
            }

            public LanguageProviderException(string message) : base(message)
            {
            }
        }

        private static bool IsAlphaNumeric(string strToCheck)
        {
            Regex rg = new Regex(@"^[a-zA-Z0-9\s,]*$");
            return rg.IsMatch(strToCheck);
        }
    }
}
