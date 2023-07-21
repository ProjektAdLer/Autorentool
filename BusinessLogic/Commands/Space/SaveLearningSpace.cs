using BusinessLogic.API;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Space;

public class SaveLearningSpace : ISaveLearningSpace
{
    public string Name => nameof(SaveLearningSpace);
    internal IBusinessLogic BusinessLogic { get; }
    internal LearningSpace LearningSpace { get; }
    internal string Filepath { get; }
    private ILogger<SpaceCommandFactory> Logger { get; }

    public SaveLearningSpace(IBusinessLogic businessLogic, LearningSpace learningSpace, string filepath, ILogger<SpaceCommandFactory> logger)
    {
        BusinessLogic = businessLogic;
        LearningSpace = learningSpace;
        Filepath = filepath;
        Logger = logger;
    }
    
    public void Execute()
    {
        BusinessLogic.SaveLearningSpace(LearningSpace, Filepath);
        Logger.LogTrace("Saved LearningSpace {LearningSpaceName} ({LearningSpaceId}) to file {FilePath}", LearningSpace.Name, LearningSpace.Id, Filepath);
    }
}