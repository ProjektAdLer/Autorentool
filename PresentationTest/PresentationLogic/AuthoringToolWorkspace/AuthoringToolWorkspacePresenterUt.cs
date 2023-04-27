using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Dialogues;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.View;

namespace PresentationTest.PresentationLogic.AuthoringToolWorkspace;

[TestFixture]
public class AuthoringToolWorkspacePresenterUt
{
    private IAuthoringToolWorkspaceViewModel _authoringToolWorkspaceViewModel;

    [Test]
    public void StandardConstructor_AllPropertiesInitialized()
    {
        var workspaceVm = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        var worldPresenter = Substitute.For<ILearningWorldPresenter>();

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter: worldPresenter);
        Assert.Multiple(() => { Assert.That(systemUnderTest.AuthoringToolWorkspaceVm, Is.EqualTo(workspaceVm)); });
    }

    #region CreateLearningWorld

    [Test]
    public void CreateLearningWorld_CallsCreateLearningWorldOnPresentationLogic()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var mediator = Substitute.For<IMediator>();
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, presentationLogic: presentationLogic, mediator: mediator);
        
        systemUnderTest.AuthoringToolWorkspaceVm.LearningWorlds.Add(new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo", "Foo"));

        systemUnderTest.CreateLearningWorld("n", "s", "a", "l", "d", "g");

        presentationLogic.Received(1).CreateLearningWorld(workspaceVm, "n", "s", "a", "l", "d", "g");
    }

    [Test]
    public void CreateLearningWorld_EventHandlerCalled()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var mediator = Substitute.For<IMediator>();
        var worldVm = new LearningWorldViewModel("n", "s", "a", "l", "d", "g");
        presentationLogic.When(x => x.CreateLearningWorld(workspaceVm, Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()))
            .Do(x =>
            {
                workspaceVm._learningWorlds.Add(worldVm);
                mediator.SelectedLearningWorld = worldVm;
            });
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, presentationLogic: presentationLogic, mediator: mediator);
        var callbackCalled = false;
        systemUnderTest.OnLearningWorldCreate += (_, world) =>
        {
            callbackCalled = true;
            Assert.That(world, Is.EqualTo(worldVm));
        };

        systemUnderTest.CreateLearningWorld("n", "s", "a", "l", "d", "g");

        Assert.That(callbackCalled, Is.EqualTo(true));
    }

    #endregion

    #region ChangeSelectedLearningWorld

    [Test]
    public void ChangeSelectedLearningWorld_EventHandlerCalled()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        workspaceVm._learningWorlds.Add(new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo", "Foo"));
        workspaceVm._learningWorlds.Add(new LearningWorldViewModel("tetete", "f", "bar", "de", "A test", "testing"));
        var firstCallbackCalled = false;

        var systemUnderTest = CreatePresenterForTesting(workspaceVm);

        EventHandler<LearningWorldViewModel?> firstCallback = (_, world) =>
        {
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

        systemUnderTest.SetSelectedLearningWorld("tetete");

        Assert.That(firstCallbackCalled, Is.EqualTo(true));
        firstCallbackCalled = false;
        systemUnderTest.OnLearningWorldSelect -= firstCallback;

        var secondCallbackCalled = false;
        systemUnderTest.OnLearningWorldSelect += (_, world) =>
        {
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
                Assert.That(systemUnderTest.LearningWorldSelected, Is.True);
            });
        };

        systemUnderTest.SetSelectedLearningWorld("Foo");
        Assert.That(secondCallbackCalled, Is.EqualTo(true));
    }

    [Test]
    public void ChangeSelectedLearningWorld_MutatesSelectionInWorkspaceViewModel()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var mediator = Substitute.For<IMediator>();
        var worldPresenter = CreateLearningWorldPresenter(mediator: mediator);
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        var world2 = new LearningWorldViewModel("tetete", "f", "bar", "de", "A test",
            "testing");
        workspaceVm._learningWorlds.Add(world1);
        workspaceVm._learningWorlds.Add(world2);


        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter: worldPresenter, mediator: mediator);

        Assert.That(mediator.SelectedLearningWorld, Is.Null);

        systemUnderTest.SetSelectedLearningWorld("tetete");
        Assert.That(mediator.SelectedLearningWorld, Is.EqualTo(world2));

        systemUnderTest.SetSelectedLearningWorld("Foo");
        Assert.That(mediator.SelectedLearningWorld, Is.EqualTo(world1));
    }

    [Test]
    public void ChangeSelectedLearningWorld_ThrowsIfNoLearningWorldWithName()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = CreateLearningWorldPresenter();
        Assert.That(workspaceVm.LearningWorlds, Is.Empty);

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter: worldPresenter);

        var ex = Assert.Throws<ArgumentException>(() => systemUnderTest.SetSelectedLearningWorld("foo"));
        Assert.That(ex!.Message, Is.EqualTo("no world with that name in viewmodel"));
    }

    #endregion

    #region DeleteSelectedLearningWorld

    [Test]
    public void DeleteSelectedLearningWorld_EventHandlerCalled()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = CreateLearningWorldPresenter();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        var world2 = new LearningWorldViewModel("tetete", "f", "bar", "de", "A test",
            "testing");
        workspaceVm._learningWorlds.Add(world1);
        workspaceVm._learningWorlds.Add(world2);
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

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter: worldPresenter);

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
    public void DeleteSelectedLearningWorld_CallsPresentationLogic()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        workspaceVm._learningWorlds.Add(world1);
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, presentationLogic: presentationLogic);

        systemUnderTest.SetSelectedLearningWorld(world1.Name);
        systemUnderTest.DeleteSelectedLearningWorld();

        presentationLogic.Received().DeleteLearningWorld(workspaceVm, world1);
    }

    [Test]
    public void DeleteSelectedLearningWorld_SetsDeletedUnsavedWorld()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var world = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo")
        {
            UnsavedChanges = true
        };
        workspaceVm._learningWorlds.Add(world);

        var systemUnderTest = CreatePresenterForTesting(workspaceVm);
        systemUnderTest.SetSelectedLearningWorld(world);

        systemUnderTest.DeleteSelectedLearningWorld();
        Assert.That(systemUnderTest.DeletedUnsavedWorld, Is.EqualTo(world));
    }

    [Test]
    public void DeleteSelectedLearningWorld_MutatesSelectionInWorkspaceViewModel()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var mediator = Substitute.For<IMediator>();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        var world2 = new LearningWorldViewModel("tetete", "f", "bar", "de", "A test",
            "testing");
        workspaceVm._learningWorlds.Add(world1);
        workspaceVm._learningWorlds.Add(world2);

        var presentationLogic = Substitute.For<IPresentationLogic>();
        presentationLogic
            .When(x => x.DeleteLearningWorld(Arg.Any<IAuthoringToolWorkspaceViewModel>(),
                Arg.Any<LearningWorldViewModel>()))
            .Do(y =>
            {
                workspaceVm.RemoveLearningWorld(mediator.SelectedLearningWorld!);
                mediator.SelectedLearningWorld =
                    ((AuthoringToolWorkspaceViewModel)y.Args()[0]).LearningWorlds.LastOrDefault();
            });

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, presentationLogic: presentationLogic, mediator: mediator);

        systemUnderTest.SetSelectedLearningWorld(world2.Name);
        Assert.That(mediator.SelectedLearningWorld, Is.EqualTo(world2));

        systemUnderTest.DeleteSelectedLearningWorld();

        Assert.That(mediator.SelectedLearningWorld, Is.EqualTo(world1));
    }

    [Test]
    public void DeleteSelectedLearningWorld_DoesNotThrowWhenSelectedWorldNull()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var worldPresenter = CreateLearningWorldPresenter();

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter: worldPresenter);

        Assert.DoesNotThrow(systemUnderTest.DeleteSelectedLearningWorld);
    }

    #endregion


    #region Save(Selected)LearningWorldAsync

    [Test]
    public async Task SaveLearningWorldAsync_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var learningWorld = new LearningWorldViewModel("fo", "f", "", "f", "", "");

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic);

        await systemUnderTest.SaveLearningWorldAsync(learningWorld);
        await presentationLogic.Received().SaveLearningWorldAsync(learningWorld);
    }

    [Test]
    public async Task SaveSelectedLearningWorldAsync_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var mediator = Substitute.For<IMediator>();
        var learningWorld = new LearningWorldViewModel("fo", "f", "", "f", "", "");
        var viewModel = new AuthoringToolWorkspaceViewModel();
        viewModel._learningWorlds.Add(learningWorld);
        mediator.SelectedLearningWorld = learningWorld;

        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic: presentationLogic, authoringToolWorkspaceVm: viewModel, mediator: mediator);

        await systemUnderTest.SaveSelectedLearningWorldAsync();
        await presentationLogic.Received().SaveLearningWorldAsync(learningWorld);
    }

    [Test]
    public void SaveSelectedLearningWorldAsync_ThrowsIfSelectedWorldNull()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var viewModel = new AuthoringToolWorkspaceViewModel();

        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic: presentationLogic, authoringToolWorkspaceVm: viewModel);

        Assert.ThrowsAsync<ApplicationException>(async () => await systemUnderTest.SaveSelectedLearningWorldAsync());
    }

    #endregion

    #region LoadLearningWorldAsync

    [Test]
    public async Task LoadLearningWorldAsync_CallsPresentationLogicAndAddsToViewModel()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var mediator = Substitute.For<IMediator>();
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        presentationLogic
            .When(x => x.LoadLearningWorldAsync(Arg.Any<IAuthoringToolWorkspaceViewModel>()))
            .Do(y =>
            {
                ((AuthoringToolWorkspaceViewModel)y.Args()[0])._learningWorlds.Add(learningWorld);
                mediator.SelectedLearningWorld = learningWorld;
            });
        var viewModel = new AuthoringToolWorkspaceViewModel();

        var systemUnderTest = CreatePresenterForTesting(viewModel, presentationLogic: presentationLogic, mediator: mediator);

        await systemUnderTest.LoadLearningWorldAsync();
        await presentationLogic.Received().LoadLearningWorldAsync(viewModel);
        Assert.That(viewModel.LearningWorlds, Contains.Item(learningWorld));
    }

    #endregion

    #region Shutdown

    [Test]
    public async Task OnBeforeShutdown_CallsDialogService_ForEveryUnsavedWorld_AndCallsSaveOnYesResponse()
    {
        var viewModel = new AuthoringToolWorkspaceViewModel();
        var unsavedWorld = new LearningWorldViewModel("unsaved", "f", "f", "f", "f", "f")
        {
            UnsavedChanges = true
        };
        var savedWorld = new LearningWorldViewModel("unsaved", "f", "f", "f", "f", "f")
        {
            UnsavedChanges = false
        };
        viewModel._learningWorlds.Add(unsavedWorld);
        viewModel._learningWorlds.Add(savedWorld);
        var args = new BeforeShutdownEventArgs();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Ok(true));
        var dialogService = Substitute.For<IDialogService>();
        var wasCalled = false;
        dialogService.ShowAsync<UnsavedWorldDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(), Arg.Any<DialogOptions>())
            .Returns(dialogReference)
            .AndDoes(callinfo => {
                var parameters = (DialogParameters)callinfo[1];
                var options = (DialogOptions)callinfo[2];
                Assert.Multiple(() =>
                {
                    Assert.That(callinfo[0], Is.EqualTo("Unsaved changes!"));
                    Assert.That(parameters[nameof(UnsavedWorldDialog.WorldName)], Is.EqualTo(unsavedWorld.Name));
                    Assert.That(options.CloseButton, Is.True);
                    Assert.That(options.CloseOnEscapeKey, Is.True);
                    Assert.That(options.DisableBackdropClick, Is.True);
                });
                wasCalled = true;
            });

        var systemUnderTest = CreatePresenterForTesting(viewModel, presentationLogic: presentationLogic,
            dialogService: dialogService);

        await systemUnderTest.OnBeforeShutdownAsync(null, args);
        Assert.Multiple(() =>
        {
            presentationLogic.Received().SaveLearningWorldAsync(unsavedWorld);
            Assert.That(wasCalled);
        });
    }

    [Test]
    public async Task OnBeforeShutdown_CallsDialogService_CancelsShutdownOnCancelReturnValue()
    {
        var viewModel = new AuthoringToolWorkspaceViewModel();
        var unsavedWorld = new LearningWorldViewModel("unsaved", "f", "f", "f", "f", "f")
        {
            UnsavedChanges = true
        };
        viewModel._learningWorlds.Add(unsavedWorld);
        var args = new BeforeShutdownEventArgs();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Cancel());
        var dialogService = Substitute.For<IDialogService>();
        var wasCalled = false;
        dialogService.ShowAsync<UnsavedWorldDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(), Arg.Any<DialogOptions>())
            .Returns(dialogReference)
            .AndDoes(_ => {
                wasCalled = true;
            });

        var systemUnderTest = CreatePresenterForTesting(viewModel, presentationLogic: presentationLogic,
            dialogService: dialogService);
        
        await systemUnderTest.OnBeforeShutdownAsync(null, args);
        Assert.Multiple(() =>
        {
            presentationLogic.DidNotReceive().SaveLearningWorldAsync(unsavedWorld);
            Assert.That(wasCalled);
            Assert.That(args.CancelShutdownState);
        });
        
    }

    #endregion

    private AuthoringToolWorkspacePresenter CreatePresenterForTesting(
        IAuthoringToolWorkspaceViewModel? authoringToolWorkspaceVm = null, IPresentationLogic? presentationLogic = null,
        ILearningWorldPresenter? learningWorldPresenter = null, ILearningSpacePresenter? learningSpacePresenter = null,
        ILogger<AuthoringToolWorkspacePresenter>? logger = null, IMediator? mediator = null,
        IShutdownManager? shutdownManager = null,
        IDialogService? dialogService = null)
    {
        authoringToolWorkspaceVm ??= Substitute.For<IAuthoringToolWorkspaceViewModel>();
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        learningWorldPresenter ??= Substitute.For<ILearningWorldPresenter>();
        learningSpacePresenter ??= Substitute.For<ILearningSpacePresenter>();
        logger ??= Substitute.For<ILogger<AuthoringToolWorkspacePresenter>>();
        mediator ??= Substitute.For<IMediator>();
        shutdownManager ??= Substitute.For<IShutdownManager>();
        dialogService ??= Substitute.For<IDialogService>();
        return new AuthoringToolWorkspacePresenter(authoringToolWorkspaceVm, presentationLogic,
            learningSpacePresenter, logger, mediator, shutdownManager, dialogService);
    }

    private LearningWorldPresenter CreateLearningWorldPresenter(IPresentationLogic? presentationLogic = null,
        ILearningSpacePresenter? learningSpacePresenter = null, ILogger<LearningWorldPresenter>? logger = null,
        IMediator? mediator = null, IErrorService? errorService = null)
    {
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        learningSpacePresenter ??= Substitute.For<ILearningSpacePresenter>();
        logger ??= Substitute.For<ILogger<LearningWorldPresenter>>();
        errorService ??= Substitute.For<IErrorService>();
        mediator ??= Substitute.For<IMediator>();
        return new LearningWorldPresenter(presentationLogic, learningSpacePresenter, logger,
             mediator, errorService);
    }
}