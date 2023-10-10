namespace Shared;

public static class PathHelper
{
    public static string TrimFileName(string fileName)
    {
        //Trim whitespace from file ending
        var fileEnding = fileName.Split(".").Skip(1).Last();
        var trimmedFileEnding = fileEnding.Trim();
        fileName = fileName.Replace(fileEnding, trimmedFileEnding);
        //Replace spaces with underscores in file name
        fileName = fileName.Replace(" ", "_");
        return fileName;
    }
}