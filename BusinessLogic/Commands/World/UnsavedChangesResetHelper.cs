using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;

namespace BusinessLogic.Commands.World;

public class UnsavedChangesResetHelper : IUnsavedChangesResetHelper
{
    public void ResetWorldUnsavedChangesState(ILearningWorld learningWorld)
    {
        learningWorld.UnsavedChanges = false;
        foreach (var element in learningWorld.UnplacedLearningElements)
        {
            ResetElementUnsavedChangesState(element);
        }

        foreach (var space in learningWorld.LearningSpaces)
        {
            space.UnsavedChanges = false;
            foreach (var element in space.ContainedLearningElements.Union(
                         space.LearningSpaceLayout.StoryElements.Values))
            {
                ResetElementUnsavedChangesState(element);
            }

            space.LearningOutcomeCollection.UnsavedChanges = false;
        }

        foreach (var condition in learningWorld.PathWayConditions)
        {
            condition.UnsavedChanges = false;
        }

        foreach (var topic in learningWorld.Topics)
        {
            topic.UnsavedChanges = false;
        }
    }

    private void ResetElementUnsavedChangesState(ILearningElement element)
    {
        element.UnsavedChanges = false;
        element.LearningContent.UnsavedChanges = false;
        if (element.LearningContent is IAdaptivityContent ac)
            ResetUnsavedChangesAdaptivityContent(ac);
    }

    private void ResetUnsavedChangesAdaptivityContent(IAdaptivityContent adaptivityContent)
    {
        foreach (var task in adaptivityContent.Tasks)
        {
            ResetUnsavedChangesInTask(task);
        }
    }

    private void ResetUnsavedChangesInTask(IAdaptivityTask task)
    {
        task.UnsavedChanges = false;
        foreach (var question in task.Questions)
        {
            ResetUnsavedChangesInQuestion(question);
        }
    }

    private void ResetUnsavedChangesInQuestion(IAdaptivityQuestion question)
    {
        question.UnsavedChanges = false;
        switch (question)
        {
            case MultipleChoiceMultipleResponseQuestion mcmrq:
                ResetUnsavedChangesInMultipleResponseQuestion(mcmrq);
                break;
            case MultipleChoiceSingleResponseQuestion mcsrq:
                ResetUnsavedChangesInSingleResponseQuestion(mcsrq);
                break;
        }

        foreach (var rule in question.Rules)
        {
            ResetUnsavedChangesInRule(rule);
        }
    }

    private void ResetUnsavedChangesInMultipleResponseQuestion(MultipleChoiceMultipleResponseQuestion mcmrq)
    {
        foreach (var correctChoice in mcmrq.CorrectChoices)
            correctChoice.UnsavedChanges = false;
        foreach (var choice in mcmrq.Choices)
            choice.UnsavedChanges = false;
    }

    private void ResetUnsavedChangesInSingleResponseQuestion(MultipleChoiceSingleResponseQuestion mcsrq)
    {
        mcsrq.CorrectChoice.UnsavedChanges = false;
        foreach (var choice in mcsrq.Choices)
            choice.UnsavedChanges = false;
    }

    private void ResetUnsavedChangesInRule(IAdaptivityRule rule)
    {
        rule.UnsavedChanges = false;
        rule.Action.UnsavedChanges = false;
        rule.Action.UnsavedChanges = false;
        rule.Trigger.UnsavedChanges = false;
    }
}