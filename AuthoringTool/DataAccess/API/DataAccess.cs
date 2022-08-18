using System.IO.Abstractions;
using AuthoringTool.API.Configuration;
using AuthoringTool.DataAccess.Persistence;
using AuthoringTool.DataAccess.PersistEntities;

namespace AuthoringTool.DataAccess.API;

internal class DataAccess : IDataAccess
{
    public DataAccess(IAuthoringToolConfiguration configuration, IXmlFileHandler<LearningWorldPe> xmlHandlerWorld, 
        IXmlFileHandler<LearningSpacePe> xmlHandlerSpace, IXmlFileHandler<LearningElementPe> xmlHandlerElement, 
        IContentFileHandler contentHandler, IFileSystem fileSystem)
    {
        XmlHandlerWorld = xmlHandlerWorld;
        XmlHandlerSpace = xmlHandlerSpace;
        XmlHandlerElement = xmlHandlerElement;
        ContentHandler = contentHandler;
        _fileSystem = fileSystem;
        Configuration = configuration;
    }

    public readonly IXmlFileHandler<LearningWorldPe> XmlHandlerWorld;
    public readonly IXmlFileHandler<LearningSpacePe> XmlHandlerSpace;
    public readonly IXmlFileHandler<LearningElementPe> XmlHandlerElement;
    public readonly IContentFileHandler ContentHandler;
    private readonly IFileSystem _fileSystem;
    public IAuthoringToolConfiguration Configuration { get; }
    


    public void SaveLearningWorldToFile(LearningWorldPe world, string filepath)
    {
        XmlHandlerWorld.SaveToDisk(world, filepath);
    }

    public LearningWorldPe LoadLearningWorldFromFile(string filepath)
    {
        return XmlHandlerWorld.LoadFromDisk(filepath);
    }

    public LearningWorldPe LoadLearningWorldFromStream(Stream stream)
    {
        return XmlHandlerWorld.LoadFromStream(stream);
    }

    public void SaveLearningSpaceToFile(LearningSpacePe space, string filepath)
    {
        XmlHandlerSpace.SaveToDisk(space, filepath);
    }

    public LearningSpacePe LoadLearningSpaceFromFile(string filepath)
    {
        return XmlHandlerSpace.LoadFromDisk(filepath);
    }

    public LearningSpacePe LoadLearningSpaceFromStream(Stream stream)
    {
        return XmlHandlerSpace.LoadFromStream(stream);
    }

    public void SaveLearningElementToFile(LearningElementPe element, string filepath)
    {
        XmlHandlerElement.SaveToDisk(element, filepath);
    }

    public LearningElementPe LoadLearningElementFromFile(string filepath)
    {
        return XmlHandlerElement.LoadFromDisk(filepath);
    }

    public LearningElementPe LoadLearningElementFromStream(Stream stream)
    {
        return XmlHandlerElement.LoadFromStream(stream);
    }

    public LearningContentPe LoadLearningContentFromFile(string filepath)
    {
        return ContentHandler.LoadFromDisk(filepath);
    }
    
    public LearningContentPe LoadLearningContentFromStream(string name, Stream stream)
    {
        return ContentHandler.LoadFromStream(name, stream);
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