using Presentation.PresentationLogic.LearningSpace.LearningOutcomeViewModel;
using Shared;
using Shared.Theme;

namespace Presentation.Components.Forms.Models;

public class LearningSpaceFormModel
{
    public LearningSpaceFormModel()
    {
        Name = "";
        Description = "";
        LearningOutcomeCollection = new LearningOutcomeCollectionViewModel();
        RequiredPoints = 0;
        SpaceTheme = default;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public LearningOutcomeCollectionViewModel LearningOutcomeCollection { get; set; }
    public int RequiredPoints { get; set; }
    public SpaceTheme SpaceTheme { get; set; }
}