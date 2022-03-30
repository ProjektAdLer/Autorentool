using AuthoringTool.API.Configuration;
using AuthoringTool.DataAccess.Persistence;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.Entities;

namespace AuthoringTool.DataAccess.API;

internal class DataAccess : IDataAccess
{

    
    public DataAccess(IAuthoringToolConfiguration configuration, IBackupFileGenerator backupFileGenerator,
        IFileSaveHandler<LearningWorld> saveHandlerWorld)
    {
        SaveHandlerWorld = saveHandlerWorld;
        Configuration = configuration;
        BackupFile = backupFileGenerator;
    }
    
    public readonly IFileSaveHandler<LearningWorld> SaveHandlerWorld;
    public IAuthoringToolConfiguration Configuration { get; }
    
    public IBackupFileGenerator BackupFile { get; set; }
    
    public void ConstructBackup()
    {
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
    
}