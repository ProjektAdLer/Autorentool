using BusinessLogic.API;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Element;

public class SaveLearningElement : ISaveLearningElement
{
    public string Name => nameof(SaveLearningElement);
    internal IBusinessLogic BusinessLogic { get; }
    internal LearningElement LearningElement { get; }
    internal string Filepath { get; }
    private ILogger<ElementCommandFactory> Logger { get; }
    
    public SaveLearningElement(IBusinessLogic businessLogic, LearningElement learningElement, string filepath, ILogger<ElementCommandFactory> logger)
    {
        BusinessLogic = businessLogic;
        LearningElement = learningElement;
        Filepath = filepath;
        Logger = logger;
    }
    
    public void Execute()
    {
        BusinessLogic.SaveLearningElement(LearningElement, Filepath);

        Logger.LogTrace("Saved LearningElement {LearningElementName} ({LearningElementId}) to file {Filepath}", LearningElement.Name, LearningElement.Id, Filepath);
    }
}