using System;
using BusinessLogic.Commands;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.SelectedViewModels;

namespace PresentationTest.PresentationLogic.SelectedViewModels;

[TestFixture]

public class SelectedLearningWorldViewModelStackEntryUt
{
    [Test]
    public void Apply_InvokesActionWithCorrectParameter()
    {
        var command = Substitute.For<ICommand>();
        var world = new LearningWorldViewModel("Test", "s", "f","f","d","e");
        Action<ILearningWorldViewModel?> action = Substitute.For<Action<ILearningWorldViewModel?>>();
        
        var systemUnderTest = new SelectedLearningWorldViewModelStackEntry(command, world, action);
        
        Assert.That(systemUnderTest.Command, Is.EqualTo(command));
        
        systemUnderTest.Apply();
        
        action.Received(1).Invoke(world);
    }
}