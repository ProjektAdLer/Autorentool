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
        IXmlFileHandler<LearningElement> xmlHandlerElement, IContentFileHandler contentHandler, ICreateDSL createDsl, IReadDSL readDsl)
    {
        XmlHandlerWorld = xmlHandlerWorld;
        XmlHandlerSpace = xmlHandlerSpace;
        XmlHandlerElement = xmlHandlerElement;
        ContentHandler = contentHandler;
        Configuration = configuration;
        BackupFile = backupFileGenerator;
        CreateDsl = createDsl;
        ReadDsl = readDsl;
    }

    public readonly IXmlFileHandler<LearningWorld> XmlHandlerWorld;
    public readonly IXmlFileHandler<LearningSpace> XmlHandlerSpace;
    public readonly IXmlFileHandler<LearningElement> XmlHandlerElement;
    public readonly IContentFileHandler ContentHandler;
    public IAuthoringToolConfiguration Configuration { get; }

    public IBackupFileGenerator BackupFile { get; set; }
    public ICreateDSL CreateDsl;
    public IReadDSL ReadDsl;

    /// <summary>
    /// Creates the DSL document, reads it, creates the needed folder structure for the backup, fills the folders with
    /// the needed xml documents and saves it to the desired location as .mbz file. 
    /// </summary>
    /// <param name="learningWorld"></param> Information about the learningWorld, topics, spaces and elements
    /// <param name="filepath"></param> Desired filepath for the .mbz file. Given by user, when Export Button is pressed.
    public void ConstructBackup(LearningWorld learningWorld, string filepath)
    {
        CreateDsl.WriteLearningWorld(learningWorld);
        ReadDsl.ReadLearningWorld();
        BackupFile.CreateBackupFolders();
        BackupFile.WriteXmlFiles(ReadDsl as ReadDSL);
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
}