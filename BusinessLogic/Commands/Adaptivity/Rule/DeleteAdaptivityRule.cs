using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Adaptivity.Rule;

internal class DeleteAdaptivityRule : IDeleteAdaptivityRule
{
    public DeleteAdaptivityRule(IAdaptivityQuestion question, IAdaptivityRule rule,
        Action<IAdaptivityQuestion> mappingAction, ILogger<DeleteAdaptivityRule> logger)
    {
        Question = question;
        Rule = rule;
        MappingAction = mappingAction;
        Logger = logger;
    }

    public string Name => nameof(DeleteAdaptivityRule);
    internal IAdaptivityQuestion Question { get; }
    internal IAdaptivityRule Rule { get; }
    internal Action<IAdaptivityQuestion> MappingAction { get; }
    private ILogger<DeleteAdaptivityRule> Logger { get; }
    private IMemento? Memento { get; set; }

    public void Execute()
    {
        Memento = Question.GetMemento();

        var rule = Question.Rules.Single(q => q.Id == Rule.Id);
        Question.Rules.Remove(rule);
        MappingAction.Invoke(Question);

        Logger.LogTrace("Deleted AdaptivityRule {AdaptivityRuleId} from AdaptivityQuestion {AdaptivityQuestionId}",
            Rule.Id, Question.Id);
    }

    public void Undo()
    {
        if (Memento == null) throw new InvalidOperationException("Memento is null");

        Question.RestoreMemento(Memento);
        MappingAction.Invoke(Question);

        Logger.LogTrace("Undne deletion of Rule in Question {QuestionId}. Trigger: {Trigger}, Action: {Action}",
            Question.Id, Rule.Trigger, Rule.Action);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing DeleteAdaptivityRule");
        Execute();
    }
}