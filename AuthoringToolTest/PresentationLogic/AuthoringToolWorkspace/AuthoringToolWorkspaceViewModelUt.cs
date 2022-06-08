using System;
using System.Collections.Generic;
using System.Linq;
using AuthoringTool.PresentationLogic.AuthoringToolWorkspace;
using AuthoringTool.PresentationLogic.LearningWorld;
using NUnit.Framework;

namespace AuthoringToolTest.PresentationLogic.AuthoringToolWorkspace;

[TestFixture]
public class AuthoringToolWorkspaceViewModelUt
{
    [Test]
    public void AuthoringToolWorkspaceViewModel_Constructor_EnumerableStartsEmpty()
    {
        IAuthoringToolWorkspaceViewModel systemUnderTest = GetViewModelForTesting();
        
        Assert.That(systemUnderTest.LearningWorlds, Is.Empty);
    }
    
    [Test]
    public void AuthoringToolWorkspaceViewModel_AddLearningWorld_AddsLearningWorldToEnumerable()
    {
        var viewModel = GetLearningWorldViewModelForTesting();
        
        IAuthoringToolWorkspaceViewModel systemUnderTest = GetViewModelForTesting();
        
        systemUnderTest.AddLearningWorld(viewModel);
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningWorlds.Count(), Is.EqualTo(1));
            Assert.That(systemUnderTest.LearningWorlds, Contains.Item(viewModel));
        });
    }

    [Test]
    public void AuthoringToolWorkspaceViewModel_AddLearningWorld_RaisesStateChangeEventWithCurrentState()
    {
        var viewModel = GetLearningWorldViewModelForTesting();
        var handlerCalled = false;

        IAuthoringToolWorkspaceViewModel systemUnderTest = GetViewModelForTesting();
        systemUnderTest.PropertyChanged += (caller, changedEventArgs) => {
            if (handlerCalled) Assert.Fail("handler called twice");
            handlerCalled = true;
            Assert.Multiple(() =>
            {
                Assert.That(caller, Is.EqualTo(systemUnderTest));
                Assert.That(changedEventArgs.PropertyName, Is.EqualTo(nameof(systemUnderTest.LearningWorlds)));
            });
        };
        
        systemUnderTest.AddLearningWorld(viewModel);
        
        Assert.That(handlerCalled, Is.True);
    }

    [Test]
    public void AuthoringToolWorkspaceViewModel_RemoveLearningWorld_RemovesLearningWorldFromEnumerable()
    {
        var viewModel = GetLearningWorldViewModelForTesting();

        IAuthoringToolWorkspaceViewModel systemUnderTest = GetViewModelForTesting();
        systemUnderTest.AddLearningWorld(viewModel);
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningWorlds.Count(), Is.EqualTo(1));
            Assert.That(systemUnderTest.LearningWorlds, Contains.Item(viewModel));
        });
        systemUnderTest.RemoveLearningWorld(viewModel);
        
        Assert.That(systemUnderTest.LearningWorlds, Is.Empty);
    }
    
    [Test]
    public void AuthoringToolWorkspaceViewModel_RemoveLearningWorld_RaisesStateChangeEventWithCurrentState()
    {
        var viewModel = GetLearningWorldViewModelForTesting();
        var handlerCalled = false;

        IAuthoringToolWorkspaceViewModel systemUnderTest = GetViewModelForTesting();
        systemUnderTest.AddLearningWorld(viewModel);
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningWorlds.Count(), Is.EqualTo(1));
            Assert.That(systemUnderTest.LearningWorlds, Contains.Item(viewModel));
        });
        
        systemUnderTest.PropertyChanged += (caller, changedEventArgs) => {
            if (handlerCalled) Assert.Fail("handler called twice");
            handlerCalled = true;
            Assert.Multiple(() =>
            {
                Assert.That(caller, Is.EqualTo(systemUnderTest));
                Assert.That(changedEventArgs.PropertyName, Is.EqualTo(nameof(systemUnderTest.LearningWorlds)));
            });
        };
        
        systemUnderTest.RemoveLearningWorld(viewModel);
        
        Assert.That(handlerCalled, Is.True);
    }

    [Test]
    public void AuthoringToolWorkspaceViewModel_SetSelectedLearningWorld_SetsLearningWorld()
    {
        var viewModel = GetLearningWorldViewModelForTesting();

        IAuthoringToolWorkspaceViewModel systemUnderTest = GetViewModelForTesting();
        systemUnderTest.AddLearningWorld(viewModel);
        
        Assert.That(systemUnderTest.SelectedLearningWorld, Is.Null);

        systemUnderTest.SelectedLearningWorld = viewModel;
        
        Assert.That(systemUnderTest.SelectedLearningWorld, Is.EqualTo(viewModel));
    }

    [Test]
    public void AuthoringToolWorkspaceViewModel_SetSelectedLearningWorld_ThrowsExceptionWhenNotInCollection()
    {
        var viewModel = GetLearningWorldViewModelForTesting();

        IAuthoringToolWorkspaceViewModel systemUnderTest = GetViewModelForTesting();
        Assert.That(systemUnderTest.LearningWorlds, Is.Empty);
        
        var ex = Assert.Throws<ArgumentException>(() => systemUnderTest.SelectedLearningWorld = viewModel);
        Assert.That(ex!.Message, Is.EqualTo("value isn't contained in collection."));
    }

    [Test]
    public void AuthoringToolWorkspaceViewModel_SetSelectedLearningWorld_RaisesStateChangeEvent()
    {
        var viewModel = GetLearningWorldViewModelForTesting();
        var handlerCalled = false;

        IAuthoringToolWorkspaceViewModel systemUnderTest = GetViewModelForTesting();
        systemUnderTest.AddLearningWorld(viewModel);
        
        systemUnderTest.PropertyChanged += (caller, changedEventArgs) => {
            if (handlerCalled) Assert.Fail("handler called twice");
            handlerCalled = true;
            Assert.Multiple(() =>
            {
                Assert.That(caller, Is.EqualTo(systemUnderTest));
                Assert.That(changedEventArgs.PropertyName, Is.EqualTo(nameof(systemUnderTest.SelectedLearningWorld)));
            });
        };
        
        systemUnderTest.SelectedLearningWorld = viewModel;
        
        Assert.That(handlerCalled, Is.True);
    }
    
    [Test]
    public void AuthoringToolWorkspaceViewModel_SetEditDialogInitialValues_SetsDictionary()
    {
        var dictionary = new Dictionary<string, string> { { "foo", "bar" }, { "bar", "baz" } };

        IAuthoringToolWorkspaceViewModel systemUnderTest = GetViewModelForTesting();
        Assert.That(systemUnderTest.EditDialogInitialValues, Is.Null);
        
        systemUnderTest.EditDialogInitialValues = dictionary;
        
        Assert.That(systemUnderTest.EditDialogInitialValues, Is.EqualTo(dictionary));
    }

    [Test]
    public void AuthoringToolWorkspaceViewModel_SetEditDialogInitialValues_RaisesStateChangeEvent()
    {
        var dictionary = new Dictionary<string, string> { { "foo", "bar" }, { "bar", "baz" } };
        var handlerCalled = false;

        IAuthoringToolWorkspaceViewModel systemUnderTest = GetViewModelForTesting();

        systemUnderTest.PropertyChanged += (caller, changedEventArgs) => {
            if (handlerCalled) Assert.Fail("handler called twice");
            handlerCalled = true;
            Assert.Multiple(() =>
            {
                Assert.That(caller, Is.EqualTo(systemUnderTest));
                Assert.That(changedEventArgs.PropertyName, Is.EqualTo(nameof(systemUnderTest.EditDialogInitialValues)));
            });
        };

        systemUnderTest.EditDialogInitialValues = dictionary;
        
        Assert.That(handlerCalled, Is.True);
    }
    
    private AuthoringToolWorkspaceViewModel GetViewModelForTesting()
    {
        return new AuthoringToolWorkspaceViewModel();
    }

    private LearningWorldViewModel GetLearningWorldViewModelForTesting()
    {
        return new LearningWorldViewModel("foo", "bar", "foo", "bar", "foo", "bar");
    }
}