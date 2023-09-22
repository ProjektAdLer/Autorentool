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
    private readonly IFileSystem _fileSystem;
    public readonly ICreateDsl CreateDsl;
    public readonly IReadDsl ReadDsl;

    internal IMapper Mapper;

    public WorldGenerator(IBackupFileGenerator backupFileGenerator, ICreateDsl createDsl, IReadDsl readDsl,
        IMapper mapper, IFileSystem fileSystem)
    {
        BackupFile = backupFileGenerator;
        CreateDsl = createDsl;
        ReadDsl = readDsl;
        Mapper = mapper;
        _fileSystem = fileSystem;
    }

    public IBackupFileGenerator BackupFile { get; }


    /// <inheritdoc cref="IWorldGenerator.ConstructBackup"/>
    public void ConstructBackup(LearningWorld learningWorld, string filepath)
    {
        try
        {
            var dslPath = CreateDsl.GenerateAndExportLearningWorldJson(Mapper.Map<LearningWorldPe>(learningWorld));
            ReadDsl.ReadLearningWorld(dslPath);
            BackupFile.WriteXmlFiles((ReadDsl as ReadDsl)!);
            BackupFile.WriteBackupFile(filepath);
        }
        finally
        {
            if (_fileSystem.Directory.Exists("XMLFilesForExport"))
                _fileSystem.Directory.Delete("XMLFilesForExport", true);
        }
    }

    public string ExtractAtfFromBackup(string filepath)
    {
        return BackupFile.ExtractAtfFromBackup(filepath);
    }
}