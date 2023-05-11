using System;
using BusinessLogic.Commands;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.SelectedViewModels;

namespace PresentationTest.PresentationLogic.SelectedViewModels;

[TestFixture]

public class SelectedLearningElementViewModelStackEntryUt
{
    [Test]
    public void Apply_InvokesActionWithCorrectParameter()
    {
        var command = Substitute.For<ICommand>();
        var element = Substitute.For<ILearningElementViewModel>();
        Action<ILearningElementViewModel?> action = Substitute.For<Action<ILearningElementViewModel?>>();
        
        var systemUnderTest = new SelectedLearningElementViewModelStackEntry(command, element, action);
        
        Assert.That(systemUnderTest.Command, Is.EqualTo(command));
        
        systemUnderTest.Apply();
        
        action.Received(1).Invoke(element);
    }
}