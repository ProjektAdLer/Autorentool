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
    public WorldGenerator(IBackupFileGenerator backupFileGenerator, ICreateDsl createDsl, IReadDsl readDsl, IMapper mapper, IFileSystem fileSystem)
    {
        BackupFile = backupFileGenerator;
        CreateDsl = createDsl;
        ReadDsl = readDsl;
        Mapper = mapper;
        _fileSystem = fileSystem;
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
    /// <param name="world"></param> Information about the world, topics, spaces and Elements
    /// <param name="filepath"></param> Desired filepath for the .mbz file. Given by user, when Export Button is pressed.
    public void ConstructBackup(World world, string filepath)
    {
        try
        {
            string dslPath = CreateDsl.WriteWorld(Mapper.Map<WorldPe>(world));
            ReadDsl.ReadWorld(dslPath);
            BackupFile.WriteXmlFiles((ReadDsl as ReadDsl)!);
            BackupFile.WriteBackupFile(filepath);
        }
        finally
        {
            if (_fileSystem.Directory.Exists("XMLFilesForExport"))
                _fileSystem.Directory.Delete("XMLFilesForExport", true);
        }
    }
    
}