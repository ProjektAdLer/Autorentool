using System.Text.RegularExpressions;
using Tommy;

namespace LanguageProvider
{
    public class LanguageProvider
    {
        private static string mLanguage = "en";
        private static string mPathToDatabaseDirectory = "./Database/";
        private static string mFileExtension = ".toml";
        private static TomlTable? mDatabase = null;
        private static string mSpaceName = "";

        public LanguageProvider()
        {
            LoadDatabase();
        }

        public LanguageProvider(string databaseName)
        {
            if (IsAlphaNumeric(databaseName))
            {
                mLanguage = databaseName;
            }
            LoadDatabase();
        }
        
        /// <summary>
        /// This function gets the content of the specified tag inside the specified table/space
        /// </summary>
        /// <param name="spaceName">The space within the database to operate in</param>
        /// <param name="tagName">The tag to look up in the database</param>
        /// <returns>string</returns>
        /// <exception cref="DatabaseEmptyTableException">Thrown if the table_empty Tag in Database table is set to true</exception>
        /// <exception cref="DatabaseLookupException">Thrown if the delivered Tag is not found in the specified table</exception>
        public static string GetContent(string spaceName, string tagName)
        {
            string? output = "Something went wrong!";

            if (mDatabase is not null)
            {
                if (mDatabase[spaceName]["table_empty"].AsBoolean)
                {
                    output = $"Database Table '{spaceName}' was empty!";
                    throw new Exceptions.DatabaseEmptyTableException($"TOML table: '{spaceName}' was empty!");
                }
                else
                {
                    try
                    {
                        output = mDatabase[spaceName][tagName].ToString();
                    }
                    catch (Exception errorMessage)
                    {
                        output = $"'{tagName}' not found in '{spaceName}'!";
                        throw new Exceptions.DatabaseLookupException($"Tag: '{tagName}' was not found in '{spaceName}'");
                    }
                }
            }
            return output;
        }
        private static void LoadDatabase()
        {
            string pathToDatabase = mPathToDatabaseDirectory + mLanguage + mFileExtension;
            try
            {
                using (StreamReader reader = File.OpenText(pathToDatabase))
                {
                    mDatabase = TOML.Parse(reader);
                }
            }
            catch (Exception errorMessage)
            {
                Console.WriteLine(errorMessage);
                throw new Exceptions.DatabaseLoadException(errorMessage.Message);
            }
        }
        private static bool IsAlphaNumeric(string strToCheck)
        {
            Regex rg = new Regex(@"^[a-zA-Z0-9\s,]*$");
            return rg.IsMatch(strToCheck);
        }
    }

    namespace Exceptions
    {
        public class LanguageProviderBaseException : Exception
        {
            public LanguageProviderBaseException() : base()
            {
            }

            public LanguageProviderBaseException(string message) : base(message)
            {
            }
        }
        public class DatabaseLoadException : LanguageProviderBaseException
        {
            public DatabaseLoadException() : base()
            {
            }

            public DatabaseLoadException(string message) : base(message)
            {
            }
        }
        public class DatabaseEmptyTableException : LanguageProviderBaseException
        {
            public DatabaseEmptyTableException() : base()
            {
            }
            public DatabaseEmptyTableException(string message) : base(message)
            {
            }
        }
        public class DatabaseLookupException : LanguageProviderBaseException
        {
            public DatabaseLookupException() : base()
            {
            }
            public DatabaseLookupException(string message) : base(message)
            {
            }
        }
    }
}