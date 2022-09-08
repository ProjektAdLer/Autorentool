using System.IO.Abstractions;
using AutoMapper;
using BusinessLogic.API;
using BusinessLogic.Entities;
using Generator.DSL;
using Generator.WorldExport;
using PersistEntities;

namespace Generator.API;

public class WorldGenerator : IWorldGenerator
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
    public readonly ICreateDsl CreateDsl;
    public readonly IReadDsl ReadDsl;

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
        BackupFile.WriteXmlFiles((ReadDsl as ReadDsl)!);
        BackupFile.WriteBackupFile(filepath);
    }
    
}