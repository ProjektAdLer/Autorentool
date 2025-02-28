﻿using System.Globalization;
using System.IO.Abstractions;
using Generator.ATF;
using Generator.XmlClasses.Entities._activities.GradeHistory.xml;
using Generator.XmlClasses.Entities._activities.Grades.xml;
using Generator.XmlClasses.Entities._activities.Inforef.xml;
using Generator.XmlClasses.Entities._activities.Module.xml;
using Generator.XmlClasses.Entities._activities.Resource.xml;
using Generator.XmlClasses.Entities._activities.Roles.xml;
using Generator.XmlClasses.Entities.Files.xml;
using Shared.Configuration;

namespace Generator.XmlClasses.XmlFileFactories;

public class XmlResourceFactory : IXmlResourceFactory
{
    private readonly IFileSystem _fileSystem;
    private readonly string _hardcodedPath = "XMLFilesForExport";
    private readonly string _workDir;
    public readonly string CurrentTime;

    public readonly IXmlFileManager FileManager;
    private List<ActivitiesInforefXmlFile> _activitiesInforefXmlFileList;
    public string FileElementDesc;
    public string FileElementId;
    public string FileElementName;
    public string FileElementParentSpaceString;
    public float FileElementPoints;
    public string FileElementType;
    public string FileElementUuid;
    public List<FilesXmlFile> FilesXmlFilesList;

    public XmlResourceFactory(IReadAtf readAtf, IXmlFileManager? xmlFileManager = null,
        IFileSystem? fileSystem = null, IActivitiesGradesXmlGradeItem? gradesGradeItem = null,
        IActivitiesGradesXmlGradeItems? gradesGradeItems = null,
        IActivitiesGradesXmlActivityGradebook? gradebook = null,
        IActivitiesResourceXmlResource? fileResourceXmlResource = null,
        IActivitiesResourceXmlActivity? fileResourceXmlActivity = null,
        IActivitiesRolesXmlRoles? roles = null, IActivitiesModuleXmlModule? module = null,
        IActivitiesGradeHistoryXmlGradeHistory? gradeHistory = null, IActivitiesInforefXmlFile? inforefXmlFile = null,
        IActivitiesInforefXmlFileref? inforefXmlFileref = null,
        IActivitiesInforefXmlGradeItem? inforefXmlGradeItem = null,
        IActivitiesInforefXmlInforef? inforefXmlInforef = null)
    {
        ReadAtf = readAtf;
        FileElementId = "";
        FileElementUuid = "";
        FileElementName = "";
        FileElementParentSpaceString = "";
        FileElementType = "";
        FileElementDesc = "";
        FileElementPoints = 0;

        CurrentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        _fileSystem = fileSystem ?? new FileSystem();
        _workDir = ApplicationPaths.BackupFolder;

        FileManager = xmlFileManager ?? new XmlFileManager();
        FilesXmlFilesList = new List<FilesXmlFile>();
        _activitiesInforefXmlFileList = new List<ActivitiesInforefXmlFile>();

        ActivitiesGradesXmlGradeItem = gradesGradeItem ?? new ActivitiesGradesXmlGradeItem();
        ActivitiesGradesXmlGradeItems = gradesGradeItems ?? new ActivitiesGradesXmlGradeItems();
        ActivitiesGradesXmlActivityGradebook = gradebook ?? new ActivitiesGradesXmlActivityGradebook();

        ActivitiesFileResourceXmlResource = fileResourceXmlResource ?? new ActivitiesResourceXmlResource();
        ActivitiesFileResourceXmlActivity = fileResourceXmlActivity ?? new ActivitiesResourceXmlActivity();

        ActivitiesRolesXmlRoles = roles ?? new ActivitiesRolesXmlRoles();

        ActivitiesModuleXmlModule = module ?? new ActivitiesModuleXmlModule();

        ActivitiesGradeHistoryXmlGradeHistory = gradeHistory ?? new ActivitiesGradeHistoryXmlGradeHistory();

        ActivitiesInforefXmlFileBlock1 = inforefXmlFile ?? new ActivitiesInforefXmlFile();
        ActivitiesInforefXmlFileBlock2 = inforefXmlFile ?? new ActivitiesInforefXmlFile();
        ActivitiesInforefXmlFileref = inforefXmlFileref ?? new ActivitiesInforefXmlFileref();
        ActivitiesInforefXmlGradeItem = inforefXmlGradeItem ?? new ActivitiesInforefXmlGradeItem();
        ActivitiesInforefXmlInforef = inforefXmlInforef ?? new ActivitiesInforefXmlInforef();
    }

