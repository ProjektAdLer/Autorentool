using System.IO.Abstractions;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using Shared;

namespace DataAccess.Persistence;

public class LearningWorldSavePathsHandler : ILearningWorldSavePathsHandler
{
    private readonly ILogger<LearningWorldSavePathsHandler> _logger;
    private readonly IFileSystem _fileSystem;
    private readonly List<SavedLearningWorldPath> _savedLearningWorldPaths;
    private readonly XmlSerializer _serializer;

    private string LearningWorldSavePathsFolderPath => Path.Join(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "AdLerAuthoring", "SavedWorlds");

    private string SavedWorldPathsFilePath => Path.Join(LearningWorldSavePathsFolderPath, "SavedWorlds.xml");

    public LearningWorldSavePathsHandler(ILogger<LearningWorldSavePathsHandler> logger, IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
        _logger = logger;
        _savedLearningWorldPaths = new List<SavedLearningWorldPath>();
        _serializer = new XmlSerializer(typeof(List<SavedLearningWorldPath>));

        AssertLearningWorldSavePathsFolderExists();
        _logger.LogInformation("LearningWorldSavePathsFolderPath is {}", LearningWorldSavePathsFolderPath);

        AssertSavedWorldPathsFileExists();
        _logger.LogInformation("SavedWorldPathsFilePath is {}", SavedWorldPathsFilePath);

        LoadSavedLearningWorldPaths();
    }

    private void AssertLearningWorldSavePathsFolderExists()
    {
        if (_fileSystem.Directory.Exists(LearningWorldSavePathsFolderPath)) return;
        _logger.LogDebug("Folder {} did not exist, creating", LearningWorldSavePathsFolderPath);
        _fileSystem.Directory.CreateDirectory(LearningWorldSavePathsFolderPath);
    }

    private void AssertSavedWorldPathsFileExists()
    {
        if (_fileSystem.File.Exists(SavedWorldPathsFilePath)) return;
        _logger.LogDebug("Saved world paths file does not exist, creating");
        SaveSavedLearningWorldPaths();
    }

    private void SaveSavedLearningWorldPaths()
    {
        using var stream = _fileSystem.File.OpenWrite(SavedWorldPathsFilePath);
        _serializer.Serialize(stream, _savedLearningWorldPaths);
        // removes trailing data after serialization if file was larger before
        stream.SetLength(stream.Position);
    }

    private void LoadSavedLearningWorldPaths()
    {
        using var stream = _fileSystem.File.OpenRead(SavedWorldPathsFilePath);
        if (CanDeserializeStream(stream))
        {
            var savedPaths = (List<SavedLearningWorldPath>) _serializer.Deserialize(stream)!;
            foreach (var loadedPath in _savedLearningWorldPaths.Where(loadedPath => savedPaths.All(x => x.Path != loadedPath.Path && x.Name != loadedPath.Name)))
            {
                _savedLearningWorldPaths.Remove(loadedPath);
            }
            foreach (var savedPath in savedPaths.Where(savedPath => _savedLearningWorldPaths.All(x => x.Path != savedPath.Path && x.Name != savedPath.Name)))
            {
                _savedLearningWorldPaths.Add(savedPath);
            }
        }
        else
        {
            stream.Close();
            SaveSavedLearningWorldPaths();
        }
    }

    private bool CanDeserializeStream(Stream stream)
    {
        try
        {
            using var reader = XmlReader.Create(stream);
            return _serializer.CanDeserialize(reader);
        }
        catch (Exception)
        {
            return false;
        }
        finally
        {
            stream.Position = 0;
        }
    }
    
    /// <inheritdoc cref="ILearningWorldSavePathsHandler.AddSavedLearningWorldPath"/>
    public void AddSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath)
    {
        _savedLearningWorldPaths.RemoveAll(x => x.Path == savedLearningWorldPath.Path);
        _savedLearningWorldPaths.FindAll(x => x.Id == savedLearningWorldPath.Id).ForEach(x => x.Id = Guid.Empty);
        _savedLearningWorldPaths.Add(savedLearningWorldPath);
        SaveSavedLearningWorldPaths();
    }
    
    /// <inheritdoc cref="ILearningWorldSavePathsHandler.AddSavedLearningWorldPathByPathOnly"/>
    public SavedLearningWorldPath AddSavedLearningWorldPathByPathOnly(string path)
    {
        var worldName = Path.GetFileNameWithoutExtension(path);
        var savedLearningWorldPath = new SavedLearningWorldPath
        {
            Name = worldName,
            Path = path
        };
        AddSavedLearningWorldPath(savedLearningWorldPath);
        return savedLearningWorldPath;
    }

    /// <inheritdoc cref="ILearningWorldSavePathsHandler.UpdateIdOfSavedLearningWorldPath"/>
    public void UpdateIdOfSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath, Guid id)
    {
        if (_savedLearningWorldPaths.Contains(savedLearningWorldPath))
        {
            _savedLearningWorldPaths.Find(x => x == savedLearningWorldPath)!.Id = id;
        }
        SaveSavedLearningWorldPaths();
    }
    
    /// <inheritdoc cref="ILearningWorldSavePathsHandler.GetSavedLearningWorldPaths"/>
    public IEnumerable<SavedLearningWorldPath> GetSavedLearningWorldPaths()
    {
        return _savedLearningWorldPaths;
    }
    
    /// <inheritdoc cref="ILearningWorldSavePathsHandler.RemoveSavedLearningWorldPath"/>
    public void RemoveSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath)
    {
        _savedLearningWorldPaths.Remove(savedLearningWorldPath);
        SaveSavedLearningWorldPaths();
    }
}