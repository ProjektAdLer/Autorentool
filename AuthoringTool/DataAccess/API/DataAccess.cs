using AuthoringTool.API.Configuration;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.DataAccess.XmlClasses;

namespace AuthoringTool.DataAccess.API;

internal class DataAccess : IDataAccess
{
    public DataAccess(IAuthoringToolConfiguration configuration)
    {
        Configuration = configuration;
        BackupFile = new BackupFileGenerator();
    }
    
    //We dont want to Test this Constructor
    internal DataAccess(IAuthoringToolConfiguration configuration, IBackupFileGenerator backupFile)
    {
        Configuration = configuration;
        BackupFile = backupFile;
    }
    
    public void ConstructBackup()
    {
        BackupFile.CreateXMLFiles();
        BackupFile.CreateBackupFile();
    }
    
    public IAuthoringToolConfiguration Configuration { get; }
    
    public IBackupFileGenerator BackupFile { get; set; }
}