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
        Assert.That(workspaceVm.LearningWorlds.Count(), Is.EqualTo(1));
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
        workspaceVm.AddLearningWorld(world1);
        workspaceVm.AddLearningWorld(world2);
        

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
        workspaceVm.AddLearningWorld(world1);
        workspaceVm.AddLearningWorld(world2);
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
    public void AuthoringToolWorkspacePresenter_DeleteSelectedLearningWorld_DeletesWorldFromViewModelAndSetsUnsaved()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = CreateLearningWorldPresenter();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo")
        {
            UnsavedChanges = true
        };
        var world2 = new LearningWorldViewModel("tetete", "f", "bar", "de", "A test",
            "testing");
        workspaceVm.AddLearningWorld(world1);
        workspaceVm.AddLearningWorld(world2);
        
        Assert.That(workspaceVm.LearningWorlds.Count(), Is.EqualTo(2));
        Assert.That(workspaceVm.LearningWorlds, Does.Contain(world1));
        Assert.That(workspaceVm.LearningWorlds, Does.Contain(world2));

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter:worldPresenter);
        
        systemUnderTest.DeleteSelectedLearningWorld();
        Assert.That(workspaceVm.LearningWorlds.Count(), Is.EqualTo(2));
        Assert.That(workspaceVm.LearningWorlds, Does.Contain(world1));
        Assert.That(workspaceVm.LearningWorlds, Does.Contain(world2));
        
        systemUnderTest.SetSelectedLearningWorld(world1.Name);
        systemUnderTest.DeleteSelectedLearningWorld();
        Assert.That(workspaceVm.LearningWorlds.Count(), Is.EqualTo(1));
        Assert.That(workspaceVm.LearningWorlds, Does.Contain(world2));
        Assert.That(systemUnderTest.DeletedUnsavedWorld, Is.EqualTo(world1));
        
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
        workspaceVm.AddLearningWorld(world1);
        workspaceVm.AddLearningWorld(world2);
        
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
        workspaceVm.AddLearningWorld(world1);
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
        workspaceVm.AddLearningWorld(world1);
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
        workspaceVm.AddLearningWorld(world1);
        workspaceVm.AddLearningWorld(world2);
        
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
        viewModel.AddLearningWorld(learningWorld);
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

    [Test]
    public async Task AuthoringToolWorkspacePresenter_LoadLearningWorldAsync_CallsPresentationLogicAndAddsToViewModel()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        presentationLogic.LoadLearningWorldAsync().Returns(learningWorld);
        var viewModel = new AuthoringToolWorkspaceViewModel();

        var systemUnderTest = CreatePresenterForTesting(viewModel, presentationLogic: presentationLogic);

        await systemUnderTest.LoadLearningWorldAsync();
        await presentationLogic.Received().LoadLearningWorldAsync();
        Assert.That(viewModel.LearningWorlds, Contains.Item(learningWorld));
    }

    [Test]
    public async Task
        AuthoringToolWorkspacePresenter_LoadLearningWorldAsync_SetsWorldToReplaceWithWhenWorldWithNameExists()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        var learningWorld2 = new LearningWorldViewModel("f", "fu", "fu", "fu", "fu", "fu");
        presentationLogic.LoadLearningWorldAsync().Returns(learningWorld2);
        var viewModel = new AuthoringToolWorkspaceViewModel();
        viewModel.AddLearningWorld(learningWorld);
        
        var systemUnderTest = CreatePresenterForTesting(viewModel, presentationLogic: presentationLogic);

        await systemUnderTest.LoadLearningWorldAsync();
        Assert.That(systemUnderTest.WorldToReplaceWith, Is.EqualTo(learningWorld2));
    }
    
    [Test]
    public void AuthoringToolWorkspacePresenter_ReplaceLearningWorld_ReplacesWorldAnpdSetsReplacedUnsavedWorld() 
    {
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f")
        {
            UnsavedChanges = true
        };
        var learningWorld2 = new LearningWorldViewModel("f", "fu", "fu", "fu", "fu", "fu");
        var viewModel = new AuthoringToolWorkspaceViewModel();
        viewModel.AddLearningWorld(learningWorld);
        viewModel.SelectedLearningWorld = learningWorld;

        var systemUnderTest = CreatePresenterForTesting(viewModel);
        
        systemUnderTest.ReplaceLearningWorld(learningWorld2);
        Assert.Multiple(() =>
        {
            Assert.That(viewModel.LearningWorlds, Contains.Item(learningWorld2));
            Assert.That(viewModel.LearningWorlds, Does.Not.Contain(learningWorld));
            Assert.That(viewModel.SelectedLearningWorld, Is.EqualTo(learningWorld2));
            Assert.That(systemUnderTest.ReplacedUnsavedWorld, Is.EqualTo(learningWorld));
        });
    }

    [Test]
    public void AuthoringToolWorkspacePresenter_OnBeforeShutdown_CancelsShutdownCreatesQueueAndInvokesViewUpdate()
    {
        var viewModel = new AuthoringToolWorkspaceViewModel();
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        viewModel.AddLearningWorld(learningWorld);
        var args = new BeforeShutdownEventArgs();
        var callbackCalled = false;
        var callback = () => { callbackCalled = true; };

        var systemUnderTest = CreatePresenterForTesting(viewModel);
        systemUnderTest.OnForceViewUpdate += callback;
        
        systemUnderTest.OnBeforeShutdown(null, args);
        Assert.That(systemUnderTest.UnsavedWorldsQueue, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(args.CancelShutdownState, Is.True);
            Assert.That(systemUnderTest.UnsavedWorldsQueue, Contains.Item(learningWorld));
            Assert.That(systemUnderTest.SaveUnsavedChangesDialogOpen, Is.True);
            Assert.That(callbackCalled, Is.True);
        });
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void AuthoringToolWorkspacePresenter_CompletedSaveQueue_DeletesQueueAndRecallsShutdownManager(bool cancelled)
    {
        var shutdownManager = Substitute.For<IShutdownManager>();

        var systemUnderTest = CreatePresenterForTesting(shutdownManager: shutdownManager);
        systemUnderTest.SaveUnsavedChangesDialogOpen = true;
        systemUnderTest.UnsavedWorldsQueue = new Queue<LearningWorldViewModel>();
        
        systemUnderTest.CompletedSaveQueue(cancelled);
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.SaveUnsavedChangesDialogOpen, Is.False);
            Assert.That(systemUnderTest.UnsavedWorldsQueue, Is.Null);
        });
        if (!cancelled)
            shutdownManager.Received().BeginShutdown();
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