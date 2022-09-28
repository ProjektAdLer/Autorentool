using System.Text.RegularExpressions;

namespace LanguageProvider
{
    public class LanguageProvider
    {
        private string DatabaseFileName;
        private string PathToDatabaseDirectory;
        private string FileExtension = ".toml";
        private LanguageProviderDatabase _languageProviderDatabase;
        private string? PathToDatabase = null;
        public LanguageProvider()
        {
            _languageProviderDatabase = new LanguageProviderDatabase();
        }
        public LanguageProvider(string databaseFileName, string? pathToDatabaseDirectory = null)
        {
            if (pathToDatabaseDirectory is not null)
            {
                PathToDatabaseDirectory = pathToDatabaseDirectory;
            }
            else
            {
                PathToDatabaseDirectory = "../Database/";
            }
            
            DatabaseFileName = databaseFileName;
            SetLanguage(databaseFileName);
        }
        public void SetLanguage(string databaseFileName)
        {
            if (IsStringAlphaNumeric(databaseFileName))
            {
                DatabaseFileName = databaseFileName;
                _languageProviderDatabase = new LanguageProviderDatabase($"{PathToDatabaseDirectory + DatabaseFileName + FileExtension}");
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
        public string GetContent(string spaceName, string tagName)
        {
            var output = "Something went wrong in LanguageProvider!";
            if (_languageProviderDatabase.Database is null)
            {
                throw new Exceptions.DatabaseNullException($"DATABASE_NULL_EXCEPTION: No Database loaded into memory!");
            }

            if (_languageProviderDatabase.Database[spaceName]["table_empty"].AsBoolean)
            {
                throw new Exceptions.DatabaseEmptyTableException($"DATABASE_EMPTY_TABLE_EXCEPTION: '{spaceName}' in '{Directory.GetCurrentDirectory() + PathToDatabase}' was empty!");
            }
                
            try
            {
                output = _languageProviderDatabase.Database[spaceName][tagName].AsString.ToString();
            }
            catch (Exception errorMessage)
            {
                throw new Exceptions.DatabaseLookupException(
                    $"DATABASE_LOOKUP_EXCEPTION: '{tagName}' was not found in '{spaceName}'");
            }
            return output;
        }

        private bool IsStringAlphaNumeric(string strToCheck)
        {
            Regex rg = new Regex(@"^[a-zA-Z0-9\s]*$");
            return rg.IsMatch(strToCheck);
        }
    }
}