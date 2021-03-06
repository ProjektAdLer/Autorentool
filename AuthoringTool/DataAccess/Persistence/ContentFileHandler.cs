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
        var fileType = Path.GetExtension(filepath).Trim('.').ToLower();
        var fileName = Path.GetFileName(filepath);
        _logger.LogInformation($"File {fileName} of type {fileType} loaded");
        return new LearningContent(fileName, fileType, fileBytes);
    }

    public LearningContent LoadFromStream(string name, Stream stream)
    {
        var fileBytes = ((MemoryStream)stream).ToArray();
        var fileType = Path.GetExtension(name).Split(".").Last().ToLower();
        var fileName = Path.GetFileName(name);
        _logger.LogInformation($"File {fileName} of type {fileType} loaded");
        return new LearningContent(fileName, fileType, fileBytes);
    }
}