using BusinessLogic.Entities.LearningContent.AdaptivityContent.Action;
using BusinessLogic.Entities.LearningContent.AdaptivityContent.Question;
using BusinessLogic.Entities.LearningContent.AdaptivityContent.Trigger;

namespace BusinessLogic.Entities.LearningContent.AdaptivityContent;

class AdaptivityRule : IAdaptivityRule
{
    public AdaptivityRule(IAdaptivityQuestion question, IAdaptivityTrigger trigger, IAdaptivityAction action)
    {
        Question = question;
        Trigger = trigger;
        Action = action;
    }

    public IAdaptivityQuestion Question { get; set; }
    public IAdaptivityTrigger Trigger { get; set; }
    public IAdaptivityAction Action { get; set; }
}