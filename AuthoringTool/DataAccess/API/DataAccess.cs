using System.IO.Abstractions;
using AuthoringTool.API.Configuration;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.Persistence;
using AuthoringTool.DataAccess.PersistEntities;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.Entities;
using AutoMapper;

namespace AuthoringTool.DataAccess.API;

internal class DataAccess : IDataAccess
{
    public DataAccess(IAuthoringToolConfiguration configuration, IBackupFileGenerator backupFileGenerator,
        IXmlFileHandler<LearningWorldPe> xmlHandlerWorld, IXmlFileHandler<LearningSpacePe> xmlHandlerSpace,
        IXmlFileHandler<LearningElementPe> xmlHandlerElement, IContentFileHandler contentHandler,
        ICreateDSL createDsl, IReadDSL readDsl, IFileSystem fileSystem, IMapper autoMapper)
    {
        XmlHandlerWorld = xmlHandlerWorld;
        XmlHandlerSpace = xmlHandlerSpace;
        XmlHandlerElement = xmlHandlerElement;
        ContentHandler = contentHandler;
        _fileSystem = fileSystem;
        Configuration = configuration;
        BackupFile = backupFileGenerator;
        CreateDsl = createDsl;
        ReadDsl = readDsl;
        _autoMapper = autoMapper;
    }

    public readonly IXmlFileHandler<LearningWorldPe> XmlHandlerWorld;
    public readonly IXmlFileHandler<LearningSpacePe> XmlHandlerSpace;
    public readonly IXmlFileHandler<LearningElementPe> XmlHandlerElement;
    public readonly IContentFileHandler ContentHandler;
    private readonly IFileSystem _fileSystem;
    public IAuthoringToolConfiguration Configuration { get; }

    public IBackupFileGenerator BackupFile { get; set; }
    public ICreateDSL CreateDsl;
    public IReadDSL ReadDsl;
    private IMapper _autoMapper;

    /// <summary>
    /// Creates the DSL document, reads it, creates the needed folder structure for the backup, fills the folders with
    /// the needed xml documents and saves it to the desired location as .mbz file. 
    /// </summary>
    /// <param name="learningWorld"></param> Information about the learningWorld, topics, spaces and elements
    /// <param name="filepath"></param> Desired filepath for the .mbz file. Given by user, when Export Button is pressed.
    public void ConstructBackup(LearningWorld learningWorld, string filepath)
    {
        string dslpath = CreateDsl.WriteLearningWorld(_autoMapper.Map<LearningWorldPe>(learningWorld));
        ReadDsl.ReadLearningWorld(dslpath);
        BackupFile.CreateBackupFolders();
        BackupFile.WriteXmlFiles(ReadDsl as ReadDSL, dslpath);
        BackupFile.WriteBackupFile(filepath);
    }

    public void SaveLearningWorldToFile(LearningWorld world, string filepath)
    {
        XmlHandlerWorld.SaveToDisk(_autoMapper.Map<LearningWorldPe>(world), filepath);
    }

    public LearningWorld LoadLearningWorldFromFile(string filepath)
    {
        return _autoMapper.Map<LearningWorld>(XmlHandlerWorld.LoadFromDisk(filepath));
    }

    public LearningWorld LoadLearningWorldFromStream(Stream stream)
    {
        return _autoMapper.Map<LearningWorld>(XmlHandlerWorld.LoadFromStream(stream));
    }

    public void SaveLearningSpaceToFile(LearningSpace space, string filepath)
    {
        XmlHandlerSpace.SaveToDisk(_autoMapper.Map<LearningSpacePe>(space), filepath);
    }

    public LearningSpace LoadLearningSpaceFromFile(string filepath)
    {
        return _autoMapper.Map<LearningSpace>(XmlHandlerSpace.LoadFromDisk(filepath));
    }

    public LearningSpace LoadLearningSpaceFromStream(Stream stream)
    {
        return _autoMapper.Map<LearningSpace>(XmlHandlerSpace.LoadFromStream(stream));
    }

    public void SaveLearningElementToFile(LearningElement element, string filepath)
    {
        XmlHandlerElement.SaveToDisk(_autoMapper.Map<LearningElementPe>(element), filepath);
    }

    public LearningElement LoadLearningElementFromFile(string filepath)
    {
        return _autoMapper.Map<LearningElement>(XmlHandlerElement.LoadFromDisk(filepath));
    }

    public LearningElement LoadLearningElementFromStream(Stream stream)
    {
        return _autoMapper.Map<LearningElement>(XmlHandlerElement.LoadFromStream(stream));
    }

    public LearningContent LoadLearningContentFromFile(string filepath)
    {
        return _autoMapper.Map<LearningContent>(ContentHandler.LoadFromDisk(filepath));
    }
    
    public LearningContent LoadLearningContentFromStream(string name, Stream stream)
    {
        return _autoMapper.Map<LearningContent>(ContentHandler.LoadFromStream(name, stream));
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