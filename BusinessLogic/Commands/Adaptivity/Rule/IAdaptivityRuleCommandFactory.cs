using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using BusinessLogic.Entities.LearningContent.Adaptivity.Trigger;

namespace BusinessLogic.Commands.Adaptivity.Rule;

public interface IAdaptivityRuleCommandFactory
{
    ICreateAdaptivityRule GetCreateCommand(IAdaptivityQuestion question, IAdaptivityTrigger trigger,
        IAdaptivityAction action, Action<IAdaptivityQuestion> mappingAction);

    IDeleteAdaptivityRule GetDeleteCommand(IAdaptivityQuestion question, IAdaptivityRule rule,
        Action<IAdaptivityQuestion> mappingAction);
}