using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    [Test]
    public void AuthoringToolWorkspacePresenter_CreateNewLearningWorld_EventHandlerCalled()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = CreateLearningWorldPresenter();
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
        var worldPresenter = CreateLearningWorldPresenter();

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter: worldPresenter);

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
        var worldPresenter = CreateLearningWorldPresenter();
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
        var worldPresenter = CreateLearningWorldPresenter();
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
        var worldPresenter = CreateLearningWorldPresenter();
        Assert.That(workspaceVm.LearningWorlds, Is.Empty);
        
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);
        
        var ex = Assert.Throws<ArgumentException>(() => systemUnderTest.SetSelectedLearningWorld("foo"));
        Assert.That(ex!.Message, Is.EqualTo("no world with that name in viewmodel"));
    }
    
    [Test]
    public void AuthoringToolWorkspacePresenter_DeleteSelectedLearningWorld_EventHandlerCalled()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = CreateLearningWorldPresenter();
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
        var worldPresenter = CreateLearningWorldPresenter();
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
        var worldPresenter = CreateLearningWorldPresenter();
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
        var worldPresenter = CreateLearningWorldPresenter();

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter: worldPresenter);

        Assert.DoesNotThrow(systemUnderTest.DeleteSelectedLearningWorld);
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_EditCurrentLearningWorld_EventHandlerCalled()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = CreateLearningWorldPresenter();
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
        var worldPresenter = CreateLearningWorldPresenter();
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
        var worldPresenter = CreateLearningWorldPresenter();

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter: worldPresenter);
        Assert.That(workspaceVm.SelectedLearningWorld, Is.Null);

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.EditSelectedLearningWorld("foo",
            "bar", "this", "does", "not", "matter"));
        Assert.That(ex!.Message, Is.EqualTo("SelectedLearningWorld is null"));
    }

    [Test]
    public async Task AuthoringToolWorkspacePresenter_SaveLearningWorldAsync_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var learningWorld = new LearningWorldViewModel("fo", "f", "", "f", "", "");

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);

        await systemUnderTest.SaveLearningWorldAsync(learningWorld);
        await presentationLogic.Received().SaveLearningWorldAsync(learningWorld);
    }

    [Test]
    public async Task AuthoringToolWorkspacePresenter_SaveSelectedLearningWorldAsync_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var learningWorld = new LearningWorldViewModel("fo", "f", "", "f", "", "");
        var viewModel = new AuthoringToolWorkspaceViewModel();
        viewModel.LearningWorlds.Add(learningWorld);
        viewModel.SelectedLearningWorld = learningWorld;

        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic: presentationLogic, authoringToolWorkspaceVm: viewModel);

        await systemUnderTest.SaveSelectedLearningWorldAsync();
        await presentationLogic.Received().SaveLearningWorldAsync(learningWorld);
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_SaveSelectedLearningWorldAsync_ThrowsIfSelectedWorldNull()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var viewModel = new AuthoringToolWorkspaceViewModel();

        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic: presentationLogic, authoringToolWorkspaceVm: viewModel);

        Assert.ThrowsAsync<ApplicationException>(async () => await systemUnderTest.SaveSelectedLearningWorldAsync());
        
    }

    private AuthoringToolWorkspacePresenter CreatePresenterForTesting(
        IAuthoringToolWorkspaceViewModel? authoringToolWorkspaceVm = null, IPresentationLogic? presentationLogic = null,
        ILearningWorldPresenter? learningWorldPresenter = null, ILearningSpacePresenter? learningSpacePresenter = null,
        ILearningElementPresenter? learningElementPresenter = null,
        ILogger<AuthoringToolWorkspacePresenter>? logger = null,
        IShutdownManager? shutdownManager = null)
    {
        authoringToolWorkspaceVm ??= Substitute.For<IAuthoringToolWorkspaceViewModel>();
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        learningWorldPresenter ??= Substitute.For<ILearningWorldPresenter>();
        learningSpacePresenter ??= Substitute.For<ILearningSpacePresenter>();
        learningElementPresenter ??= Substitute.For<ILearningElementPresenter>();
        logger ??= Substitute.For<ILogger<AuthoringToolWorkspacePresenter>>();
        shutdownManager ??= Substitute.For<IShutdownManager>();
        return new AuthoringToolWorkspacePresenter(authoringToolWorkspaceVm, presentationLogic, learningWorldPresenter,
            learningSpacePresenter, learningElementPresenter, logger, shutdownManager);
    }

    private LearningWorldPresenter CreateLearningWorldPresenter(IPresentationLogic? presentationLogic = null,
        ILearningSpacePresenter? learningSpacePresenter = null,
        ILearningElementPresenter? learningElementPresenter = null,
        ILogger<LearningWorldPresenter>? logger = null)
    {
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        learningSpacePresenter ??= Substitute.For<ILearningSpacePresenter>();
        learningElementPresenter ??= Substitute.For<ILearningElementPresenter>();
        logger ??= Substitute.For<ILogger<LearningWorldPresenter>>();
        return new LearningWorldPresenter(presentationLogic, learningSpacePresenter, learningElementPresenter, logger);
    }
}