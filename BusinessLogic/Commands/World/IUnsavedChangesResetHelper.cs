using BusinessLogic.Entities;

namespace BusinessLogic.Commands.World;

public interface IUnsavedChangesResetHelper
{
    void ResetWorldUnsavedChangesState(ILearningWorld learningWorld);
}