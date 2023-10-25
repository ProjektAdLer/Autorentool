﻿using System.IO.Abstractions;
using AutoMapper;
using BusinessLogic.API;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.FileContent;
using BusinessLogic.Entities.LearningContent.LinkContent;
using DataAccess.Persistence;
using PersistEntities;
using PersistEntities.LearningContent;
using Shared;
using Shared.Configuration;

namespace DataAccess.API;

public class DataAccess : IDataAccess
{
    internal readonly IContentFileHandler ContentFileHandler;
    internal readonly IFileSystem FileSystem;
    internal readonly ILearningWorldSavePathsHandler WorldSavePathsHandler;
    internal readonly IXmlFileHandler<LearningElementPe> XmlHandlerElement;
    internal readonly IXmlFileHandler<LearningSpacePe> XmlHandlerSpace;
    internal readonly IXmlFileHandler<LearningWorldPe> XmlHandlerWorld;
    internal readonly IXmlFileHandler<List<LinkContentPe>> XmlHandlerLink;


    public DataAccess(IApplicationConfiguration configuration, IXmlFileHandler<LearningWorldPe> xmlHandlerWorld,
        IXmlFileHandler<LearningSpacePe> xmlHandlerSpace, IXmlFileHandler<LearningElementPe> xmlHandlerElement,
        IXmlFileHandler<List<LinkContentPe>> xmlHandlerLink,
        IContentFileHandler contentFileHandler, ILearningWorldSavePathsHandler worldSavePathsHandler,
        IFileSystem fileSystem, IMapper mapper)
    {
        XmlHandlerWorld = xmlHandlerWorld;
        XmlHandlerSpace = xmlHandlerSpace;
        XmlHandlerElement = xmlHandlerElement;
        ContentFileHandler = contentFileHandler;
        WorldSavePathsHandler = worldSavePathsHandler;
        FileSystem = fileSystem;
        Configuration = configuration;
        Mapper = mapper;
        XmlHandlerLink = xmlHandlerLink;
    }

    public IMapper Mapper { get; }
    public IApplicationConfiguration Configuration { get; }


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

    /// <inheritdoc cref="IDataAccess.LoadLearningContentAsync(string)"/>
    public async Task<ILearningContent> LoadLearningContentAsync(string filepath)
    {
        return Mapper.Map<ILearningContent>(await ContentFileHandler.LoadContentAsync(filepath));
    }

    /// <inheritdoc cref="IDataAccess.LoadLearningContentAsync(string,Stream)"/>
    public async Task<ILearningContent> LoadLearningContentAsync(string name, Stream stream)
    {
        return Mapper.Map<ILearningContent>(await ContentFileHandler.LoadContentAsync(name, stream));
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

    /// <inheritdoc cref="IDataAccess.ExportLearningWorldToArchive"/>
    public void ExportLearningWorldToArchive(LearningWorld world, string pathToFolder)
    {
        //ensure folders are created
        if (!FileSystem.Directory.Exists(ApplicationPaths.TempFolder))
            FileSystem.Directory.CreateDirectory(ApplicationPaths.TempFolder);
        var tempFolder = FileSystem.Path.Join(ApplicationPaths.TempFolder, Guid.NewGuid().ToString());
        try
        {
            //create temp folder structure
            FileSystem.Directory.CreateDirectory(tempFolder);
            var tempContentFolder = FileSystem.Path.Join(tempFolder, "Content");
            FileSystem.Directory.CreateDirectory(tempContentFolder);
            //save world file
            var worldFilePath = FileSystem.Path.Join(tempFolder, "world.awf");
            SaveLearningWorldToFile(world, worldFilePath);
            CopyContentFiles(world, tempContentFolder);
        }
        finally
        {
            FileSystem.Directory.Delete(tempFolder, true);
        }
    }

    private void CopyContentFiles(LearningWorld world, string contentFolder)
    {
        var contentInWorld = world.LearningSpaces
            .SelectMany(space =>
                space.ContainedLearningElements.Select(element => element.LearningContent)
            )
            .ToList();
        var fileContent = contentInWorld.Where(content => content is FileContent).Cast<FileContent>();
        var linkContent = contentInWorld.Where(content => content is LinkContent).Cast<LinkContent>().ToList();
        
        //copy files
        foreach (var file in fileContent)
        {
            //copy file
            var sourceFileName = FileSystem.Path.GetFileName(file.Filepath);
            var targetFilePath = FileSystem.Path.Join(contentFolder, sourceFileName);
            FileSystem.File.Copy(file.Filepath, targetFilePath);
            //copy it's hash
            var sourceHashFileName = sourceFileName + ".hash";
            var targetHashFilePath = FileSystem.Path.Join(contentFolder, sourceHashFileName);
            FileSystem.File.Copy(file.Filepath + ".hash", targetHashFilePath);
        }

        //save links
        var linkContentFilepath = FileSystem.Path.Join(contentFolder, ".linkstore");
        var linkContentPe = Mapper.Map<List<LinkContentPe>>(linkContent);
        XmlHandlerLink.SaveToDisk(linkContentPe, linkContentFilepath);
    }

    /// <inheritdoc cref="IDataAccess.ImportLearningWorldFromArchive"/>
    public void ImportLearningWorldFromArchive(string pathToArchive)
    {
        throw new NotImplementedException();
    }
}