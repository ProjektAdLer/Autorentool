using System.Text.RegularExpressions;
using Tommy;

namespace LanguageProvider
{
    public static class LanguageProvider
    {
        private static string _mLanguage = "";
        static readonly string _mPathToDatabaseDirectory = "../LanguageProvider/Database/";
        static readonly string _mFileExtension = ".toml";
        private static TomlTable? _mDatabase = null;

        public static void SetLanguage(string databaseName)
        {
            if (IsAlphaNumeric(databaseName))
            {
                _mLanguage = databaseName;
                LoadDatabase();
            }
            else
            {
                throw new Exceptions.DatabaseNameFormatException("Name Included a not alphanumeric character");
            }
        }

        /// <summary>
        /// gets the content of the specified tag inside the specified table/space
        /// </summary>
        /// <param name="spaceName">The space within the database to operate in</param>
        /// <param name="tagName">The tag to look up in the database</param>
        /// <returns>string</returns>
        /// <exception cref="DatabaseEmptyTableException">Thrown if the table_empty Tag in Database table is set to true</exception>
        /// <exception cref="DatabaseLookupException">Thrown if the delivered Tag is not found in the specified table</exception>
        public static string GetContent(string spaceName, string tagName)
        {
            string? output = "Something went wrong!";

            try
            {
                if (_mDatabase is not null)
                {
                    if (_mDatabase[spaceName]["table_empty"].AsBoolean)
                    {
                        throw new Exceptions.DatabaseEmptyTableException($"DatabaseEmptyTableException: '{spaceName}' was empty!");
                    }
                    else
                    {
                        try
                        {
                            output = _mDatabase[spaceName][tagName].ToString();
                        }
                        catch (Exception errorMessage)
                        {
                            throw new Exceptions.DatabaseLookupException($"DatabaseLookupException: '{tagName}' was not found in '{spaceName}'");
                        }
                    }
                }
                else
                {
                    throw new Exceptions.DatabaseLoadException($"Failed to Load Database '{_mPathToDatabaseDirectory + _mLanguage + _mFileExtension}'");
                }
            }
            catch (Exceptions.LanguageProviderBaseException error)
            {
                Console.WriteLine(error.Message);
                output = error.Message;
            }
            
            return output;
        }
        private static void LoadDatabase()
        {
            string pathToDatabase = _mPathToDatabaseDirectory + _mLanguage + _mFileExtension;
            try
            {
                using (StreamReader reader = File.OpenText(pathToDatabase))
                {
                    _mDatabase = TOML.Parse(reader);
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                throw new Exceptions.DatabaseLoadException(error.Message);
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
        public class DatabaseNameFormatException : LanguageProviderBaseException
        {
            public DatabaseNameFormatException() : base()
            {
            }
            public DatabaseNameFormatException(string message) : base(message)
            {
            }
        }
    }
}