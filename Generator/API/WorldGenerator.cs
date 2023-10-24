using System.IO.Abstractions;
using AutoMapper;
using BusinessLogic.API;
using BusinessLogic.Entities;
using Generator.ATF;
using Generator.WorldExport;
using PersistEntities;

namespace Generator.API;

public class WorldGenerator : IWorldGenerator
{
    private readonly IFileSystem _fileSystem;
    public readonly ICreateAtf CreateAtf;
    public readonly IReadAtf ReadAtf;

    internal IMapper Mapper;

    public WorldGenerator(IBackupFileGenerator backupFileGenerator, ICreateAtf createAtf, IReadAtf readAtf,
        IMapper mapper, IFileSystem fileSystem)
    {
        BackupFile = backupFileGenerator;
        CreateAtf = createAtf;
        ReadAtf = readAtf;
        Mapper = mapper;
        _fileSystem = fileSystem;
    }

    public IBackupFileGenerator BackupFile { get; }


    /// <inheritdoc cref="IWorldGenerator.ConstructBackup"/>
    public void ConstructBackup(LearningWorld learningWorld, string filepath)
    {
        try
        {
            var atfPath = CreateAtf.GenerateAndExportLearningWorldJson(Mapper.Map<LearningWorldPe>(learningWorld));
            ReadAtf.ReadLearningWorld(atfPath);
            BackupFile.WriteXmlFiles((ReadAtf as ReadAtf)!);
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