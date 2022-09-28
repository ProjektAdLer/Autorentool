namespace LanguageProvider
{
    namespace Exceptions
    {
        public class LanguageProviderBaseException : Exception
        {
            public LanguageProviderBaseException()
            {
            }

            public LanguageProviderBaseException(string message) : base(message)
            {
            }
        }

        public class DatabaseLoaderException : LanguageProviderBaseException
        {
            public DatabaseLoaderException()
            {
            }

            public DatabaseLoaderException(string message) : base(message)
            {
            }
        }

        public class DatabaseParseException : DatabaseLoaderException
        {
            public DatabaseParseException()
            {
            }

            public DatabaseParseException(string message) : base(message)
            {
            }
        }

        public class DatabaseNullException : DatabaseLoaderException
        {
            public DatabaseNullException()
            {
            }

            public DatabaseNullException(string message) : base(message)
            {
            }
        }

        public class DatabaseNotFoundException : DatabaseLoaderException
        {
            public DatabaseNotFoundException()
            {
            }

            public DatabaseNotFoundException(string message) : base(message)
            {
            }
        }

        public class DatabaseEmptyTableException : LanguageProviderBaseException
        {
            public DatabaseEmptyTableException()
            {
            }

            public DatabaseEmptyTableException(string message) : base(message)
            {
            }
        }

        public class DatabaseLookupException : LanguageProviderBaseException
        {
            public DatabaseLookupException()
            {
            }

            public DatabaseLookupException(string message) : base(message)
            {
            }
        }

        public class DatabaseNameFormatException : LanguageProviderBaseException
        {
            public DatabaseNameFormatException()
            {
            }

            public DatabaseNameFormatException(string message) : base(message)
            {
            }
        }
    }
}