    public IActivitiesGradesXmlGradeItem ActivitiesGradesXmlGradeItem { get; }
    public IActivitiesGradesXmlGradeItems ActivitiesGradesXmlGradeItems { get; }
    public IActivitiesGradesXmlActivityGradebook ActivitiesGradesXmlActivityGradebook { get; }
    public IActivitiesResourceXmlResource ActivitiesFileResourceXmlResource { get; }
    public IActivitiesResourceXmlActivity ActivitiesFileResourceXmlActivity { get; }
    public IActivitiesRolesXmlRoles ActivitiesRolesXmlRoles { get; }
    public IActivitiesModuleXmlModule ActivitiesModuleXmlModule { get; }
    public IActivitiesGradeHistoryXmlGradeHistory ActivitiesGradeHistoryXmlGradeHistory { get; }
    public IActivitiesInforefXmlFile ActivitiesInforefXmlFileBlock1 { get; }
    public IActivitiesInforefXmlFile ActivitiesInforefXmlFileBlock2 { get; }
    public IActivitiesInforefXmlFileref ActivitiesInforefXmlFileref { get; }
    public IActivitiesInforefXmlGradeItem ActivitiesInforefXmlGradeItem { get; }
    public IActivitiesInforefXmlInforef ActivitiesInforefXmlInforef { get; }
    public IReadAtf ReadAtf { get; }

    /// <inheritdoc cref="IXmlResourceFactory.CreateResourceFactory"/>
    public void CreateResourceFactory()
    {
        var resourceList = ReadAtf.GetResourceElementList();
        FilesXmlFilesList = FileManager.GetXmlFilesList();
        ReadFileListAndSetParametersResource(resourceList);

        FileManager.SetXmlFilesList(FilesXmlFilesList);
    }

    /// <summary>
    /// Reads the file list and sets parameters for each resource in the list.
    /// </summary>
    private void ReadFileListAndSetParametersResource(List<ILearningElementJson> resourceList)
    {
        foreach (var resource in resourceList)
        {
            FileElementId = resource.ElementId.ToString();
            FileElementUuid = resource.ElementUUID;
            FileElementType = resource.ElementFileType;
            FileElementName = resource.ElementName;

            switch (resource)
            {
                case BaseLearningElementJson:
                    FileElementParentSpaceString = (ReadAtf.GetSpaceList().Count + 1).ToString();
                    break;
                case LearningElementJson learningElementJson:
                    FileElementDesc = learningElementJson.ElementDescription ?? "";
                    FileElementPoints = learningElementJson.ElementMaxScore;
                    FileElementParentSpaceString = learningElementJson.LearningSpaceParentId.ToString();
                    break;
            }

            FileManager.CalculateHashCheckSumAndFileSize(_fileSystem.Path.Join(_workDir, _hardcodedPath,
                resource.ElementName + "." + resource.ElementFileType));
            FileManager.CreateFolderAndFiles(_fileSystem.Path.Join(_workDir, _hardcodedPath,
                resource.ElementName + "." + resource.ElementFileType), FileManager.GetHashCheckSum());

            var mimeType = resource.ElementFileType switch
            {
                "pdf" or "json" =>
                    "application/" + FileElementType,
                "js" =>
                    "application/x-javascript",
                "cs" =>
                    "application/x-csh",
                "jpg" or "jpeg" or "png" or "bmp" =>
                    "image/" + FileElementType,
                "webp" or "cc" or "c++" =>
                    "document/unknown",
                "txt" or "c" or "h" or "py" or "cpp" or "php" =>
                    "text/plain",
                "css" or "html" =>
                    "text/" + FileElementType,
                _ => null
            };
            if (mimeType != null)
                ResourceSetParametersFilesXml(FileManager.GetHashCheckSum(), FileManager.GetFileSize(), mimeType,
                    FileElementUuid);

            FileSetParametersActivity();

            // These ints are needed for the activities/inforef.xml file. 
            XmlEntityManager.IncreaseFileId();
        }
    }

    //Every resource has to be put into files.xml file
    public void ResourceSetParametersFilesXml(string hashCheckSum, string filesize, string mimeType, string uuid)
    {
        var file1 = new FilesXmlFile
        {
            Id = XmlEntityManager.GetFileIdBlock1().ToString(),
            ContentHash = hashCheckSum,
            ContextId = FileElementId,
            Filename = FileElementName + "." + FileElementType,
            Mimetype = mimeType,
            Source = FileElementName + "." + FileElementType,
            Filesize = filesize,
            Timecreated = CurrentTime,
            Timemodified = CurrentTime,
            ElementUuid = uuid
        };
        var file2 = (FilesXmlFile)file1.Clone();
        file2.Id = XmlEntityManager.GetFileIdBlock2().ToString();

        FilesXmlFilesList.Add(file1);
        FilesXmlFilesList.Add(file2);
    }

