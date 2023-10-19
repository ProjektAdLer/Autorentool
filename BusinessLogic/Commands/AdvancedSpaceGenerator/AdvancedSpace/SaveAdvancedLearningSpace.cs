using BusinessLogic.API;
using BusinessLogic.Entities.AdvancedLearningSpaces;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.AdvancedSpaceGenerator.AdvancedSpace;

public class SaveAdvancedLearningSpace : ISaveAdvancedLearningSpace
{
    public SaveAdvancedLearningSpace(IBusinessLogic businessLogic, AdvancedLearningSpace advancedLearningSpace, string filepath,
        ILogger<SaveAdvancedLearningSpace> logger)
    {
        BusinessLogic = businessLogic;
        AdvancedLearningSpace = advancedLearningSpace;
        Filepath = filepath;
        Logger = logger;
    }

    internal IBusinessLogic BusinessLogic { get; }
    internal AdvancedLearningSpace AdvancedLearningSpace { get; }
    internal string Filepath { get; }
    private ILogger<SaveAdvancedLearningSpace> Logger { get; }
    public string Name => nameof(SaveAdvancedLearningSpace);

    public void Execute()
    {
        // BusinessLogic.SaveAdvancedLearningSpace(AdvancedLearningSpace, Filepath);
        // Logger.LogTrace("Saved AdvancedLearningSpace {LearningSpaceName} ({LearningSpaceId}) to file {FilePath}",
        //     AdvancedLearningSpace.Name, AdvancedLearningSpace.Id, Filepath);
    }
}