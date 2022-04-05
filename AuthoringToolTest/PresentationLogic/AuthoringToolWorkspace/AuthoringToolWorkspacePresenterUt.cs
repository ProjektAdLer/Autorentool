using System;
using System.Collections.Generic;
using System.Linq;
using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic;
using AuthoringTool.PresentationLogic.API;
using AuthoringTool.PresentationLogic.AuthoringToolWorkspace;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.PresentationLogic.AuthoringToolWorkspace;

[TestFixture]
public class AuthoringToolWorkspacePresenterUt
{
    [Test]
    public void AuthoringToolWorkspacePresenter_StandardConstructor_AllPropertiesInitialized()
    {
        var workspaceVm = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        var worldPresenter = Substitute.For<ILearningWorldPresenter>();
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.CreateLearningSpaceDialogueOpen, Is.EqualTo(false));
            Assert.That(systemUnderTest.CreateLearningWorldDialogOpen, Is.EqualTo(false));
            Assert.That(systemUnderTest.EditLearningSpaceDialogOpen, Is.EqualTo(false));
            Assert.That(systemUnderTest.EditLearningWorldDialogOpen, Is.EqualTo(false));
        });
    }

    #region LearningWorld

    

    [Test]
    public void AuthoringToolWorkspacePresenter_CreateNewLearningWorld_EventHandlerCalled()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = new LearningWorldPresenter();
        var callbackCalled = false;
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);

        systemUnderTest.OnLearningWorldCreate += (_, world) => {
            callbackCalled = true;
            Assert.That(world, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(world!.Name, Is.EqualTo("foo"));
                Assert.That(world.Shortname, Is.EqualTo("f"));
                Assert.That(world.Authors, Is.EqualTo("bar"));
                Assert.That(world.Language, Is.EqualTo("de"));
                Assert.That(world.Description, Is.EqualTo("A test"));
                Assert.That(world.Goals, Is.EqualTo("testing"));
            });
        };
        systemUnderTest.CreateNewLearningWorld("foo", "f", "bar", "de", "A test", "testing");
        Assert.That(callbackCalled, Is.EqualTo(true));
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_CreateNewLearningWorld_CallsLearningWorldPresenter()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = Substitute.For<ILearningWorldPresenter>();
        worldPresenter.CreateNewLearningWorld(Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
            .Returns(new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo",
                "Foo", "Foo"));

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);
        
        systemUnderTest.CreateNewLearningWorld("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        
        worldPresenter.Received()
            .CreateNewLearningWorld(Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
    }
    
    [Test]
    public void AuthoringToolWorkspacePresenter_CreateNewLearningWorld_AddsWorldToWorkspaceViewModel()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = new LearningWorldPresenter();

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);
        
        Assert.That(workspaceVm.LearningWorlds, Is.Empty);
        systemUnderTest.CreateNewLearningWorld("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        Assert.That(workspaceVm.LearningWorlds, Has.Count.EqualTo(1));
        var learningWorldViewModel = workspaceVm.LearningWorlds.First();
        Assert.Multiple(() =>
        {
            Assert.That(learningWorldViewModel.Name, Is.EqualTo("Foo"));
            Assert.That(learningWorldViewModel.Shortname, Is.EqualTo("Foo"));
            Assert.That(learningWorldViewModel.Authors, Is.EqualTo("Foo"));
            Assert.That(learningWorldViewModel.Language, Is.EqualTo("Foo"));
            Assert.That(learningWorldViewModel.Description, Is.EqualTo("Foo"));
            Assert.That(learningWorldViewModel.Goals, Is.EqualTo("Foo"));
        });
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_ChangeSelectedLearningWorld_EventHandlerCalled()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = new LearningWorldPresenter();
        var firstCallbackCalled = false;
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);

        EventHandler<LearningWorldViewModel?> firstCallback = (_, world) => {
            firstCallbackCalled = true;
            Assert.That(world, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(world!.Name, Is.EqualTo("tetete"));
                Assert.That(world.Shortname, Is.EqualTo("f"));
                Assert.That(world.Authors, Is.EqualTo("bar"));
                Assert.That(world.Language, Is.EqualTo("de"));
                Assert.That(world.Description, Is.EqualTo("A test"));
                Assert.That(world.Goals, Is.EqualTo("testing"));
            });
        };
        
        systemUnderTest.OnLearningWorldSelect += firstCallback;
        
        systemUnderTest.CreateNewLearningWorld("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        systemUnderTest.CreateNewLearningWorld("tetete", "f", "bar", "de", "A test",
            "testing");
        systemUnderTest.SetSelectedLearningWorld("tetete");
        
        Assert.That(firstCallbackCalled, Is.EqualTo(true));
        firstCallbackCalled = false;
        systemUnderTest.OnLearningWorldSelect -= firstCallback;
        
        var secondCallbackCalled = false;
        systemUnderTest.OnLearningWorldSelect += (_, world) => {
            secondCallbackCalled = true;
            Assert.That(world, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(world!.Name, Is.EqualTo("Foo"));
                Assert.That(world.Shortname, Is.EqualTo("Foo"));
                Assert.That(world.Authors, Is.EqualTo("Foo"));
                Assert.That(world.Language, Is.EqualTo("Foo"));
                Assert.That(world.Description, Is.EqualTo("Foo"));
                Assert.That(world.Goals, Is.EqualTo("Foo"));
            });
        };
        
        systemUnderTest.SetSelectedLearningWorld("Foo");
        Assert.That(secondCallbackCalled, Is.EqualTo(true));
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_ChangeSelectedLearningWorld_MutatesSelectionInWorkspaceViewModel()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = new LearningWorldPresenter();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        var world2 = new LearningWorldViewModel("tetete", "f", "bar", "de", "A test",
            "testing");
        workspaceVm.LearningWorlds.Add(world1);
        workspaceVm.LearningWorlds.Add(world2);
        

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);
        
        Assert.That(workspaceVm.SelectedLearningWorld, Is.Null);
        
        systemUnderTest.SetSelectedLearningWorld("tetete");
        Assert.That(workspaceVm.SelectedLearningWorld, Is.EqualTo(world2));
        
        systemUnderTest.SetSelectedLearningWorld("Foo");
        Assert.That(workspaceVm.SelectedLearningWorld, Is.EqualTo(world1));
    }
    
    [Test]
    public void AuthoringToolWorkspacePresenter_ChangeSelectedLearningWorld_ThrowsIfNoLearningWorldWithName() 
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = new LearningWorldPresenter();
        Assert.That(workspaceVm.LearningWorlds, Is.Empty);
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);
        
        var ex = Assert.Throws<ArgumentException>(() => systemUnderTest.SetSelectedLearningWorld("foo"));
        Assert.That(ex!.Message, Is.EqualTo("no world with that name in viewmodel"));
    }
    
    [Test]
    public void AuthoringToolWorkspacePresenter_DeleteSelectedLearningWorld_EventHandlerCalled()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = new LearningWorldPresenter();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        var world2 = new LearningWorldViewModel("tetete", "f", "bar", "de", "A test",
            "testing");
        workspaceVm.LearningWorlds.Add(world1);
        workspaceVm.LearningWorlds.Add(world2);
        EventHandler<LearningWorldViewModel?> firstCallback = (_, _) =>
        {
            Assert.Fail("first callback with null as selected world should not have been called");
        };
        var secondCallbackCalled = false;
        EventHandler<LearningWorldViewModel?> secondCallback = (_, world) =>
        {
            secondCallbackCalled = true;
            Assert.That(world, Is.EqualTo(world1));
        };
        var thirdCallbackCalled = false;
        EventHandler<LearningWorldViewModel?> thirdCallback = (_, world) =>
        {
            thirdCallbackCalled = true;
            Assert.That(world, Is.EqualTo(world2));
        };
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);
        
        systemUnderTest.OnLearningWorldDelete += firstCallback;
         
        systemUnderTest.DeleteSelectedLearningWorld();

        systemUnderTest.OnLearningWorldDelete -= firstCallback;
        systemUnderTest.OnLearningWorldDelete += secondCallback;
        
        systemUnderTest.SetSelectedLearningWorld(world1.Name);
        systemUnderTest.DeleteSelectedLearningWorld();
        
        Assert.That(secondCallbackCalled, Is.True);
        systemUnderTest.OnLearningWorldDelete -= secondCallback;
        systemUnderTest.OnLearningWorldDelete += thirdCallback;
        
        systemUnderTest.SetSelectedLearningWorld(world2.Name);
        systemUnderTest.DeleteSelectedLearningWorld();
        
        Assert.That(thirdCallbackCalled, Is.True);
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_DeleteSelectedLearningWorld_DeletesWorldFromViewModel()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = new LearningWorldPresenter();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        var world2 = new LearningWorldViewModel("tetete", "f", "bar", "de", "A test",
            "testing");
        workspaceVm.LearningWorlds.Add(world1);
        workspaceVm.LearningWorlds.Add(world2);
        
        Assert.That(workspaceVm.LearningWorlds, Has.Count.EqualTo(2));
        Assert.That(workspaceVm.LearningWorlds, Does.Contain(world1));
        Assert.That(workspaceVm.LearningWorlds, Does.Contain(world2));

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);
        
        systemUnderTest.DeleteSelectedLearningWorld();
        Assert.That(workspaceVm.LearningWorlds.Count, Is.EqualTo(2));
        Assert.That(workspaceVm.LearningWorlds, Does.Contain(world1));
        Assert.That(workspaceVm.LearningWorlds, Does.Contain(world2));
        
        systemUnderTest.SetSelectedLearningWorld(world1.Name);
        systemUnderTest.DeleteSelectedLearningWorld();
        Assert.That(workspaceVm.LearningWorlds, Has.Count.EqualTo(1));
        Assert.That(workspaceVm.LearningWorlds, Does.Contain(world2));
        
        systemUnderTest.SetSelectedLearningWorld(world2.Name);
        systemUnderTest.DeleteSelectedLearningWorld();
        Assert.That(workspaceVm.LearningWorlds, Is.Empty);
    }
    
    [Test]
    public void AuthoringToolWorkspacePresenter_DeleteSelectedLearningWorld_MutatesSelectionInWorkspaceViewModel()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = new LearningWorldPresenter();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        var world2 = new LearningWorldViewModel("tetete", "f", "bar", "de", "A test",
            "testing");
        workspaceVm.LearningWorlds.Add(world1);
        workspaceVm.LearningWorlds.Add(world2);
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);
        
        systemUnderTest.SetSelectedLearningWorld(world2.Name);
        Assert.That(workspaceVm.SelectedLearningWorld, Is.EqualTo(world2));
        
        systemUnderTest.DeleteSelectedLearningWorld();
        
        Assert.That(workspaceVm.SelectedLearningWorld, Is.EqualTo(world1));
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_DeleteSelectedLearningWorld_DoesNotThrowWhenSelectedWorldNull()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = new LearningWorldPresenter();

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);
        
        Assert.DoesNotThrow(systemUnderTest.DeleteSelectedLearningWorld);
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_EditCurrentLearningWorld_EventHandlerCalled()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = new LearningWorldPresenter();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        workspaceVm.LearningWorlds.Add(world1);
        var callbackCalled = false;
        EventHandler<LearningWorldViewModel?> callback = (_, world) =>
        {
            callbackCalled = true;
            Assert.That(world, Is.Not.Null);
            Assert.That(world, Is.EqualTo(world1));
        };
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);
        systemUnderTest.OnLearningWorldEdit += callback;
        
        systemUnderTest.SetSelectedLearningWorld(world1.Name);
        
        systemUnderTest.EditSelectedLearningWorld("Name", "Shortname", "Authors", "Language",
            "Description", "Goals");
        
        Assert.That(callbackCalled, Is.True);
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_EditCurrentLearningWorld_CallsLearningWorldPresenter()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = Substitute.For<ILearningWorldPresenter>();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        worldPresenter.EditLearningWorld(world1, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(world1);
        workspaceVm.LearningWorlds.Add(world1);
        workspaceVm.SelectedLearningWorld = world1;
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);

        systemUnderTest.EditSelectedLearningWorld("a", "a", "a", "a", "a", "a");

        worldPresenter.Received().EditLearningWorld(world1, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
    }
    
    [Test]
    public void AuthoringToolWorkspacePresenter_EditCurrentLearningWorld_SelectedWorldChanged()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = new LearningWorldPresenter();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        var world2 = new LearningWorldViewModel("tetete", "f", "bar", "de", "A test",
            "testing");
        workspaceVm.LearningWorlds.Add(world1);
        workspaceVm.LearningWorlds.Add(world2);
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);
        
        systemUnderTest.SetSelectedLearningWorld(world1.Name);
        
        systemUnderTest.EditSelectedLearningWorld("Name", "Shortname", "Authors", "Language",
            "Description", "Goals");
        Assert.Multiple(() =>
        {
            Assert.That(world1.Name, Is.EqualTo("Name"));
            Assert.That(world1.Shortname, Is.EqualTo("Shortname"));
            Assert.That(world1.Authors, Is.EqualTo("Authors"));
            Assert.That(world1.Language, Is.EqualTo("Language"));
            Assert.That(world1.Description, Is.EqualTo("Description"));
            Assert.That(world1.Goals, Is.EqualTo("Goals"));
        });
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_EditCurrentLearningWorld_ThrowsWhenSelectedWorldNull()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = new LearningWorldPresenter();
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);
        Assert.That(workspaceVm.SelectedLearningWorld, Is.Null);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.EditSelectedLearningWorld("foo",
            "bar", "this", "does", "not", "matter"));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    #endregion

    #region LearningObject

    #region CreateNewLearningSpace/Element

    [Test]
    public void AuthoringToolWorkspacePresenter_CreateNewLearningSpace_ThrowsWhenSelectedWorldNull()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var learningSpacePresenter = Substitute.For<ILearningSpacePresenter>();
        learningSpacePresenter.CreateNewLearningSpace(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(
            new LearningSpaceViewModel("foo", "bar", "foo", "bar", "baz"));

        var systemUnderTest = CreatePresenterForTesting(workspaceVm,
            learningSpacePresenter: learningSpacePresenter);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.CreateNewLearningSpace("foo",
            "this", "does", "not", "matter"));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }



    [Test]
    public void AuthoringToolWorkspacePresenter_CreateNewLearningElement_ThrowsWhenSelectedWorldNull()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewLearningElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(),Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), 
            Arg.Any<string>()).Returns(new LearningElementViewModel("foo", "bar",
            "Transfer", "Video","bar","foo", "foo", "bar", "foo"));

        var systemUnderTest = CreatePresenterForTesting(workspaceVm,
            learningElementPresenter: learningElementPresenter);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.CreateNewLearningElement("foo",
            "bar", "foo","bar","foo", "bar", "foo", "bar",
            "foo"));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }
    
    
    [Test]
    public void AuthoringToolWorkspacePresenter_CreateNewLearningSpace_CallsLearningSpacePresenter()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        workspaceVm.LearningWorlds.Add(world);
        workspaceVm.SelectedLearningWorld = world;
        var learningSpacePresenter = Substitute.For<ILearningSpacePresenter>();
        learningSpacePresenter.CreateNewLearningSpace(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(
            new LearningSpaceViewModel("foo", "bar", "foo", "bar", "baz"));

        var systemUnderTest = CreatePresenterForTesting(workspaceVm,
            learningSpacePresenter: learningSpacePresenter);

        systemUnderTest.CreateNewLearningSpace("foo", "bar", "foo", "bar", "foo");

        learningSpacePresenter.Received().CreateNewLearningSpace(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
    }
    
    [Test]
    public void AuthoringToolWorkspacePresenter_CreateNewLearningElement_CallsLearningElementPresenter()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        workspaceVm.LearningWorlds.Add(world);
        workspaceVm.SelectedLearningWorld = world;
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.CreateNewLearningElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(),Arg.Any<string>(),Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>()).Returns(new LearningElementViewModel("foo", "bar","foo",
            "foo", "bar", "foo", "bar", "foo", "bar"));

        var systemUnderTest = CreatePresenterForTesting(workspaceVm,
            learningElementPresenter:learningElementPresenter);
        
        systemUnderTest.CreateNewLearningElement("foo", "bar", "foo", "bar", "foo",
            "bar","foo", "foo", "bar");

        learningElementPresenter.Received().CreateNewLearningElement(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(),Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(),Arg.Any<string>(), Arg.Any<string>());
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_CreateNewLearningSpace_AddsLearningSpaceToViewModel()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var learningSpacePresenter = new LearningSpacePresenter();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        workspaceVm.LearningWorlds.Add(world);

        var systemUnderTest = CreatePresenterForTesting(workspaceVm,
            learningSpacePresenter: learningSpacePresenter);

        Assert.IsEmpty(world.LearningSpaces);

        workspaceVm.SelectedLearningWorld = world;
        systemUnderTest.CreateNewLearningSpace("foo", "bar", "foo", "bar", "foo");

        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));
        var space = world.LearningSpaces.First();
        Assert.Multiple(() =>
        {
            Assert.That(space.Name, Is.EqualTo("foo"));
            Assert.That(space.Shortname, Is.EqualTo("bar"));
            Assert.That(space.Authors, Is.EqualTo("foo"));
            Assert.That(space.Description, Is.EqualTo("bar"));
            Assert.That(space.Goals, Is.EqualTo("foo"));
        });
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_CreateNewLearningElement_AddsLearningElementToViewModel()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var learningElementPresenter = new LearningElementPresenter();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        workspaceVm.LearningWorlds.Add(world);

        var systemUnderTest = CreatePresenterForTesting(workspaceVm,
            learningElementPresenter: learningElementPresenter);

        Assert.That(world.LearningElements, Is.Empty);

        workspaceVm.SelectedLearningWorld = world;
        systemUnderTest.CreateNewLearningElement("foo", "bar", "foo", "bar",
            "foo", "bar", "foo", "bar", "foo");

        Assert.That(world.LearningElements, Has.Count.EqualTo(1));
        var element = world.LearningElements.First();
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo("foo"));
            Assert.That(element.Shortname, Is.EqualTo("bar"));
            Assert.That(element.Type, Is.EqualTo("foo"));
            Assert.That(element.Content, Is.EqualTo("bar"));
            Assert.That(element.Authors, Is.EqualTo("foo"));
            Assert.That(element.Description, Is.EqualTo("bar"));
            Assert.That(element.Goals, Is.EqualTo("foo"));
        });
    }

    #endregion

    #region OnEditSpace/ElementDalogClose

    [Test]
    public void AuthoringToolWorkspacePresenter_OnEditSpaceDialogClose_CallsLearningSpacePresenter()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var learningSpacePresenter = Substitute.For<ILearningSpacePresenter>();
        learningSpacePresenter.EditLearningSpace(Arg.Any<LearningSpaceViewModel>(), Arg.Any<string>(),
                Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
            .Returns(new LearningSpaceViewModel("ba", "ba", "ba", "ba", "ba"));
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        workspaceVm.LearningWorlds.Add(world);
        var space = new LearningSpaceViewModel("foo", "bar", "foo", "bar", "foo");
        world.LearningSpaces.Add(space);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "n";
        dictionary["Shortname"] = "sn";
        dictionary["Description"] = "d";
        dictionary["Authors"] = "a";
        dictionary["Goals"] = "g";
        var returnValueTuple =
            new Tuple<ModalDialogReturnValue, IDictionary<string, string>?>(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(workspaceVm,
            learningSpacePresenter: learningSpacePresenter);
        workspaceVm.SelectedLearningWorld = world;
        world.SelectedLearningObject = space;


        systemUnderTest.OnEditSpaceDialogClose(returnValueTuple);

        learningSpacePresenter.Received().EditLearningSpace(space, "n", "sn", "a", "d", "g");
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_OnEditElementDialogClose_CallsLearningElementPresenter()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var learningElementPresenter = Substitute.For<ILearningElementPresenter>();
        learningElementPresenter.EditLearningElement(Arg.Any<LearningElementViewModel>(), Arg.Any<string>(),
                Arg.Any<string>(), Arg.Any<string>(),Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>()).Returns(new LearningElementViewModel("ba", "ba",
            "ba", "ba", "ba", "ba", "ba", "ba", "ba"));
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        workspaceVm.LearningWorlds.Add(world);
        var element = new LearningElementViewModel("foo", "bar", "foo", "bar", "foo",
            "bar", "foo","bar", "foo");
        world.LearningElements.Add(element);

        var modalDialogReturnValue = ModalDialogReturnValue.Ok;
        IDictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary["Name"] = "a";
        dictionary["Shortname"] = "b";
        dictionary["Parent"] = "bb";
        dictionary["Assignment"] = "bbb";
        dictionary["Type"] = "c";
        dictionary["Content"] = "d";
        dictionary["Authors"] = "e";
        dictionary["Description"] = "f";
        dictionary["Goals"] = "g";
        var returnValueTuple =
            new Tuple<ModalDialogReturnValue, IDictionary<string, string>?>(modalDialogReturnValue, dictionary);

        var systemUnderTest = CreatePresenterForTesting(workspaceVm,
            learningElementPresenter: learningElementPresenter);
        workspaceVm.SelectedLearningWorld = world;
        world.SelectedLearningObject = element;


        systemUnderTest.OnEditElementDialogClose(returnValueTuple);

        learningElementPresenter.Received().EditLearningElement(element, "a", "b","bb",
            "bbb", "c", "d", "e", "f", "g");
    }

    #endregion

    #region DeleteSelectedLearningObject

    [Test]
    public void AuthoringToolWorkspacePresenter_DeleteSelectedLearningObject_ThrowsWhenSelectedWorldNull()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        workspaceVm.SelectedLearningWorld = null;

        var systemUnderTest = CreatePresenterForTesting(workspaceVm);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.DeleteSelectedLearningObject());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_DeleteSelectedLearningObject_DoesNotThrowWhenSelectedObjectNull()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        workspaceVm.LearningWorlds.Add(world);
        workspaceVm.SelectedLearningWorld = world;

        var systemUnderTest = CreatePresenterForTesting(workspaceVm);
        Assert.Multiple(() =>
        {
            Assert.That(workspaceVm.SelectedLearningWorld.SelectedLearningObject, Is.Null);
            Assert.That(workspaceVm.SelectedLearningWorld.LearningObjects, Is.Empty);
            Assert.DoesNotThrow(() => systemUnderTest.DeleteSelectedLearningObject());
        });
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_DeleteSelectedLearningObject_WithSpace_DeletesSpaceFromViewModel()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        workspaceVm.LearningWorlds.Add(world);
        workspaceVm.SelectedLearningWorld = world;
        var space = new LearningSpaceViewModel("f", "f", "f", "f", "f");
        world.LearningSpaces.Add(space);
        world.SelectedLearningObject = space;

        Assert.That(world.LearningSpaces, Contains.Item(space));
        Assert.That(world.LearningSpaces, Has.Count.EqualTo(1));

        var systemUnderTest = CreatePresenterForTesting(workspaceVm);
        systemUnderTest.DeleteSelectedLearningObject();

        Assert.That(world.LearningSpaces, Is.Empty);
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_DeleteSelectedLearningObject_WithElement_DeletesElementFromViewModel()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        workspaceVm.LearningWorlds.Add(world);
        workspaceVm.SelectedLearningWorld = world;
        var element = new LearningElementViewModel("f", "f", "f", "f", "f", "f", "f", "f", "f");
        world.LearningElements.Add(element);
        world.SelectedLearningObject = element;

        Assert.That(world.LearningElements, Contains.Item(element));
        Assert.That(world.LearningElements, Has.Count.EqualTo(1));

        var systemUnderTest = CreatePresenterForTesting(workspaceVm);
        systemUnderTest.DeleteSelectedLearningObject();

        Assert.That(world.LearningElements, Is.Empty);
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_DeleteSelectedLearningObject_WithUnknownObject_ThrowsNotImplemented()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        workspaceVm.LearningWorlds.Add(world);
        workspaceVm.SelectedLearningWorld = world;
        var learningObject = Substitute.For<ILearningObjectViewModel>();
        world.SelectedLearningObject = learningObject;

        var systemUnderTest = CreatePresenterForTesting(workspaceVm);

        var ex = Assert.Throws<NotImplementedException>(() => systemUnderTest.DeleteSelectedLearningObject());
        Assert.That(ex!.Message, Is.EqualTo("Type of LearningObject is not implemented"));
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_DeleteSelectedLearningObject_WithSpace_MutatesSelectionInViewModel()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        workspaceVm.LearningWorlds.Add(world);
        workspaceVm.SelectedLearningWorld = world;
        var space = new LearningSpaceViewModel("f", "f", "f", "f", "f");
        world.LearningSpaces.Add(space);
        world.SelectedLearningObject = space;

        Assert.That(world.SelectedLearningObject, Is.EqualTo(space));

        var systemUnderTest = CreatePresenterForTesting(workspaceVm);
        systemUnderTest.DeleteSelectedLearningObject();

        Assert.That(world.SelectedLearningObject, Is.Null);
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_DeleteSelectedLearningObject_WithElement_MutatesSelectionInViewModel()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        workspaceVm.LearningWorlds.Add(world);
        workspaceVm.SelectedLearningWorld = world;
        var element = new LearningElementViewModel("f", "f", "f", "f", "f",
            "f", "f","f","f");
        world.LearningElements.Add(element);
        world.SelectedLearningObject = element;

        Assert.That(world.SelectedLearningObject, Is.EqualTo(element));

        var systemUnderTest = CreatePresenterForTesting(workspaceVm);
        systemUnderTest.DeleteSelectedLearningObject();

        Assert.That(world.SelectedLearningObject, Is.Null);
    }

    #endregion

    #region OpenEditSelectedLearningObjectDialog

    [Test]
    public void AuthoringToolWorkspacePresenter_OpenEditSelectedLearningObjectDialog_ThrowsWhenSelectedWorldNull()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        workspaceVm.SelectedLearningWorld = null;

        var systemUnderTest = CreatePresenterForTesting(workspaceVm);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.OpenEditSelectedLearningObjectDialog());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void
        AuthoringToolWorkspacePresenter_OpenEditSelectedLearningObjectDialog_DoesNotThrowWhenSelectedObjectNull()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        workspaceVm.LearningWorlds.Add(world);
        workspaceVm.SelectedLearningWorld = world;

        var systemUnderTest = CreatePresenterForTesting(workspaceVm);
        systemUnderTest.OpenEditSelectedLearningObjectDialog();
        Assert.Multiple(() =>
        {
            Assert.That(workspaceVm.SelectedLearningWorld.SelectedLearningObject, Is.EqualTo(null));
            Assert.That(workspaceVm.SelectedLearningWorld.LearningObjects, Is.Empty);
            Assert.DoesNotThrow(() => systemUnderTest.DeleteSelectedLearningObject());
        });
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_OpenEditSelectedLearningObjectDialog_WithSpace_CallsMethod()
    {
        //TODO
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_OpenEditSelectedLearningObjectDialog_WithElement_CallsMethod()
    {
        //TODO
    }

    [Test]
    public void
        AuthoringToolWorkspacePresenter_OpenEditSelectedLearningObjectDialog_WithUnknownObject_ThrowsNotImplemented()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        workspaceVm.LearningWorlds.Add(world);
        workspaceVm.SelectedLearningWorld = world;
        var learningObject = Substitute.For<ILearningObjectViewModel>();
        world.SelectedLearningObject = learningObject;

        var systemUnderTest = CreatePresenterForTesting(workspaceVm);

        var ex = Assert.Throws<NotImplementedException>(() => systemUnderTest.OpenEditSelectedLearningObjectDialog());
        Assert.That(ex!.Message, Is.EqualTo("Type of LearningObject is not implemented"));
    }

    #endregion

    #region SaveSelectedLearningObject

    [Test]
    public void AuthoringToolWorkspacePresenter_SaveSelectedLearningObject_ThrowsWhenSelectedWorldNull()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        workspaceVm.SelectedLearningWorld = null;

        var systemUnderTest = CreatePresenterForTesting(workspaceVm);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.SaveSelectedLearningObject());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_SaveSelectedLearningObject_DoesNotThrowWhenSelectedObjectNull()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        workspaceVm.LearningWorlds.Add(world);
        workspaceVm.SelectedLearningWorld = world;

        var systemUnderTest = CreatePresenterForTesting(workspaceVm);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.SaveSelectedLearningObject());
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningObject is null"));
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_SaveSelectedLearningObject_WithSpace_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        workspaceVm.LearningWorlds.Add(world);
        workspaceVm.SelectedLearningWorld = world;
        var space = new LearningSpaceViewModel("f", "f", "f", "f", "f");
        world.LearningSpaces.Add(space);
        world.SelectedLearningObject = space;

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, presentationLogic);
        systemUnderTest.SaveSelectedLearningObject();

        presentationLogic.Received().SaveLearningSpace(space);
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_SaveSelectedLearningObject_WithElement_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        workspaceVm.LearningWorlds.Add(world);
        workspaceVm.SelectedLearningWorld = world;
        var element = new LearningElementViewModel("f", "f","f", "f", "f", "f",
            "f", "f", "f");
        world.LearningElements.Add(element);
        world.SelectedLearningObject = element;

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, presentationLogic);
        systemUnderTest.SaveSelectedLearningObject();

        presentationLogic.Received().SaveLearningElement(element);
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_SaveSelectedLearningObject_WithUnknownObject_ThrowsNotImplemented()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var world = new LearningWorldViewModel("foo", "foo", "foo", "foo", "foo",
            "foo");
        workspaceVm.LearningWorlds.Add(world);
        workspaceVm.SelectedLearningWorld = world;
        var learningObject = Substitute.For<ILearningObjectViewModel>();
        world.SelectedLearningObject = learningObject;

        var systemUnderTest = CreatePresenterForTesting(workspaceVm);

        var ex = Assert.Throws<NotImplementedException>(() => systemUnderTest.SaveSelectedLearningObject());
        Assert.That(ex!.Message, Is.EqualTo("Type of LearningObject is not implemented"));
    }

    #endregion

    #endregion

    private AuthoringToolWorkspacePresenter CreatePresenterForTesting(
        IAuthoringToolWorkspaceViewModel? authoringToolWorkspaceVm = null, IPresentationLogic? presentationLogic = null,
        ILearningWorldPresenter? learningWorldPresenter = null, ILearningSpacePresenter? learningSpacePresenter = null,
        ILearningElementPresenter? learningElementPresenter = null,
        ILogger<AuthoringToolWorkspacePresenter>? logger = null)
    {
        authoringToolWorkspaceVm ??= Substitute.For<IAuthoringToolWorkspaceViewModel>();
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        learningWorldPresenter ??= Substitute.For<ILearningWorldPresenter>();
        learningSpacePresenter ??= Substitute.For<ILearningSpacePresenter>();
        learningElementPresenter ??= Substitute.For<ILearningElementPresenter>();
        logger ??= Substitute.For<ILogger<AuthoringToolWorkspacePresenter>>();
        return new AuthoringToolWorkspacePresenter(authoringToolWorkspaceVm, presentationLogic, learningWorldPresenter,
            learningSpacePresenter, learningElementPresenter, logger);
    }
}