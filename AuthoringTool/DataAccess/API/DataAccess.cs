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
        IXmlFileHandler<LearningElement> xmlHandlerElement, IContentFileHandler contentHandler)
    {
        XmlHandlerWorld = xmlHandlerWorld;
        XmlHandlerSpace = xmlHandlerSpace;
        XmlHandlerElement = xmlHandlerElement;
        ContentHandler = contentHandler;
        Configuration = configuration;
        BackupFile = backupFileGenerator;
    }

    public readonly IXmlFileHandler<LearningWorld> XmlHandlerWorld;
    public readonly IXmlFileHandler<LearningSpace> XmlHandlerSpace;
    public readonly IXmlFileHandler<LearningElement> XmlHandlerElement;
    public readonly IContentFileHandler ContentHandler;
    public IAuthoringToolConfiguration Configuration { get; }

    public IBackupFileGenerator BackupFile { get; set; }
    public ICreateDSL CreateDsl { get; set; }

    public void ConstructBackup(LearningWorld learningWorld, string filepath)
    {
        //CreateDsl.WriteLearningWorld(learningWorld);
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

    public void SaveLearningSpaceToFile(LearningSpace space, string filepath)
    {
        XmlHandlerSpace.SaveToDisk(space, filepath);
    }

    public LearningSpace LoadLearningSpaceFromFile(string filepath)
    {
        return XmlHandlerSpace.LoadFromDisk(filepath);
    }

    public void SaveLearningElementToFile(LearningElement element, string filepath)
    {
        XmlHandlerElement.SaveToDisk(element, filepath);
    }

    public LearningElement LoadLearningElementFromFile(string filepath)
    {
        return XmlHandlerElement.LoadFromDisk(filepath);
    }
    
    public LearningContent LoadLearningContentFromFile(string filepath)
    {
        return ContentHandler.LoadFromDisk(filepath);
    }
}