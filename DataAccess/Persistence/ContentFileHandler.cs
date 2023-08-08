using System.IO.Abstractions;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using PersistEntities.LearningContent;
using Shared.Extensions;

namespace DataAccess.Persistence;

public class ContentFileHandler : IContentFileHandler
{
    private const string LinkFile = ".linkstore";
    private readonly IFileSystem _fileSystem;
    private readonly ILogger<ContentFileHandler> _logger;
    private readonly IXmlFileHandler<List<LinkContentPe>> _xmlFileHandlerLink;

    public ContentFileHandler(ILogger<ContentFileHandler> logger, IFileSystem fileSystem,
        IXmlFileHandler<List<LinkContentPe>> xmlFileHandlerLink)
    {
        _fileSystem = fileSystem;
        _xmlFileHandlerLink = xmlFileHandlerLink;
        _logger = logger;
        //make sure that the folder exists
        AssertContentFilesFolderExists();
        _logger.LogInformation("ContentFilesFolderPath is {}", ContentFilesFolderPath);
        AssertAllFilesInContentFilesFolderHaveHash();
    }

    public string ContentFilesFolderPath => Path.Join(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "AdLerAuthoring", "ContentFiles");

    /// <inheritdoc cref="IContentFileHandler.LoadContentAsync(string)"/>
    public async Task<ILearningContentPe> LoadContentAsync(string filepath)
    {
        var (duplicatePath, hash) = await GetFilePathOfExistingCopyAndHashAsync(filepath);
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
        return new FileContentPe(fileName, fileType, finalPath);
    }

    /// <inheritdoc cref="IContentFileHandler.LoadContentAsync(string,System.IO.Stream)"/>
    public async Task<ILearningContentPe> LoadContentAsync(string name, Stream stream)
    {
        var (duplicatePath, hash) = await GetFilePathOfExistingCopyAndHashAsync(stream);
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
        return new FileContentPe(fileName, fileType, finalPath);
    }

    /// <inheritdoc cref="IContentFileHandler.SaveLink"/>
    public void SaveLink(LinkContentPe linkContent)
    {
        var links = GetAllLinks();
        if (links.Contains(linkContent)) return;
        if (links.Any(l => l.Name == linkContent.Name)) linkContent.Name += " (1)";
        links.Add(linkContent);
        OverwriteLinksFile(links);
    }

    /// <inheritdoc cref="IContentFileHandler.GetAllContent"/>
    public IEnumerable<ILearningContentPe> GetAllContent()
    {
        var fileContent = _fileSystem.Directory
            .EnumerateFiles(ContentFilesFolderPath)
            .Select(ParseFileName)
            .Where(FilterContent);
        return fileContent.Union(GetAllLinks());
    }

