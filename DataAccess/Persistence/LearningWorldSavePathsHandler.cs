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
    private List<SavedLearningWorldPath> _savedLearningWorldPaths;
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
            _savedLearningWorldPaths = (List<SavedLearningWorldPath>) _serializer.Deserialize(stream)!;
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
        _savedLearningWorldPaths.Add(savedLearningWorldPath);
        SaveSavedLearningWorldPaths();
    }
    
    /// <inheritdoc cref="ILearningWorldSavePathsHandler.AddSavedLearningWorldPathByPathOnly"/>
    public SavedLearningWorldPath AddSavedLearningWorldPathByPathOnly(string path)
    {
        var id = Guid.NewGuid();
        var worldName = Path.GetFileNameWithoutExtension(path);
        var savedLearningWorldPath = new SavedLearningWorldPath
        {
            Id = id,
            Name = worldName,
            Path = path
        };
        AddSavedLearningWorldPath(savedLearningWorldPath);
        return savedLearningWorldPath;
    }

    /// <inheritdoc cref="ILearningWorldSavePathsHandler.UpdateIdOfSavedLearningWorldPath"/>
    public void UpdateIdOfSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath, Guid id)
    {
        _savedLearningWorldPaths.RemoveAll(x=> x.Id == savedLearningWorldPath.Id);
        savedLearningWorldPath.Id = id;
        _savedLearningWorldPaths.Add(savedLearningWorldPath);
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
        _savedLearningWorldPaths.RemoveAll(x => x.Id == savedLearningWorldPath.Id);
        SaveSavedLearningWorldPaths();
    }
}