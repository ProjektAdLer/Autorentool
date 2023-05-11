using System;
using BusinessLogic.Commands;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.SelectedViewModels;
using Shared;

namespace PresentationTest.PresentationLogic.SelectedViewModels;

[TestFixture]

public class SelectedLearningObjectInPathWayViewModelStackEntryUt
{
    [Test]
    public void Apply_InvokesActionWithCorrectParameter()
    {
        var command = Substitute.For<ICommand>();
        var condition = new PathWayConditionViewModel(ConditionEnum.Or, false);
        Action<ISelectableObjectInWorldViewModel?> action = Substitute.For<Action<ISelectableObjectInWorldViewModel?>>();
        
        var systemUnderTest = new SelectedLearningObjectInPathWayViewModelStackEntry(command, condition, action);
        
        Assert.That(systemUnderTest.Command, Is.EqualTo(command));
        
        systemUnderTest.Apply();
        
        action.Received(1).Invoke(condition);
    }
}