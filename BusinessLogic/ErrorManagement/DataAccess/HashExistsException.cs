namespace BusinessLogic.ErrorManagement.DataAccess;

public class HashExistsException : Exception
{
    public HashExistsException(string duplicateFileName, string duplicateFilePath)
    {
        DuplicateFileName = duplicateFileName;
        DuplicateFilePath = duplicateFilePath;
    }

    public HashExistsException(string message, string duplicateFileName, string duplicateFilePath) : base(message)
    {
        DuplicateFileName = duplicateFileName;
        DuplicateFilePath = duplicateFilePath;
    }

    public string DuplicateFileName { get; }
    public string DuplicateFilePath { get; }
}