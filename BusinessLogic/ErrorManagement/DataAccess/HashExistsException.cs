namespace BusinessLogic.ErrorManagement.DataAccess;

public class HashExistsException : Exception
{
    public HashExistsException(string duplicateFileName) : base()
    {
        DuplicateFileName = duplicateFileName;
    }

    public HashExistsException(string message, string duplicateFileName) : base(message)
    {
        DuplicateFileName = duplicateFileName;
    }

    public string DuplicateFileName { get; }
}