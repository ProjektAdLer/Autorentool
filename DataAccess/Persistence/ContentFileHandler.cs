using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using PersistEntities;

namespace DataAccess.Persistence;

public class ContentFileHandler : IContentFileHandler
{
    private readonly ILogger<ContentFileHandler> _logger;
    private readonly IFileSystem _fileSystem;

    public ContentFileHandler(ILogger<ContentFileHandler> logger,IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
        _logger = logger;
    }
   

    public LearningContentPe LoadFromDisk(string filepath)
    {
        var fileType = Path.GetExtension(filepath).Trim('.').ToLower();
        var fileName = Path.GetFileName(filepath);
        _logger.LogInformation("File {FileName} of type {FileType} loaded", fileName, fileType);
        return new LearningContentPe(fileName, fileType, filepath);
    }

    public LearningContentPe LoadFromStream(string name, Stream stream)
    {
        var fileBytes = ((MemoryStream)stream).ToArray();
        var fileType = Path.GetExtension(name).Split(".").Last().ToLower();
        var fileName = Path.GetFileName(name);
        _logger.LogInformation("File {FileName} of type {FileType} loaded", fileName, fileType);
        return new LearningContentPe(fileName, fileType, "");
    }
}