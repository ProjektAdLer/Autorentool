using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Action;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Trigger;

namespace Presentation.PresentationLogic.LearningContent.AdaptivityContent;

class AdaptivityRuleViewModel : IAdaptivityRuleViewModel
{
    public AdaptivityRuleViewModel(IAdaptivityQuestionViewModel question, IAdaptivityTriggerViewModel trigger, IAdaptivityActionViewModel action)
    {
        Question = question;
        Trigger = trigger;
        Action = action;
    }

    public IAdaptivityQuestionViewModel Question { get; set; }
    public IAdaptivityTriggerViewModel Trigger { get; set; }
    public IAdaptivityActionViewModel Action { get; set; }
}