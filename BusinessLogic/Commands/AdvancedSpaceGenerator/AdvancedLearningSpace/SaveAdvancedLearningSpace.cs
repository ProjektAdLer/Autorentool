using BusinessLogic.API;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Space.AdvancedLearningSpace;

public class SaveAdvancedLearningSpace : ISaveAdvancedLearningSpace
{
    public SaveAdvancedLearningSpace(IBusinessLogic businessLogic, Entities.AdvancedLearningSpaces.AdvancedLearningSpace advancedLearningSpace, string filepath,
        ILogger<SaveAdvancedLearningSpace> logger)
    {
        BusinessLogic = businessLogic;
        AdvancedLearningSpace = advancedLearningSpace;
        Filepath = filepath;
        Logger = logger;
    }

    internal IBusinessLogic BusinessLogic { get; }
    internal Entities.AdvancedLearningSpaces.AdvancedLearningSpace AdvancedLearningSpace { get; }
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