    /// <inheritdoc cref="IContentFileHandler.RemoveContent"/>
    public void RemoveContent(ILearningContentPe content)
    {
        switch (content)
        {
            case FileContentPe fileContent:
                RemoveFileContent(fileContent);
                break;
            case LinkContentPe linkContent:
                DeleteLink(linkContent);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(content), content, null);
        }
    }

    private void AssertContentFilesFolderExists()
    {
        if (_fileSystem.Directory.Exists(ContentFilesFolderPath)) return;
        _logger.LogDebug("Folder {} did not exist, creating", ContentFilesFolderPath);
        _fileSystem.Directory.CreateDirectory(ContentFilesFolderPath);
    }

    private void DeleteLink(LinkContentPe linkContent)
    {
        var links = GetAllLinks();
        links.Remove(linkContent);
        OverwriteLinksFile(links);
    }

    private List<LinkContentPe> GetAllLinks()
    {
        AssertContentFilesFolderExists();
        var filepath = _fileSystem.Path.Combine(ContentFilesFolderPath, LinkFile);
        //assert file exists
        return _fileSystem.File.Exists(filepath)
            ? _xmlFileHandlerLink.LoadFromDisk(filepath)
            : new List<LinkContentPe>();
    }

    private void OverwriteLinksFile(List<LinkContentPe> links)
    {
        AssertContentFilesFolderExists();
        var filepath = _fileSystem.Path.Combine(ContentFilesFolderPath, LinkFile);
        _xmlFileHandlerLink.SaveToDisk(links, filepath);
    }

    /// <summary>
    /// Filters out content entries that shouldn't be shown to the user.
    /// </summary>
    /// <param name="arg">The content that should be filtered.</param>
    /// <returns>True if it should be shown, false if not.</returns>
    private static bool FilterContent(ILearningContentPe arg)
    {
        if (arg is FileContentPe { Type: "hash" or "ds_store" }) return false;
        return !arg.Name.StartsWith(".");
    }

    /// <summary>
    /// Parses a file at a given path into a <see cref="ILearningContentPe"/> object.
    /// </summary>
    /// <param name="filePath">The path of the file that should be parsed.</param>
    /// <returns>The parsed <see cref="ILearningContentPe"/> object.</returns>
    private ILearningContentPe ParseFileName(string filePath)
    {
        var fileType = _fileSystem.Path.GetExtension(filePath).Trim('.').ToLower();
        var fileName = _fileSystem.Path.GetFileName(filePath);
        return new FileContentPe(fileName, fileType, filePath);
    }

    private void RemoveFileContent(FileContentPe fileContent)
    {
        var files = _fileSystem.Directory.EnumerateFiles(ContentFilesFolderPath).ToList();
        if (!files.Contains(fileContent.Filepath) || !files.Contains($"{fileContent.Filepath}.hash"))
            throw new FileNotFoundException("The corresponding file was not found in the content folder.");
        _fileSystem.File.Delete(fileContent.Filepath);
        _fileSystem.File.Delete($"{fileContent.Filepath}.hash");
    }

    /// <summary>
    /// For a given path and a hash, write the hash to [<paramref name="finalPath"/>].hash
    /// </summary>
    private async void SaveHashForFileAsync(string finalPath, byte[] hash)
    {
        var finalHashPath = finalPath + ".hash";
        await using var writeStream = _fileSystem.File.OpenWrite(finalHashPath);
        await writeStream.WriteAsync(new ReadOnlyMemory<byte>(hash));
        _logger.LogDebug("Wrote hash for {} to {}", finalPath, finalHashPath);
    }

    /// <summary>
    /// Calculates hash of file content and finds path of duplicate files in <see cref="ContentFilesFolderPath"/>.
    /// </summary>
    /// <param name="filepath">The path to the file which should be checked.</param>
    /// <returns>A file path if any duplicate is found, null otherwise, and a byte array containing the hash of the file contents.</returns>
    /// <exception cref="IOException">The file has a length of 0 and is empty.</exception>
    /// <exception cref="ArgumentException">The <paramref name="filepath"/> was null or whitespace.</exception>
    internal async Task<(string?, byte[])> GetFilePathOfExistingCopyAndHashAsync(string filepath)
    {
        if (string.IsNullOrWhiteSpace(filepath))
            throw new ArgumentException("Path cannot be null or whitespace.", nameof(filepath));
        _logger.LogDebug("Opening file at {Filepath} for reading", filepath);
        await using var stream = _fileSystem.File.OpenRead(filepath);
        _logger.LogDebug("Opened file at {Filepath} for reading", filepath);
        try
        {
            return await GetFilePathOfExistingCopyAndHashAsync(stream);
        }
        catch (IOException ioex)
        {
            _logger.LogError("File at path {} is empty", filepath);
            throw new IOException($"File at path {filepath} is empty.", ioex);
        }
    }

    /// <summary>
    /// Calculates hash of stream content and finds path of duplicate files in <see cref="ContentFilesFolderPath"/>.
    /// </summary>
    /// <param name="stream">The stream which should be checked.</param>
    /// <returns>A file path if any duplicate is found, null otherwise, and a byte array containing the hash of the stream contents.</returns>
    /// <exception cref="IOException">The stream has a length of 0 and is empty.</exception>
    private async Task<(string?, byte[])> GetFilePathOfExistingCopyAndHashAsync(Stream stream)
    {
        if (stream.Length == 0)
        {
            _logger.LogError("Stream to check for duplicate was empty");
            throw new IOException("The given stream is empty.");
        }

        AssertAllFilesInContentFilesFolderHaveHash();

        _logger.LogDebug("Calculating hash for stream");
        var streamHash = await ComputeHashAsync(stream);
        _logger.LogDebug("Calculated hash for stream: {Hash}", BitConverter.ToString(streamHash));

        var match = _fileSystem.Directory
            .GetFiles(ContentFilesFolderPath, "*.hash")
            //pre-filter all hash files where length of actual file is not equal to length of stream
            .Where(hashPath => RealFileHasSameLength(stream, hashPath))
            .FirstOrDefault(path => streamHash.SequenceEqual(_fileSystem.File.ReadAllBytes(path)));
        if (match != null)
            _logger.LogInformation("Found matching hash at {Match}", match);
        return match == null
            ? (null, streamHash)
            : (Path.Join(ContentFilesFolderPath, Path.GetFileNameWithoutExtension(match)), streamHash);
    }

    private async void AssertAllFilesInContentFilesFolderHaveHash()
    {
        _logger.LogDebug("Asserting all files in {ContentFilesFolderPath} have a hash file", ContentFilesFolderPath);
        IList<(string realPath, string hashPath)> filesWithoutHash = _fileSystem.Directory
            .GetFiles(ContentFilesFolderPath)
            //macOS specific
            .Where(path => !path.EndsWith(".DS_Store"))
            .Where(path => !path.EndsWith(".hash"))
            .Where(path => !path.Contains(LinkFile))
            .Where(path => !_fileSystem.File.Exists($"{path}.hash"))
            .Select(path => (path, $"{path}.hash"))
            .ToList();
        _logger.LogDebug("Found {Count} applicable files without a hash", filesWithoutHash.Count);
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

    private async Task<byte[]> ComputeHashAsync(Stream stream)
    {
        _logger.LogTrace("Enter ComputeHashAsync");
        _logger.LogTrace("Create SHA256 object");
        using var sha256 = SHA256.Create();
        _logger.LogTrace("Finished creating SHA256 object");
        _logger.LogTrace("Begin seeking stream to position 0");
        if (!stream.CanSeek)
            throw new ArgumentException("Stream must be seekable.", nameof(stream));
        stream.Position = 0;
        _logger.LogTrace("Finished seeking stream to position 0");
        _logger.LogTrace("Begin computing hash");
        return await sha256.ComputeHashAsync(stream);
    }

    private string CopyFileToContentFilesFolder(string filePath)
    {
        var uniqueDestination = FindUniqueDestination(filePath);
        _fileSystem.File.Copy(filePath, uniqueDestination);
        return uniqueDestination;
    }

    private async Task<string> CopyFileToContentFilesFolderAsync(string file, Stream stream)
    {
        var uniqueDestination = FindUniqueDestination(file);
        await using var writeStream = _fileSystem.File.OpenWrite(uniqueDestination);
        stream.Position = 0;
        await stream.CopyToAsync(writeStream);
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