using System.IO.Abstractions;
using AutoMapper;
using BusinessLogic.API;
using BusinessLogic.Entities;
using DataAccess.Persistence;
using PersistEntities;
using Shared.Configuration;

namespace DataAccess.API;

public class DataAccess : IDataAccess
{
    public DataAccess(IAuthoringToolConfiguration configuration, IXmlFileHandler<WorldPe> xmlHandlerWorld, 
        IXmlFileHandler<SpacePe> xmlHandlerSpace, IXmlFileHandler<ElementPe> xmlHandlerElement, 
        IContentFileHandler xmlHandlerContent, IFileSystem fileSystem, IMapper mapper)
    {
        XmlHandlerWorld = xmlHandlerWorld;
        XmlHandlerSpace = xmlHandlerSpace;
        XmlHandlerElement = xmlHandlerElement;
        XmlHandlerContent = xmlHandlerContent;
        FileSystem = fileSystem;
        Configuration = configuration;
        Mapper = mapper;
    }

    public readonly IXmlFileHandler<WorldPe> XmlHandlerWorld;
    public readonly IXmlFileHandler<SpacePe> XmlHandlerSpace;
    public readonly IXmlFileHandler<ElementPe> XmlHandlerElement;
    public readonly IContentFileHandler XmlHandlerContent;
    public readonly IFileSystem FileSystem;
    public IAuthoringToolConfiguration Configuration { get; }
    public IMapper Mapper { get; }
    


    public void SaveWorldToFile(World world, string filepath)
    {
        XmlHandlerWorld.SaveToDisk(Mapper.Map<WorldPe>(world), filepath);
    }

    public World LoadWorld(string filepath)
    {
        return Mapper.Map<World>(XmlHandlerWorld.LoadFromDisk(filepath));
    }

    public World LoadWorld(Stream stream)
    {
        return Mapper.Map<World>(XmlHandlerWorld.LoadFromStream(stream));
    }

    public void SaveSpaceToFile(Space space, string filepath)
    {
        XmlHandlerSpace.SaveToDisk(Mapper.Map<SpacePe>(space), filepath);
    }

    public Space LoadSpace(string filepath)
    {
        return Mapper.Map<Space>(XmlHandlerSpace.LoadFromDisk(filepath));
    }

    public Space LoadSpace(Stream stream)
    {
        return Mapper.Map<Space>(XmlHandlerSpace.LoadFromStream(stream));
    }

    public void SaveElementToFile(Element element, string filepath)
    {
        XmlHandlerElement.SaveToDisk(Mapper.Map<ElementPe>(element), filepath);
    }

    public Element LoadElement(string filepath)
    {
        return Mapper.Map<Element>(XmlHandlerElement.LoadFromDisk(filepath));
    }

    public Element LoadElement(Stream stream)
    {
        return Mapper.Map<Element>(XmlHandlerElement.LoadFromStream(stream));
    }

    public Content LoadContent(string filepath)
    {
        return Mapper.Map<Content>(XmlHandlerContent.LoadContentAsync(filepath).Result);
    }

    public Content LoadContent(string name, MemoryStream stream)
    {
        return Mapper.Map<Content>(XmlHandlerContent.LoadContentAsync(name, stream).Result);
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
}