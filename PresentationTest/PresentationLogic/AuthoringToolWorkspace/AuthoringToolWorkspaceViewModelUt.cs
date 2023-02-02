using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.World;

namespace PresentationTest.PresentationLogic.AuthoringToolWorkspace;

[TestFixture]
public class AuthoringToolWorkspaceViewModelUt
{
    [Test]
    public void AuthoringToolWorkspaceViewModel_Constructor_EnumerableStartsEmpty()
    {
        IAuthoringToolWorkspaceViewModel systemUnderTest = GetViewModelForTesting();
        
        Assert.That(systemUnderTest.Worlds, Is.Empty);
    }
    
    [Test]
    public void AuthoringToolWorkspaceViewModel_RemoveWorld_RemovesWorldFromEnumerable()
    {
        var viewModel = GetWorldViewModelForTesting();

        var systemUnderTest = GetViewModelForTesting();
        systemUnderTest._worlds.Add(viewModel);
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Worlds.Count(), Is.EqualTo(1));
            Assert.That(systemUnderTest.Worlds, Contains.Item(viewModel));
        });
        systemUnderTest.RemoveWorld(viewModel);
        
        Assert.That(systemUnderTest.Worlds, Is.Empty);
    }
    
    [Test]
    public void AuthoringToolWorkspaceViewModel_RemoveWorld_RaisesStateChangeEventWithCurrentState()
    {
        var viewModel = GetWorldViewModelForTesting();
        var handlerCalled = false;

        var systemUnderTest = GetViewModelForTesting();
        systemUnderTest._worlds.Add(viewModel);
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Worlds.Count(), Is.EqualTo(1));
            Assert.That(systemUnderTest.Worlds, Contains.Item(viewModel));
        });
        
        systemUnderTest.PropertyChanged += (caller, changedEventArgs) => {
            if (handlerCalled) Assert.Fail("handler called twice");
            handlerCalled = true;
            Assert.Multiple(() =>
            {
                Assert.That(caller, Is.EqualTo(systemUnderTest));
                Assert.That(changedEventArgs.PropertyName, Is.EqualTo(nameof(systemUnderTest.Worlds)));
            });
        };
        
        systemUnderTest.RemoveWorld(viewModel);
        
        Assert.That(handlerCalled, Is.True);
    }

    [Test]
    public void AuthoringToolWorkspaceViewModel_SetSelectedWorld_SetsWorld()
    {
        var viewModel = GetWorldViewModelForTesting();

        var systemUnderTest = GetViewModelForTesting();
        systemUnderTest._worlds.Add(viewModel);
        
        Assert.That(systemUnderTest.SelectedWorld, Is.Null);

        systemUnderTest.SelectedWorld = viewModel;
        
        Assert.That(systemUnderTest.SelectedWorld, Is.EqualTo(viewModel));
    }

    [Test]
    public void AuthoringToolWorkspaceViewModel_SetSelectedWorld_ThrowsExceptionWhenNotInCollection()
    {
        var viewModel = GetWorldViewModelForTesting();

        IAuthoringToolWorkspaceViewModel systemUnderTest = GetViewModelForTesting();
        Assert.That(systemUnderTest.Worlds, Is.Empty);
        
        var ex = Assert.Throws<ArgumentException>(() => systemUnderTest.SelectedWorld = viewModel);
        Assert.That(ex!.Message, Is.EqualTo("value isn't contained in collection."));
    }

    [Test]
    public void AuthoringToolWorkspaceViewModel_SetSelectedWorld_RaisesStateChangeEvent()
    {
        var viewModel = GetWorldViewModelForTesting();
        var handlerCalled = false;

        var systemUnderTest = GetViewModelForTesting();
        systemUnderTest._worlds.Add(viewModel);
        
        systemUnderTest.PropertyChanged += (caller, changedEventArgs) => {
            if (handlerCalled) Assert.Fail("handler called twice");
            handlerCalled = true;
            Assert.Multiple(() =>
            {
                Assert.That(caller, Is.EqualTo(systemUnderTest));
                Assert.That(changedEventArgs.PropertyName, Is.EqualTo(nameof(systemUnderTest.SelectedWorld)));
            });
        };
        
        systemUnderTest.SelectedWorld = viewModel;
        
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

    private WorldViewModel GetWorldViewModelForTesting()
    {
        return new WorldViewModel("foo", "bar", "foo", "bar", "foo", "bar");
    }
}