    /// <summary>
    /// Sets the parameters for a file activity including its related XML files.
    /// </summary>
    public void FileSetParametersActivity()
    {
        CreateActivityFolder(FileElementId);

        //file activities/resource.../grades.xml
        ActivitiesGradesXmlActivityGradebook.Serialize("resource", FileElementId);

        //file activities/resource.../resource.xml
        ActivitiesFileResourceXmlResource.Name = FileElementName;
        ActivitiesFileResourceXmlResource.Timemodified = CurrentTime;
        ActivitiesFileResourceXmlResource.Id = FileElementId;
        ActivitiesFileResourceXmlResource.Intro =
            "<p style=\"position:relative; background-color:#e6e9ed;\">" + FileElementDesc + "</p>";


        ActivitiesFileResourceXmlActivity.Resource =
            ActivitiesFileResourceXmlResource as ActivitiesResourceXmlResource ?? new ActivitiesResourceXmlResource();
        ActivitiesFileResourceXmlActivity.Id = FileElementId;
        ActivitiesFileResourceXmlActivity.ModuleId = FileElementId;
        ActivitiesFileResourceXmlActivity.ModuleName = "resource";
        ActivitiesFileResourceXmlActivity.ContextId = FileElementId;

        ActivitiesFileResourceXmlActivity.Serialize("resource", FileElementId);

        //file activities/resource.../roles.xml
        ActivitiesRolesXmlRoles.Serialize("resource", FileElementId);

        //file activities/resource.../module.xml
        ActivitiesModuleXmlModule.ModuleName = "resource";
        ActivitiesModuleXmlModule.SectionId = FileElementParentSpaceString;
        ActivitiesModuleXmlModule.SectionNumber = FileElementParentSpaceString;
        ActivitiesModuleXmlModule.Indent = "1";
        ActivitiesModuleXmlModule.Completion = "2";
        ActivitiesModuleXmlModule.CompletionView = "1";
        ActivitiesModuleXmlModule.Added = CurrentTime;
        ActivitiesModuleXmlModule.Id = FileElementId;
        ActivitiesModuleXmlModule.ShowDescription = "1";
        //AdlerScore can not be null at this point because it is set in the constructor
        ActivitiesModuleXmlModule.PluginLocalAdlerModule.AdlerModule!.ScoreMax =
            FileElementPoints.ToString("F5", CultureInfo.InvariantCulture);
        ActivitiesModuleXmlModule.PluginLocalAdlerModule.AdlerModule!.Uuid = FileElementUuid;

        ActivitiesModuleXmlModule.Serialize("resource", FileElementId);

        //file activities/resource.../grade_history.xml
        ActivitiesGradeHistoryXmlGradeHistory.Serialize("resource", FileElementId);

        //file activities/resource.../inforef.xml
        _activitiesInforefXmlFileList = new List<ActivitiesInforefXmlFile>();

        var file1 = new ActivitiesInforefXmlFile
        {
            Id = XmlEntityManager.GetFileIdBlock1().ToString()
        };
        var file2 = new ActivitiesInforefXmlFile
        {
            Id = XmlEntityManager.GetFileIdBlock1().ToString()
        };

        _activitiesInforefXmlFileList.Add(file1);
        _activitiesInforefXmlFileList.Add(file2);

        ActivitiesInforefXmlFileref.File = _activitiesInforefXmlFileList;

        ActivitiesInforefXmlInforef.Fileref = ActivitiesInforefXmlFileref as ActivitiesInforefXmlFileref ??
                                              new ActivitiesInforefXmlFileref();

        ActivitiesInforefXmlInforef.Serialize("resource", FileElementId);
    }

    /// <summary>
    /// Creates a Resource folder in the activity folder. Each activity needs an folder.
    /// </summary>
    /// <param name="moduleId"></param>
    public void CreateActivityFolder(string moduleId)
    {
        var currWorkDir = ApplicationPaths.BackupFolder;
        _fileSystem.Directory.CreateDirectory(Path.Join(currWorkDir, "XMLFilesForExport", "activities",
            "resource_" + moduleId));
    }
}