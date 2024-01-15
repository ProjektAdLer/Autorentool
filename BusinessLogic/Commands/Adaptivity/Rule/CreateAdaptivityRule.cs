using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using BusinessLogic.Entities.LearningContent.Adaptivity.Trigger;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Commands.Adaptivity.Rule;

internal class CreateAdaptivityRule : ICreateAdaptivityRule
{

    public CreateAdaptivityRule(IAdaptivityQuestion question, IAdaptivityTrigger trigger, IAdaptivityAction action,
        Action<IAdaptivityQuestion> mappingAction, ILogger<CreateAdaptivityRule> logger)
    {
        Question = question;
        Rule = new AdaptivityRule(trigger, action);
        MappingAction = mappingAction;
        Logger = logger;
    }

    public string Name => nameof(CreateAdaptivityRule);
    internal IAdaptivityQuestion Question { get; }
    internal IAdaptivityRule Rule { get; }
    internal Action<IAdaptivityQuestion> MappingAction { get; }
    private ILogger<CreateAdaptivityRule> Logger { get; }
    private IMemento? Memento { get; set; }

    public void Execute()
    {
        Memento = Question.GetMemento();
        
        Question.Rules.Add(Rule);
        Question.UnsavedChanges = true;
        MappingAction.Invoke(Question);

        Logger.LogTrace(
            "Created Rule in Question {QuestionId}. Trigger: {Trigger}, Action: {Action}",
            Question.Id, Rule.Trigger, Rule.Action);
    }

    public void Undo()
    {
        if (Memento == null)
        {
            throw new InvalidOperationException("Memento is null");
        }

        Question.RestoreMemento(Memento);
        MappingAction.Invoke(Question);
        
        Logger.LogTrace(
            "Undone creation of Rule in Question {QuestionId}. Trigger: {Trigger}, Action: {Action}",
            Question.Id, Rule.Trigger, Rule.Action);
    }

    public void Redo()
    {
        Logger.LogTrace("Redoing CreateAdaptivityRule");
        Execute();
    }
}