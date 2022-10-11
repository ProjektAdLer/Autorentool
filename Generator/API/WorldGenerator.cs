using AutoMapper;
using BusinessLogic.API;
using BusinessLogic.Entities;
using Generator.DSL;
using Generator.WorldExport;
using PersistEntities;

namespace Generator.API;

public class WorldGenerator : IWorldGenerator
{
    public WorldGenerator(IBackupFileGenerator backupFileGenerator, ICreateDsl createDsl, IReadDsl readDsl, IMapper mapper)
    {
        BackupFile = backupFileGenerator;
        CreateDsl = createDsl;
        ReadDsl = readDsl;
        Mapper = mapper;
    }

    internal IMapper Mapper;
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
        string dslPath = CreateDsl.WriteLearningWorld(Mapper.Map<LearningWorldPe>(learningWorld));
        ReadDsl.ReadLearningWorld(dslPath);
        BackupFile.WriteXmlFiles((ReadDsl as ReadDsl)!);
        BackupFile.WriteBackupFile(filepath);
    }
    
}