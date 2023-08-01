using BusinessLogic.API;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Element;

public class SaveLearningElement : ISaveLearningElement
{
    public SaveLearningElement(IBusinessLogic businessLogic, LearningElement learningElement, string filepath,
        ILogger<SaveLearningElement> logger)
    {
        BusinessLogic = businessLogic;
        LearningElement = learningElement;
        Filepath = filepath;
        Logger = logger;
    }

    internal IBusinessLogic BusinessLogic { get; }
    internal LearningElement LearningElement { get; }
    internal string Filepath { get; }
    private ILogger<SaveLearningElement> Logger { get; }
    public string Name => nameof(SaveLearningElement);

    public void Execute()
    {
        BusinessLogic.SaveLearningElement(LearningElement, Filepath);

        Logger.LogTrace("Saved LearningElement {LearningElementName} ({LearningElementId}) to file {Filepath}",
            LearningElement.Name, LearningElement.Id, Filepath);
    }
}