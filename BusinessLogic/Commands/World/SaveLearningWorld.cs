using BusinessLogic.API;
using BusinessLogic.Entities;

namespace BusinessLogic.Commands.World;

public class SaveLearningWorld : ICommand
{
    public string Name => nameof(SaveLearningWorld);
    private readonly IBusinessLogic _businessLogic;
    private readonly LearningWorld _learningWorld;
    private readonly string _filepath;
    
    public SaveLearningWorld(IBusinessLogic businessLogic, LearningWorld learningWorld, string filepath)
    {
        _businessLogic = businessLogic;
        _learningWorld = learningWorld;
        _filepath = filepath;
    }
    
    public void Execute()
    {
        _businessLogic.SaveLearningWorld(_learningWorld, _filepath);
        ResetWorldUnsavedChangesState();
    }

    private void ResetWorldUnsavedChangesState()
    {
        _learningWorld.UnsavedChanges = false;
        foreach (var element in _learningWorld.UnplacedLearningElements)
        {
            element.UnsavedChanges = false;
        }

        foreach (var space in _learningWorld.LearningSpaces)
        {
            space.UnsavedChanges = false;
            foreach (var element in space.ContainedLearningElements)
            {
                element.UnsavedChanges = false;
            }
        }

        foreach (var condition in _learningWorld.PathWayConditions)
        {
            condition.UnsavedChanges = false;
        }

        foreach (var topic in _learningWorld.Topics)
        {
            topic.UnsavedChanges = false;
        }
    }
}