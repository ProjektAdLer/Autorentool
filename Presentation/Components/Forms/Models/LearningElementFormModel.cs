using Presentation.Components.Forms.Element;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningContent.FileContent;
using Presentation.PresentationLogic.LearningContent.LinkContent;
using Shared;

namespace Presentation.Components.Forms.Models;

public class LearningElementFormModel
{
    private ILearningContentViewModel? _learningContent;

    public LearningElementFormModel()
    {
        Name = "";
        Description = "";
        Description = "";
        Goals = "";
        Difficulty = LearningElementDifficultyEnum.None;
        ElementModel = ElementModel.l_random;
        Workload = 0;
        Points = 1;
        LearningContent = null;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public LearningElementDifficultyEnum Difficulty { get; set; }
    public ElementModel ElementModel { get; set; }
    public int Workload { get; set; }
    public int Points { get; set; }

    public ILearningContentViewModel? LearningContent
    {
        get => _learningContent;
        set
        {
            if (Equals(value, _learningContent)) return;
            var learningContentIsSameType =
                (_learningContent is LinkContentViewModel && value is LinkContentViewModel) ||
                (_learningContent is FileContentViewModel lC && value is FileContentViewModel vC &&
                 lC.Type == vC.Type);

            _learningContent = value;
            if (learningContentIsSameType) return;
            ElementModel = ElementModelHandler.GetElementModelRandom();
        }
    }
}