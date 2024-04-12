using System.IO.Abstractions;
using AutoMapper;
using BusinessLogic.API;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.FileContent;
using BusinessLogic.Entities.LearningContent.LinkContent;
using BusinessLogic.ErrorManagement.DataAccess;
using DataAccess.Extensions;
using DataAccess.Persistence;
using ICSharpCode.SharpZipLib.Zip;
using PersistEntities;
using PersistEntities.LearningContent;
using Shared.Configuration;

namespace DataAccess.API;

public class DataAccess : IDataAccess
{
    internal readonly IContentFileHandler ContentFileHandler;
    internal readonly IFileSystem FileSystem;
    internal readonly ILearningWorldSavePathsHandler WorldSavePathsHandler;
    internal readonly IXmlFileHandler<LearningElementPe> XmlHandlerElement;
    internal readonly IXmlFileHandler<List<LinkContentPe>> XmlHandlerLink;
    internal readonly IXmlFileHandler<LearningSpacePe> XmlHandlerSpace;
    internal readonly IXmlFileHandler<LearningWorldPe> XmlHandlerWorld;


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

    public IEnumerable<IFileInfo> GetSavedLearningWorldPaths()
    {
        return WorldSavePathsHandler.GetSavedLearningWorldPaths();
    }

    /// <inheritdoc cref="IDataAccess.FindSuitableNewSavePath"/>
    public string FindSuitableNewSavePath(string targetFolder, string fileName, string fileEnding, out int iteration)
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
        iteration = 0;
        while (FileSystem.File.Exists($"{savePath}.{fileEnding}"))
        {
            iteration++;
            savePath = $"{baseSavePath}_{iteration}";
        }

