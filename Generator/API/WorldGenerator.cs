using System.IO.Abstractions;
using AuthoringTool.BusinessLogic.API;
using AuthoringTool.Entities;
using AutoMapper;
using Generator.DSL;
using Generator.PersistEntities;
using Generator.WorldExport;

namespace Generator.API;

internal class WorldGenerator : IWorldGenerator
{
    public WorldGenerator(IBackupFileGenerator backupFileGenerator, ICreateDsl createDsl, IReadDsl readDsl, 
        IFileSystem fileSystem, IMapper mapper)
    {
        _fileSystem = fileSystem;
        BackupFile = backupFileGenerator;
        CreateDsl = createDsl;
        ReadDsl = readDsl;
        Mapper = mapper;
    }

    internal IMapper Mapper;
    private readonly IFileSystem _fileSystem;
    public IBackupFileGenerator BackupFile { get; }
    public ICreateDsl CreateDsl;
    public IReadDsl ReadDsl;

    /// <summary>
    /// Creates the DSL document, reads it, creates the needed folder structure for the backup, fills the folders with
    /// the needed xml documents and saves it to the desired location as .mbz file. 
    /// </summary>
    /// <param name="learningWorld"></param> Information about the learningWorld, topics, spaces and elements
    /// <param name="filepath"></param> Desired filepath for the .mbz file. Given by user, when Export Button is pressed.
    public void ConstructBackup(LearningWorld learningWorld, string filepath)
    {
        string dslpath = CreateDsl.WriteLearningWorld(Mapper.Map<LearningWorldPe>(learningWorld));
        ReadDsl.ReadLearningWorld(dslpath);
        BackupFile.CreateBackupFolders();
        BackupFile.WriteXmlFiles(ReadDsl as ReadDsl, dslpath);
        BackupFile.WriteBackupFile(filepath);
    }
    
}