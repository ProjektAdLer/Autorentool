using Presentation.PresentationLogic.LearningSpace.LearningOutcomeViewModel;
using Shared;

namespace Presentation.Components.Forms.Models;

public class LearningSpaceFormModel
{
    public LearningSpaceFormModel()
    {
        Name = "";
        Description = "";
        LearningOutcomes = new List<ILearningOutcomeViewModel>();
        RequiredPoints = 0;
        Theme = default;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<ILearningOutcomeViewModel> LearningOutcomes { get; set; }
    public int RequiredPoints { get; set; }
    public Theme Theme { get; set; }
}