using System.Diagnostics;
using System.Runtime.CompilerServices;
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
                throw new Exceptions.DatabaseNameFormatException($"DATABASE_NAME_FORMAT_EXCEPTION: Name Included a not alphanumeric character");
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
            var output = "Something went wrong!";
            try
            {
                if (_mDatabase is not null)
                {
                    if (_mDatabase[spaceName]["table_empty"].AsBoolean)
                    {
                        throw new Exceptions.DatabaseEmptyTableException($"DATABASE_EMPTY_TABLE_EXCEPTION: '{spaceName}' in '{_mLanguage}' was empty!");
                    }
                    else
                    {
                        try
                        {
                            output = _mDatabase[spaceName][tagName].AsString.ToString();
                        }
                        catch (Exception errorMessage)
                        {
                            throw new Exceptions.DatabaseLookupException($"DATABASE_LOOKUP_EXCEPTION: '{tagName}' was not found in '{spaceName}'");
                        }
                    }
                }
                else
                {
                    throw new Exceptions.DatabaseNullException($"DATABASE_NULL_EXCEPTION: No Database loaded into memory!");
                }
            }
            catch (Exceptions.LanguageProviderBaseException error)
            {
                output = error.Message;
            }
            return output;
        }
        private static void LoadDatabase()
        {
            string pathToDatabase = _mPathToDatabaseDirectory + _mLanguage + _mFileExtension;
            try
            {
                if (File.Exists(pathToDatabase))
                {
                    using var reader = File.OpenText(pathToDatabase);
                    _mDatabase = TOML.Parse(reader);
                }
                else
                {
                    throw new Exceptions.DatabaseNotFoundException($"DATABASE_NOT_FOUND: Failed to find Database in '{pathToDatabase}'!");
                }
                
            }
            catch (Exception error)
            {
                if (error.GetType() == typeof(TomlParseException))
                {
                    throw new Exceptions.DatabaseParseException($"DATABASE_PARSING_ERROR:Failed to Load Database '{_mLanguage + _mFileExtension}'!");
                }
                if (error.GetType() == typeof(Exceptions.DatabaseNotFoundException))
                {
                    throw new Exceptions.DatabaseNotFoundException(error.Message);
                }
            }
        }
        
        //for testing only
        public static void Unload()
        {
            _mLanguage = "";
            _mDatabase = null;
        }
        private static bool IsAlphaNumeric(string strToCheck)
        {
            Regex rg = new Regex(@"^[a-zA-Z0-9\s]*$");
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
        public class DatabaseLoaderException : LanguageProviderBaseException
        {
            public DatabaseLoaderException() : base()
            {
            }

            public DatabaseLoaderException(string message) : base(message)
            {
            }
        }
        public class DatabaseParseException : DatabaseLoaderException
        {
            public DatabaseParseException() : base()
            {
            }

            public DatabaseParseException(string message) : base(message)
            {
            }
        }
        public class DatabaseNullException : DatabaseLoaderException
        {
            public DatabaseNullException() : base()
            {
            }

            public DatabaseNullException(string message) : base(message)
            {
            }
        }
        public class DatabaseNotFoundException : DatabaseLoaderException
        {
            public DatabaseNotFoundException() : base()
            {
            }

            public DatabaseNotFoundException(string message) : base(message)
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