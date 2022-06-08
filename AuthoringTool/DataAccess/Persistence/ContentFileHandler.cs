using System.IO.Abstractions;
using AuthoringTool.Entities;

namespace AuthoringTool.DataAccess.Persistence;

public class ContentFileHandler : IContentFileHandler
{
    private readonly ILogger<ContentFileHandler> _logger;
    private readonly IFileSystem _fileSystem;

    internal ContentFileHandler(ILogger<ContentFileHandler> logger,IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
        _logger = logger;
    }
   
    public ContentFileHandler(ILogger<ContentFileHandler> logger):this(logger, new FileSystem()){ }

    public LearningContent LoadFromDisk(string filepath)
    {
        var fileBytes = _fileSystem.File.ReadAllBytes(filepath);
        var fileType = Path.GetExtension(filepath);
        var fileName = Path.GetFileName(filepath);
        _logger.LogInformation($"File {fileName} of type {fileType} loaded");
        return new LearningContent(fileName, fileType, fileBytes);
    }
}