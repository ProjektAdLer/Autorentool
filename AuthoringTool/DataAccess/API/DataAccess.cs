using AuthoringTool.API.Configuration;
using AuthoringTool.DataAccess.Persistence;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.Entities;

namespace AuthoringTool.DataAccess.API;

internal class DataAccess : IDataAccess
{
    public DataAccess(IAuthoringToolConfiguration configuration, IBackupFileGenerator backupFileGenerator,
        IFileSaveHandler<LearningWorld> saveHandlerWorld, IFileSaveHandler<LearningSpace> saveHandlerSpace,
        IFileSaveHandler<LearningElement> saveHandlerElement)
    {
        SaveHandlerWorld = saveHandlerWorld;
        SaveHandlerSpace = saveHandlerSpace;
        SaveHandlerElement = saveHandlerElement;
        Configuration = configuration;
        BackupFile = backupFileGenerator;
    }

    public readonly IFileSaveHandler<LearningWorld> SaveHandlerWorld;
    public readonly IFileSaveHandler<LearningSpace> SaveHandlerSpace;
    public readonly IFileSaveHandler<LearningElement> SaveHandlerElement;
    public IAuthoringToolConfiguration Configuration { get; }

    public IBackupFileGenerator BackupFile { get; set; }

    public void ConstructBackup()
    {
        BackupFile.CreateBackupFolders();
        BackupFile.WriteXMLFiles();
        BackupFile.WriteBackupFile();
    }

    public void SaveLearningWorldToFile(LearningWorld world, string filepath)
    {
        SaveHandlerWorld.SaveToDisk(world, filepath);
    }

    public LearningWorld LoadLearningWorldFromFile(string filepath)
    {
        return SaveHandlerWorld.LoadFromDisk(filepath);
    }

    public void SaveLearningSpaceToFile(LearningSpace space, string filepath)
    {
        SaveHandlerSpace.SaveToDisk(space, filepath);
    }

    public LearningSpace LoadLearningSpaceFromFile(string filepath)
    {
        return SaveHandlerSpace.LoadFromDisk(filepath);
    }

    public void SaveLearningElementToFile(LearningElement element, string filepath)
    {
        SaveHandlerElement.SaveToDisk(element, filepath);
    }

    public LearningElement LoadLearningElementFromFile(string filepath)
    {
        return SaveHandlerElement.LoadFromDisk(filepath);
    }
}