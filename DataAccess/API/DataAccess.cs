using System.IO.Abstractions;
using AutoMapper;
using BusinessLogic.API;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using DataAccess.Persistence;
using PersistEntities;
using PersistEntities.LearningContent;
using Shared;
using Shared.Configuration;

namespace DataAccess.API;

public class DataAccess : IDataAccess
{
    public DataAccess(IAuthoringToolConfiguration configuration, IXmlFileHandler<LearningWorldPe> xmlHandlerWorld, 
        IXmlFileHandler<LearningSpacePe> xmlHandlerSpace, IXmlFileHandler<LearningElementPe> xmlHandlerElement, 
        IContentFileHandler contentFileHandler, ILearningWorldSavePathsHandler worldSavePathsHandler, IFileSystem fileSystem, IMapper mapper)
    {
        XmlHandlerWorld = xmlHandlerWorld;
        XmlHandlerSpace = xmlHandlerSpace;
        XmlHandlerElement = xmlHandlerElement;
        ContentFileHandler = contentFileHandler;
        WorldSavePathsHandler = worldSavePathsHandler;
        FileSystem = fileSystem;
        Configuration = configuration;
        Mapper = mapper;
    }

    public readonly IXmlFileHandler<LearningWorldPe> XmlHandlerWorld;
    public readonly IXmlFileHandler<LearningSpacePe> XmlHandlerSpace;
    public readonly IXmlFileHandler<LearningElementPe> XmlHandlerElement;
    public readonly IContentFileHandler ContentFileHandler;
    public readonly ILearningWorldSavePathsHandler WorldSavePathsHandler;
    public readonly IFileSystem FileSystem;
    public IAuthoringToolConfiguration Configuration { get; }
    public IMapper Mapper { get; }
    


    public void SaveLearningWorldToFile(LearningWorld world, string filepath)
    {
        XmlHandlerWorld.SaveToDisk(Mapper.Map<LearningWorldPe>(world), filepath);
    }

    public LearningWorld LoadLearningWorld(string filepath)
    {
        return Mapper.Map<LearningWorld>(XmlHandlerWorld.LoadFromDisk(filepath));
    }

    public LearningWorld LoadLearningWorld(Stream stream)
    {
        return Mapper.Map<LearningWorld>(XmlHandlerWorld.LoadFromStream(stream));
    }

    public void SaveLearningSpaceToFile(LearningSpace space, string filepath)
    {
        XmlHandlerSpace.SaveToDisk(Mapper.Map<LearningSpacePe>(space), filepath);
    }

    public LearningSpace LoadLearningSpace(string filepath)
    {
        return Mapper.Map<LearningSpace>(XmlHandlerSpace.LoadFromDisk(filepath));
    }

    public LearningSpace LoadLearningSpace(Stream stream)
    {
        return Mapper.Map<LearningSpace>(XmlHandlerSpace.LoadFromStream(stream));
    }

    public void SaveLearningElementToFile(LearningElement element, string filepath)
    {
        XmlHandlerElement.SaveToDisk(Mapper.Map<LearningElementPe>(element), filepath);
    }

    public LearningElement LoadLearningElement(string filepath)
    {
        return Mapper.Map<LearningElement>(XmlHandlerElement.LoadFromDisk(filepath));
    }

    public LearningElement LoadLearningElement(Stream stream)
    {
        return Mapper.Map<LearningElement>(XmlHandlerElement.LoadFromStream(stream));
    }

    public ILearningContent LoadLearningContent(string filepath)
    {
        return Mapper.Map<ILearningContent>(ContentFileHandler.LoadContentAsync(filepath).Result);
    }

    public ILearningContent LoadLearningContent(string name, Stream stream)
    {
        return Mapper.Map<ILearningContent>(ContentFileHandler.LoadContentAsync(name, stream).Result);
    }
    
    /// <inheritdoc cref="IDataAccess.GetAllContent"/>
    public IEnumerable<ILearningContent> GetAllContent()
    {
        return ContentFileHandler.GetAllContent().Select(Mapper.Map<ILearningContent>);
    }

    /// <inheritdoc cref="IDataAccess.RemoveContent"/>
    public void RemoveContent(ILearningContent content) =>
        ContentFileHandler.RemoveContent(Mapper.Map<ILearningContentPe>(content));

    /// <inheritdoc cref="IDataAccess.SaveLink"/>
    public void SaveLink(LinkContent linkContent) =>
        ContentFileHandler.SaveLink(Mapper.Map<LinkContentPe>(linkContent));

    public IEnumerable<SavedLearningWorldPath> GetSavedLearningWorldPaths()
    {
        return WorldSavePathsHandler.GetSavedLearningWorldPaths();
    }

    public void AddSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath)
    {
        WorldSavePathsHandler.AddSavedLearningWorldPath(savedLearningWorldPath);
    }

    public SavedLearningWorldPath AddSavedLearningWorldPathByPathOnly(string path)
    {
        return WorldSavePathsHandler.AddSavedLearningWorldPathByPathOnly(path);
    }
    
    public void UpdateIdOfSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath, Guid id)
    {
        WorldSavePathsHandler.UpdateIdOfSavedLearningWorldPath(savedLearningWorldPath, id);
    }

    public void RemoveSavedLearningWorldPath(SavedLearningWorldPath savedLearningWorldPath)
    {
        WorldSavePathsHandler.RemoveSavedLearningWorldPath(savedLearningWorldPath);
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

        var baseSavePath = FileSystem.Path.Combine(targetFolder, fileName);
        var savePath = baseSavePath;
        var iteration = 0;
        while (FileSystem.File.Exists($"{savePath}.{fileEnding}"))
        {
            iteration++;
            savePath = $"{baseSavePath}_{iteration}";
        }

        return $"{savePath}.{fileEnding}";
    }

    public string GetContentFilesFolderPath() => ContentFileHandler.ContentFilesFolderPath;
}