        return $"{savePath}.{fileEnding}";
    }

    public string GetContentFilesFolderPath() => ContentFileHandler.ContentFilesFolderPath;

    /// <inheritdoc cref="IDataAccess.ExportLearningWorldToArchiveAsync"/>
    public async Task ExportLearningWorldToArchiveAsync(LearningWorld world, string pathToFile)
    {
        //ensure folders are created
        if (!FileSystem.Directory.Exists(ApplicationPaths.TempFolder))
            FileSystem.Directory.CreateDirectory(ApplicationPaths.TempFolder);
        FileSystem.CreateDisposableDirectory(out var directoryInfo);
        var tempFolder = directoryInfo.FullName;
        //create temp folder structure
        FileSystem.Directory.CreateDirectory(tempFolder);
        var tempContentFolder = FileSystem.Path.Join(tempFolder, "Content");
        FileSystem.Directory.CreateDirectory(tempContentFolder);
        //save world file
        var worldFilePath = FileSystem.Path.Join(tempFolder, "world.awf");
        SaveLearningWorldToFile(world, worldFilePath);
        CopyContentFiles(world, tempContentFolder);
        //zip up temp folder
        await ZipExtensions.CreateFromDirectoryAsync(FileSystem, tempFolder, pathToFile);
    }

    /// <inheritdoc cref="IDataAccess.ImportLearningWorldFromArchiveAsync"/>
    public async Task<LearningWorld> ImportLearningWorldFromArchiveAsync(string pathToArchive)
    {
        //we create a temp folder using IFileSystemExtensions which handles disposing of the folder for us
        using var dispDir = FileSystem.CreateDisposableDirectory(out var dirInfo);
        var tempFolder = dirInfo.FullName;
        FileSystem.Directory.CreateDirectory(Path.Combine(tempFolder, "Content"));
        //unzip archive into temp folder
        using var zipArchive = ZipExtensions.GetZipArchive(FileSystem, pathToArchive);
        zipArchive.ExtractToDirectory(FileSystem, tempFolder);
        //load world file
        var worldFilePath = FileSystem.Path.Join(tempFolder, "world.awf");
        var world = LoadLearningWorld(worldFilePath);
        var contentFolder = await CopyContentAsync(world);

        //add links in linkstore to local linkstore
        var linkListPath = FileSystem.Path.Join(contentFolder, ".linkstore");
        var links = XmlHandlerLink.LoadFromDisk(linkListPath);
        ContentFileHandler.SaveLinks(links);

        //save world to savedworlds folder
        var newSavePath =
            FindSuitableNewSavePath(ApplicationPaths.SavedWorldsFolder, world.Name, "awf", out var iterations);
        
        //we must update the save path in the world entity because it still holds the save path from the archive,
        //which is in turn the save path on the previous machine. this *could* lead to an exception being thrown,
        //if the current user on the new machine is not the exact same username as the user on the previous machine.
        //this problem resolves itself when the application is restarted, as the loaded world contains the correct save path,
        //but for the world that is loaded in the current session, we must update the save path.
        world.SavePath = newSavePath;
        
        //parse save path back into name
        if (iterations != 0) world.Name = $"{world.Name} ({iterations})";

        SaveLearningWorldToFile(world, newSavePath);

        //return world entity
        return world;

        async Task<string> CopyContentAsync(ILearningWorld world)
        {
            EnsureCreated(ApplicationPaths.ContentFolder);
            var fileContents = world.LearningSpaces
                .SelectMany(space => space.ContainedLearningElements.Select(element => element.LearningContent))
                .Concat(world.UnplacedLearningElements.Select(element => element.LearningContent))
                .Where(content => content is FileContent)
                .Cast<FileContent>()
                .ToList();
            //copy content files into content folder (avoiding duplicates) and changing filepaths in world
            var contentFolder = FileSystem.Path.Join(tempFolder, "Content");
            var contentFiles = FileSystem.Directory.GetFiles(contentFolder).Where(filepath =>
                !filepath.EndsWith(".hash") && !filepath.EndsWith(".linkstore"));
            foreach (var contentFile in contentFiles)
            {
                var contentFileHash = await FileSystem.File.ReadAllBytesAsync(contentFile + ".hash");
                var affectedElements = fileContents.Where(content =>
                    FileSystem.Path.GetFileName(content.Name) == FileSystem.Path.GetFileName(contentFile));
                string newFilepath;
                try
                {
                    var newContentPe =
                        (FileContentPe)await ContentFileHandler.LoadContentAsync(contentFile, contentFileHash);
                    newFilepath = newContentPe.Filepath;
                }
                catch (HashExistsException heex)
                {
                    newFilepath = heex.DuplicateFilePath;
                }

                //change filepaths in world
                foreach (var affectedElement in affectedElements)
                {
                    affectedElement.Filepath = newFilepath;
                }
            }

            return contentFolder;
        }
    }

    /// <inheritdoc cref="IDataAccess.GetFileInfoForPath"/>
    public IFileInfo GetFileInfoForPath(string savePath)
    {
        return FileSystem.FileInfo.New(savePath);
    }

    /// <inheritdoc cref="IDataAccess.DeleteFileByPath"/>
    public void DeleteFileByPath(string savePath)
    {
        FileSystem.File.Delete(savePath);
    }

    /// <summary>
    /// For a given world, contains all referenced content files into <paramref name="contentFolder"/>.
    /// </summary>
    /// <param name="world">The world whose content shall be copied.</param>
    /// <param name="contentFolder">The path to the directory the content shall be copied to.</param>
    private void CopyContentFiles(ILearningWorld world, string contentFolder)
    {
        var contentInWorld = world.LearningSpaces
            .SelectMany(space =>
                space.ContainedLearningElements.Select(element => element.LearningContent)
            )
            .Concat(world.UnplacedLearningElements.Select(element => element.LearningContent))
            .ToList();
        var fileContent = contentInWorld.Where(content => content is FileContent).Cast<FileContent>().Distinct();
        var linkContent = contentInWorld.Where(content => content is LinkContent).Cast<LinkContent>().Distinct()
            .ToList();

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

    /// <summary>
    /// Ensures a directory exists at <paramref name="folder"/>.
    /// </summary>
    /// <param name="folder">The folder path.</param>
    private void EnsureCreated(string folder)
    {
        if (!FileSystem.Directory.Exists(folder))
        {
            FileSystem.Directory.CreateDirectory(folder);
        }
    }
}