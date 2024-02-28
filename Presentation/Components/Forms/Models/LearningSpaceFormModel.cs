using Presentation.PresentationLogic.LearningSpace.LearningOutcomeViewModel;
using Shared;

namespace Presentation.Components.Forms.Models;

public class LearningSpaceFormModel
{
    public LearningSpaceFormModel()
    {
        Name = "";
        Description = "";
        LearningOutcomeCollection = new LearningOutcomeCollectionViewModel();
        RequiredPoints = 0;
        Theme = default;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public LearningOutcomeCollectionViewModel LearningOutcomeCollection { get; set; }
    public int RequiredPoints { get; set; }
    public Theme Theme { get; set; }
}