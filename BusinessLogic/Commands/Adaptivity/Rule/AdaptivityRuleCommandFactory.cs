using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using BusinessLogic.Entities.LearningContent.Adaptivity.Trigger;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Adaptivity.Rule;

class AdaptivityRuleCommandFactory : IAdaptivityRuleCommandFactory
{
    public AdaptivityRuleCommandFactory(ILoggerFactory loggerFactory)
    {
        LoggerFactory = loggerFactory;
    }

    private ILoggerFactory LoggerFactory { get; }

    public ICreateAdaptivityRule GetCreateCommand(IAdaptivityQuestion question, IAdaptivityTrigger trigger,
        IAdaptivityAction action, Action<IAdaptivityQuestion> mappingAction)
    {
        return new CreateAdaptivityRule(question, trigger, action, mappingAction,
            LoggerFactory.CreateLogger<CreateAdaptivityRule>());
    }

    public IDeleteAdaptivityRule GetDeleteCommand(IAdaptivityQuestion question, IAdaptivityRule rule,
        Action<IAdaptivityQuestion> mappingAction)
    {
        return new DeleteAdaptivityRule(question, rule, mappingAction,
            LoggerFactory.CreateLogger<DeleteAdaptivityRule>());
    }
}