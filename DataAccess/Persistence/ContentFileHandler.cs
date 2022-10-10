using System.IO.Abstractions;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using PersistEntities;
using Shared.Extensions;

namespace DataAccess.Persistence;

public class ContentFileHandler : IContentFileHandler
{
    private readonly ILogger<ContentFileHandler> _logger;
    private readonly IFileSystem _fileSystem;

    private string ContentFilesFolderPath => Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "AdLerAuthoring", "ContentFiles");

    public ContentFileHandler(ILogger<ContentFileHandler> logger, IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
        _logger = logger;
        //make sure that the folder exists
        AssertContentFilesFolderExists();
        _logger.LogInformation("ContentFilesFolderPath is {}", ContentFilesFolderPath);
        AssertAllFilesHaveHashAsync();
    }
    
    private void AssertContentFilesFolderExists()
    {
        if (_fileSystem.Directory.Exists(ContentFilesFolderPath)) return;
        _logger.LogDebug("Folder {} did not exist, creating", ContentFilesFolderPath);
        _fileSystem.Directory.CreateDirectory(ContentFilesFolderPath);
    }

    public async Task<LearningContentPe> LoadContentAsync(string filepath)
    {
        var (duplicatePath, hash) = await ExistsAlreadyInContentFilesFolderAsync(filepath);
        if (duplicatePath == null)
            _logger.LogDebug("{} not found in {}, copying file", filepath, ContentFilesFolderPath);
        else
            _logger.LogDebug("{} found at {}, not copying", filepath, duplicatePath);
        
        var finalPath = duplicatePath ?? CopyFileToContentFilesFolder(filepath);
        
        var fileType = Path.GetExtension(finalPath).Trim('.').ToLower();
        var fileName = Path.GetFileName(finalPath);
        _logger.LogInformation("File {FileName} of type {FileType} loaded", fileName, fileType);
        if (duplicatePath == null)
            SaveHashForFileAsync(finalPath, hash);
        return new LearningContentPe(fileName, fileType, finalPath);
    }

    public async Task<LearningContentPe> LoadContentAsync(string name, MemoryStream stream)
    {
        var (duplicatePath, hash) = await ExistsAlreadyInContentFilesFolderAsync(stream);
        if (duplicatePath == null)
            _logger.LogDebug("stream {} not found in {}, copying file", name, ContentFilesFolderPath);
        else
            _logger.LogDebug("stream {} found at {}, not copying", name, duplicatePath);
        
        var finalPath = duplicatePath ?? await CopyFileToContentFilesFolderAsync(name, stream);
        
        var fileType = Path.GetExtension(name).Trim('.').ToLower();
        var fileName = Path.GetFileName(name);
        _logger.LogInformation("File {FileName} of type {FileType} loaded", fileName, fileType);
        if (duplicatePath == null)
            SaveHashForFileAsync(finalPath, hash);
        return new LearningContentPe(fileName, fileType, finalPath);
    }

    private async void SaveHashForFileAsync(string finalPath, byte[] hash)
    {
        var finalHashPath = finalPath + ".hash";
        await using var writeStream = _fileSystem.File.OpenWrite(finalHashPath);
        await writeStream.WriteAsync(new ReadOnlyMemory<byte>(hash));
        _logger.LogDebug("Wrote hash for {} to {}", finalPath, finalHashPath);
    }


    internal async Task<(string?,byte[])> ExistsAlreadyInContentFilesFolderAsync(string filepath)
    {
        if (string.IsNullOrWhiteSpace(filepath))
            throw new ArgumentException("Path cannot be null or whitespace.", nameof(filepath));
        await using var stream = _fileSystem.File.OpenRead(filepath);
        return await ExistsAlreadyInContentFilesFolderAsync(stream);
    }

    private async Task<(string?,byte[])> ExistsAlreadyInContentFilesFolderAsync(Stream stream)
    {
        AssertAllFilesHaveHashAsync();

        var streamHash = await ComputeHashAsync(stream);

        var match = _fileSystem.Directory
            .GetFiles(ContentFilesFolderPath, "*.hash")
            //pre-filter all hash files where length of actual file is not equal to length of stream
            .Where(hashPath => RealFileHasSameLength(stream, hashPath))
            .FirstOrDefault(path => streamHash.SequenceEqual(_fileSystem.File.ReadAllBytes(path)));
        return match == null ? (null, streamHash) : (Path.Join(ContentFilesFolderPath, Path.GetFileNameWithoutExtension(match)), streamHash);
    }

    private async void AssertAllFilesHaveHashAsync()
    {
        IEnumerable<(string realPath, string hashPath)> filesWithoutHash = _fileSystem.Directory.GetFiles(ContentFilesFolderPath)
            //macOS specific
            .Where(path => !path.EndsWith(".DS_Store"))
            .Where(path => !path.EndsWith(".hash"))
            .Where(path => !_fileSystem.File.Exists($"{path}.hash"))
            .Select(path => (path, $"{path}.hash"));
        foreach (var path in filesWithoutHash)
        {
            _logger.LogWarning("Found no hash file for real file at {}, creating one", path.realPath);
            await using var readStream = _fileSystem.File.OpenRead(path.realPath);
            await using var writeStream = _fileSystem.File.OpenWrite(path.hashPath);
            var hash = await ComputeHashAsync(readStream);
            await writeStream.WriteAsync(new ReadOnlyMemory<byte>(hash));
        }
    }

    private bool RealFileHasSameLength(Stream stream, string hashPath)
    {
        var filePath = Path.Join(ContentFilesFolderPath, Path.GetFileNameWithoutExtension(hashPath));
        try
        {
            using var fStream = _fileSystem.File.OpenRead(filePath);
            return fStream.Length == stream.Length;
        }
        catch (FileNotFoundException)
        {
            //log error and remove hash file
            _logger.LogWarning("Found hash file {} but no real file at {} when checking for duplicates, removing it",
                hashPath, filePath);
            _fileSystem.File.Delete(hashPath);
            return false;
        }
    }

    private static async Task<byte[]> ComputeHashAsync(Stream stream)
    {
        using var sha256 = SHA256.Create();
        stream.Position = 0;
        return await sha256.ComputeHashAsync(stream);
    }
    
    private string CopyFileToContentFilesFolder(string filePath)
    {
        var uniqueDestination = FindUniqueDestination(filePath);
        _fileSystem.File.Copy(filePath, uniqueDestination);
        return uniqueDestination;
    }

    private async Task<string> CopyFileToContentFilesFolderAsync(string file, MemoryStream stream)
    {
        var uniqueDestination = FindUniqueDestination(file);
        await using var writeStream = _fileSystem.File.OpenWrite(uniqueDestination);
        stream.Position = 0;
        stream.WriteTo(writeStream);
        return uniqueDestination;
    }

    private string FindUniqueDestination(string filePath)
    {
        var fileName = _fileSystem.Path.GetFileNameWithoutExtension(filePath);
        var fileExtension = _fileSystem.Path.GetExtension(filePath);
        //we must get a unique file name so we don't have duplicate file names in the folder
        var takenNames = _fileSystem.Directory.GetFiles(ContentFilesFolderPath)
            .Where(file => _fileSystem.Path.GetExtension(file) == fileExtension)
            .Select(_fileSystem.Path.GetFileNameWithoutExtension);
        var uniqueFilename = StringHelper.GetUniqueName(takenNames, fileName) + fileExtension;
        var uniqueDestination = Path.Combine(ContentFilesFolderPath, uniqueFilename);
        return uniqueDestination;
    }

}