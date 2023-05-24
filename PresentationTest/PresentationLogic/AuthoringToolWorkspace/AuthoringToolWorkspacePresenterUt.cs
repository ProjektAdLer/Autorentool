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
using Presentation.PresentationLogic.Mediator;
using Presentation.PresentationLogic.SelectedViewModels;

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
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, presentationLogic: presentationLogic, selectedViewModelsProvider: selectedViewModelsProvider);
        
        systemUnderTest.AuthoringToolWorkspaceVm.LearningWorlds.Add(new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo", "Foo"));

        systemUnderTest.CreateLearningWorld("n", "s", "a", "l", "d", "g");

        presentationLogic.Received(1).CreateLearningWorld(workspaceVm, "n", "s", "a", "l", "d", "g");
    }

    #endregion

    #region ChangeSelectedLearningWorld

    [Test]
    public void ChangeSelectedLearningWorld_MutatesSelectionInWorkspaceViewModel()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        selectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var worldPresenter = CreateLearningWorldPresenter(selectedViewModelsProvider: selectedViewModelsProvider);
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        var world2 = new LearningWorldViewModel("tetete", "f", "bar", "de", "A test",
            "testing");
        workspaceVm._learningWorlds.Add(world1);
        workspaceVm._learningWorlds.Add(world2);

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, learningWorldPresenter: worldPresenter, selectedViewModelsProvider: selectedViewModelsProvider);

        Assert.That(selectedViewModelsProvider.LearningWorld, Is.Null);

        systemUnderTest.SetSelectedLearningWorld("tetete");
        selectedViewModelsProvider.Received(1).SetLearningWorld(world2, null);
        selectedViewModelsProvider.LearningWorld.Returns(world2);
        Assert.That(selectedViewModelsProvider.LearningWorld, Is.EqualTo(world2));

        systemUnderTest.SetSelectedLearningWorld("Foo");
        selectedViewModelsProvider.Received(1).SetLearningWorld(world1, null);
        selectedViewModelsProvider.LearningWorld.Returns(world1);
        Assert.That(selectedViewModelsProvider.LearningWorld, Is.EqualTo(world1));
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
    public void DeleteSelectedLearningWorld_CallsPresentationLogic()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        workspaceVm._learningWorlds.Add(world1);
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, presentationLogic: presentationLogic, selectedViewModelsProvider: selectedViewModelsProvider);

        systemUnderTest.SetSelectedLearningWorld(world1.Name);
        selectedViewModelsProvider.Received(1).SetLearningWorld(world1, null);
        selectedViewModelsProvider.LearningWorld.Returns(world1);
        systemUnderTest.DeleteSelectedLearningWorld();

        presentationLogic.Received().DeleteLearningWorld(workspaceVm, world1);
    }


    [Test]
    public void DeleteSelectedLearningWorld_MutatesSelectionInWorkspaceViewModel()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
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
                workspaceVm.LearningWorlds.Remove(selectedViewModelsProvider.LearningWorld!);
                //workspaceVm.RemoveLearningWorld(selectedViewModelsProvider.LearningWorld!);
                selectedViewModelsProvider.SetLearningWorld(((AuthoringToolWorkspaceViewModel)y.Args()[0]).LearningWorlds.LastOrDefault(), null);
                selectedViewModelsProvider.LearningWorld.Returns(((AuthoringToolWorkspaceViewModel)y.Args()[0])
                    .LearningWorlds.LastOrDefault());
            });

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, presentationLogic: presentationLogic, selectedViewModelsProvider: selectedViewModelsProvider);

        systemUnderTest.SetSelectedLearningWorld(world2.Name);
        selectedViewModelsProvider.Received(1).SetLearningWorld(world2, null);
        selectedViewModelsProvider.LearningWorld.Returns(world2);
        Assert.That(selectedViewModelsProvider.LearningWorld, Is.EqualTo(world2));

        systemUnderTest.DeleteSelectedLearningWorld();

        Assert.That(selectedViewModelsProvider.LearningWorld, Is.EqualTo(world1));
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
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic, selectedViewModelsProvider: selectedViewModelsProvider);

        await systemUnderTest.SaveLearningWorldAsync(learningWorld);
        await presentationLogic.Received().SaveLearningWorldAsync(learningWorld);
    }

    [Test]
    public async Task SaveSelectedLearningWorldAsync_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var learningWorld = new LearningWorldViewModel("fo", "f", "", "f", "", "");
        var viewModel = new AuthoringToolWorkspaceViewModel();
        viewModel._learningWorlds.Add(learningWorld);

        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic: presentationLogic, authoringToolWorkspaceVm: viewModel, selectedViewModelsProvider: selectedViewModelsProvider);

        selectedViewModelsProvider.LearningWorld.Returns(learningWorld);
        await systemUnderTest.SaveSelectedLearningWorldAsync();
        await presentationLogic.Received().SaveLearningWorldAsync(learningWorld);
    }

    [Test]
    public void SaveSelectedLearningWorldAsync_ThrowsIfSelectedWorldNull()
    {
        var viewModel = new AuthoringToolWorkspaceViewModel();
        var mockPresentationLogic = Substitute.For<IPresentationLogic>();
        var mockSelectedViewModelProvider = Substitute.For<ISelectedViewModelsProvider>();
        mockSelectedViewModelProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);

        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic: mockPresentationLogic, authoringToolWorkspaceVm: viewModel,
                selectedViewModelsProvider: mockSelectedViewModelProvider);

        Assert.ThrowsAsync<ApplicationException>(async () => await systemUnderTest.SaveSelectedLearningWorldAsync());
    }

    #endregion

    #region LoadLearningWorldAsync

    [Test]
    public async Task LoadLearningWorldAsync_CallsPresentationLogicAndAddsToViewModel()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var learningWorld = new LearningWorldViewModel("f", "f", "f", "f", "f", "f");
        presentationLogic
            .When(x => x.LoadLearningWorldAsync(Arg.Any<IAuthoringToolWorkspaceViewModel>()))
            .Do(y =>
            {
                ((AuthoringToolWorkspaceViewModel)y.Args()[0])._learningWorlds.Add(learningWorld);
                selectedViewModelsProvider.SetLearningWorld(learningWorld, null);
            });
        var viewModel = new AuthoringToolWorkspaceViewModel();

        var systemUnderTest = CreatePresenterForTesting(viewModel, presentationLogic: presentationLogic, selectedViewModelsProvider: selectedViewModelsProvider);

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
        ILogger<AuthoringToolWorkspacePresenter>? logger = null, ISelectedViewModelsProvider? selectedViewModelsProvider = null,
        IShutdownManager? shutdownManager = null,
        IDialogService? dialogService = null)
    {
        authoringToolWorkspaceVm ??= Substitute.For<IAuthoringToolWorkspaceViewModel>();
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        learningWorldPresenter ??= Substitute.For<ILearningWorldPresenter>();
        learningSpacePresenter ??= Substitute.For<ILearningSpacePresenter>();
        logger ??= Substitute.For<ILogger<AuthoringToolWorkspacePresenter>>();
        selectedViewModelsProvider ??= Substitute.For<ISelectedViewModelsProvider>();
        shutdownManager ??= Substitute.For<IShutdownManager>();
        dialogService ??= Substitute.For<IDialogService>();
        return new AuthoringToolWorkspacePresenter(authoringToolWorkspaceVm, presentationLogic,
            learningSpacePresenter, logger, selectedViewModelsProvider, shutdownManager, dialogService);
    }

    private LearningWorldPresenter CreateLearningWorldPresenter(IPresentationLogic? presentationLogic = null,
        ILearningSpacePresenter? learningSpacePresenter = null, ILogger<LearningWorldPresenter>? logger = null,
        IMediator? mediator = null,
        ISelectedViewModelsProvider? selectedViewModelsProvider = null, IErrorService? errorService = null)
    {
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        learningSpacePresenter ??= Substitute.For<ILearningSpacePresenter>();
        logger ??= Substitute.For<ILogger<LearningWorldPresenter>>();
        errorService ??= Substitute.For<IErrorService>();
        selectedViewModelsProvider ??= Substitute.For<ISelectedViewModelsProvider>();
        mediator ??= Substitute.For<IMediator>();
        return new LearningWorldPresenter(presentationLogic, learningSpacePresenter, logger, mediator,
             selectedViewModelsProvider, errorService);
    }
}