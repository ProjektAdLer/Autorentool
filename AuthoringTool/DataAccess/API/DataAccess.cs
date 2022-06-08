using System.IO.Abstractions;
using AuthoringTool.API.Configuration;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.Persistence;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.Entities;

namespace AuthoringTool.DataAccess.API;

internal class DataAccess : IDataAccess
{
    public DataAccess(IAuthoringToolConfiguration configuration, IBackupFileGenerator backupFileGenerator,
        IXmlFileHandler<LearningWorld> xmlHandlerWorld, IXmlFileHandler<LearningSpace> xmlHandlerSpace,
        IXmlFileHandler<LearningElement> xmlHandlerElement, IContentFileHandler contentHandler, ICreateDSL createDsl) : this(configuration,
        backupFileGenerator, xmlHandlerWorld, xmlHandlerSpace, xmlHandlerElement, contentHandler, createDsl, new FileSystem()) { }
    public DataAccess(IAuthoringToolConfiguration configuration, IBackupFileGenerator backupFileGenerator,
        IXmlFileHandler<LearningWorld> xmlHandlerWorld, IXmlFileHandler<LearningSpace> xmlHandlerSpace,
        IXmlFileHandler<LearningElement> xmlHandlerElement, IContentFileHandler contentHandler,
        ICreateDSL createDsl, IFileSystem fileSystem)
    {
        XmlHandlerWorld = xmlHandlerWorld;
        XmlHandlerSpace = xmlHandlerSpace;
        XmlHandlerElement = xmlHandlerElement;
        ContentHandler = contentHandler;
        _fileSystem = fileSystem;
        Configuration = configuration;
        BackupFile = backupFileGenerator;
        CreateDsl = createDsl;
    }

    public readonly IXmlFileHandler<LearningWorld> XmlHandlerWorld;
    public readonly IXmlFileHandler<LearningSpace> XmlHandlerSpace;
    public readonly IXmlFileHandler<LearningElement> XmlHandlerElement;
    public readonly IContentFileHandler ContentHandler;
    private readonly IFileSystem _fileSystem;
    public IAuthoringToolConfiguration Configuration { get; }

    public IBackupFileGenerator BackupFile { get; set; }
    public ICreateDSL CreateDsl;

    public void ConstructBackup(LearningWorld learningWorld, string filepath)
    {
        CreateDsl.WriteLearningWorld(learningWorld);
        BackupFile.CreateBackupFolders();
        BackupFile.WriteXmlFiles();
        BackupFile.WriteBackupFile(filepath);
    }

    public void SaveLearningWorldToFile(LearningWorld world, string filepath)
    {
        XmlHandlerWorld.SaveToDisk(world, filepath);
    }

    public LearningWorld LoadLearningWorldFromFile(string filepath)
    {
        return XmlHandlerWorld.LoadFromDisk(filepath);
    }

    public LearningWorld LoadLearningWorldFromStream(Stream stream)
    {
        return XmlHandlerWorld.LoadFromStream(stream);
    }

    public void SaveLearningSpaceToFile(LearningSpace space, string filepath)
    {
        XmlHandlerSpace.SaveToDisk(space, filepath);
    }

    public LearningSpace LoadLearningSpaceFromFile(string filepath)
    {
        return XmlHandlerSpace.LoadFromDisk(filepath);
    }

    public LearningSpace LoadLearningSpaceFromStream(Stream stream)
    {
        return XmlHandlerSpace.LoadFromStream(stream);
    }

    public void SaveLearningElementToFile(LearningElement element, string filepath)
    {
        XmlHandlerElement.SaveToDisk(element, filepath);
    }

    public LearningElement LoadLearningElementFromFile(string filepath)
    {
        return XmlHandlerElement.LoadFromDisk(filepath);
    }

    public LearningElement LoadLearningElementFromStream(Stream stream)
    {
        return XmlHandlerElement.LoadFromStream(stream);
    }

    public LearningContent LoadLearningContentFromFile(string filepath)
    {
        return ContentHandler.LoadFromDisk(filepath);
    }

    /// <inheritdoc cref="IDataAccess.FindSuitableNewSavePath"/>
    public string FindSuitableNewSavePath(string targetFolder, string fileName, string fileEnding)
    {
        if (string.IsNullOrWhiteSpace(targetFolder))
        {
            throw new ArgumentException("targetFolder cannot be empty", nameof(targetFolder));
        }

        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("fileName cannot be empty", nameof(fileName));
        }

        if (string.IsNullOrEmpty(fileEnding))
        {
            throw new ArgumentException("fileEnding cannot be empty", nameof(fileEnding));
        }
        
        var baseSavePath = _fileSystem.Path.Combine(targetFolder, fileName);
        var savePath = baseSavePath;
        var iteration = 0;
        while (_fileSystem.File.Exists($"{savePath}.{fileEnding}"))
        {
            iteration++;
            savePath = $"{baseSavePath}_{iteration}";
        }

        return $"{savePath}.{fileEnding}";
    }
}