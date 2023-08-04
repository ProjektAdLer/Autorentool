using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Dialogues;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningWorld;
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

        var systemUnderTest = CreatePresenterForTesting(workspaceVm);
        Assert.Multiple(() => { Assert.That(systemUnderTest.AuthoringToolWorkspaceVm, Is.EqualTo(workspaceVm)); });
    }

    [Test]
    public void CreateLearningWorld_CallsCreateLearningWorldOnPresentationLogic()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, presentationLogic: presentationLogic,
            selectedViewModelsProvider: selectedViewModelsProvider);

        systemUnderTest.AuthoringToolWorkspaceVm.LearningWorlds.Add(
            new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo", "Foo"));

        systemUnderTest.CreateLearningWorld("n", "s", "a", "l", "d", "g");

        presentationLogic.Received(1).CreateLearningWorld(workspaceVm, "n", "s", "a", "l", "d", "g");
    }

    [Test]
    public void ChangeSelectedLearningWorld_MutatesSelectionInWorkspaceViewModel()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        selectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        var world2 = new LearningWorldViewModel("tetete", "f", "bar", "de", "A test",
            "testing");
        workspaceVm._learningWorlds.Add(world1);
        workspaceVm._learningWorlds.Add(world2);

        var systemUnderTest = CreatePresenterForTesting(workspaceVm,
            selectedViewModelsProvider: selectedViewModelsProvider);

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
    public void ChangeSelectedLearningWorld_CallsErrorServiceIfNoLearningWorldWithName()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var errorService = Substitute.For<IErrorService>();
        Assert.That(workspaceVm.LearningWorlds, Is.Empty);

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, errorService: errorService);

        systemUnderTest.SetSelectedLearningWorld("foo");
        errorService.Received(1).SetError("Operation failed", "No learning world with name foo found");
    }

    [Test]
    public void DeleteSelectedLearningWorld_CallsPresentationLogic()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var world1 = new LearningWorldViewModel("Foo", "Foo", "Foo", "Foo", "Foo",
            "Foo");
        workspaceVm._learningWorlds.Add(world1);
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, presentationLogic: presentationLogic,
            selectedViewModelsProvider: selectedViewModelsProvider);

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
                selectedViewModelsProvider.SetLearningWorld(
                    ((AuthoringToolWorkspaceViewModel)y.Args()[0]).LearningWorlds.LastOrDefault(), null);
                selectedViewModelsProvider.LearningWorld.Returns(((AuthoringToolWorkspaceViewModel)y.Args()[0])
                    .LearningWorlds.LastOrDefault());
            });

        var systemUnderTest = CreatePresenterForTesting(workspaceVm, presentationLogic: presentationLogic,
            selectedViewModelsProvider: selectedViewModelsProvider);

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

        var systemUnderTest = CreatePresenterForTesting(workspaceVm);

        Assert.DoesNotThrow(systemUnderTest.DeleteSelectedLearningWorld);
    }

    [Test]
    public async Task DeleteLearningWorld_CallsSaveLearningWorldAsync_WhenUnsavedChangesAndYesResponse()
    {
        var learningWorld = new LearningWorldViewModel("unsaved", "f", "f", "f", "f", "f")
        {
            UnsavedChanges = true
        };
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Ok(true));
        var dialogService = Substitute.For<IDialogService>();
        dialogService
            .ShowAsync<UnsavedWorldDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(), Arg.Any<DialogOptions>())
            .Returns(dialogReference);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic,
            dialogService: dialogService);

        await systemUnderTest.DeleteLearningWorld(learningWorld);

        await presentationLogic.Received().SaveLearningWorldAsync(learningWorld);
    }

    [Test]
    public async Task DeleteLearningWorld_CallsDeleteLearningWorld_WhenNoUnsavedChangesOrNoResponse()
    {
        var learningWorld = new LearningWorldViewModel("saved", "f", "f", "f", "f", "f")
        {
            UnsavedChanges = false
        };
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Ok(false));
        var dialogService = Substitute.For<IDialogService>();
        dialogService
            .ShowAsync<UnsavedWorldDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(), Arg.Any<DialogOptions>())
            .Returns(dialogReference);

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic,
            dialogService: dialogService);

        await systemUnderTest.DeleteLearningWorld(learningWorld);

        presentationLogic.Received().DeleteLearningWorld(Arg.Any<IAuthoringToolWorkspaceViewModel>(), learningWorld);
    }

    [Test]
    public async Task DeleteLearningWorld_CallsErrorService_WhenUnexpectedDialogResultType()
    {
        var learningWorld = new LearningWorldViewModel("unsaved", "f", "f", "f", "f", "f")
        {
            UnsavedChanges = true
        };
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Ok("unexpected type"));
        var dialogService = Substitute.For<IDialogService>();
        dialogService
            .ShowAsync<UnsavedWorldDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(), Arg.Any<DialogOptions>())
            .Returns(dialogReference);
        var errorService = Substitute.For<IErrorService>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic,
            dialogService: dialogService, errorService: errorService);

        await systemUnderTest.DeleteLearningWorld(learningWorld);

        errorService.Received().SetError("Operation failed", "Unexpected dialog result type");
    }

    [Test]
    public async Task DeleteLearningWorld_AbortsWhenDialogCanceled()
    {
        var learningWorld = new LearningWorldViewModel("unsaved", "f", "f", "f", "f", "f")
        {
            UnsavedChanges = true
        };
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Cancel());
        var dialogService = Substitute.For<IDialogService>();
        dialogService
            .ShowAsync<UnsavedWorldDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(), Arg.Any<DialogOptions>())
            .Returns(dialogReference);
        var errorService = Substitute.For<IErrorService>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic,
            dialogService: dialogService, errorService: errorService);

        await systemUnderTest.DeleteLearningWorld(learningWorld);

        presentationLogic.DidNotReceive()
            .DeleteLearningWorld(Arg.Any<AuthoringToolWorkspaceViewModel>(), learningWorld);
        errorService.DidNotReceive().SetError(Arg.Any<string>(), Arg.Any<string>());
    }


    [Test]
    public async Task SaveLearningWorldAsync_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var learningWorld = new LearningWorldViewModel("fo", "f", "", "f", "", "");
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic,
            selectedViewModelsProvider: selectedViewModelsProvider);

        await systemUnderTest.SaveLearningWorldAsync(learningWorld);
        await presentationLogic.Received().SaveLearningWorldAsync(learningWorld);
    }

    [Test]
    public async Task SaveLearningWorldAsync_SerializationException_CallsErrorService()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var learningWorld = new LearningWorldViewModel("fo", "f", "", "f", "", "");
        var errorService = Substitute.For<IErrorService>();
        presentationLogic
            .When(x => x.SaveLearningWorldAsync(Arg.Any<ILearningWorldViewModel>()))
            .Do(_ => throw new SerializationException("test"));

        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic: presentationLogic, errorService: errorService);

        await systemUnderTest.SaveLearningWorldAsync(learningWorld);
        errorService.Received().SetError("Error while saving world", "test");
    }

    [Test]
    public async Task SaveLearningWorldAsync_InvalidOperationException_CallsErrorService()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var learningWorld = new LearningWorldViewModel("fo", "f", "", "f", "", "");
        var errorService = Substitute.For<IErrorService>();
        presentationLogic
            .When(x => x.SaveLearningWorldAsync(Arg.Any<ILearningWorldViewModel>()))
            .Do(_ => throw new InvalidOperationException("test"));

        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic: presentationLogic, errorService: errorService);

        await systemUnderTest.SaveLearningWorldAsync(learningWorld);
        errorService.Received().SetError("Error while saving world", "test");
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
            CreatePresenterForTesting(presentationLogic: presentationLogic, authoringToolWorkspaceVm: viewModel,
                selectedViewModelsProvider: selectedViewModelsProvider);

        selectedViewModelsProvider.LearningWorld.Returns(learningWorld);
        await systemUnderTest.SaveSelectedLearningWorldAsync();
        await presentationLogic.Received().SaveLearningWorldAsync(learningWorld);
    }

    [Test]
    public async Task SaveSelectedLearningWorldAsync_CallsErrorServiceIfSelectedWorldNull()
    {
        var viewModel = new AuthoringToolWorkspaceViewModel();
        var mockPresentationLogic = Substitute.For<IPresentationLogic>();
        var mockSelectedViewModelProvider = Substitute.For<ISelectedViewModelsProvider>();
        var errorService = Substitute.For<IErrorService>();
        mockSelectedViewModelProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);

        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic: mockPresentationLogic, authoringToolWorkspaceVm: viewModel,
                selectedViewModelsProvider: mockSelectedViewModelProvider, errorService: errorService);

        await systemUnderTest.SaveSelectedLearningWorldAsync();
        errorService.Received().SetError("Operation failed", "No world selected");
    }

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

        var systemUnderTest = CreatePresenterForTesting(viewModel, presentationLogic: presentationLogic,
            selectedViewModelsProvider: selectedViewModelsProvider);

        await systemUnderTest.LoadLearningWorldAsync();
        await presentationLogic.Received().LoadLearningWorldAsync(viewModel);
        Assert.That(viewModel.LearningWorlds, Contains.Item(learningWorld));
    }

    [Test]
    public async Task LoadLearningWorldAsync_SerializationException_CallsErrorService()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var errorService = Substitute.For<IErrorService>();
        presentationLogic
            .When(x => x.LoadLearningWorldAsync(Arg.Any<IAuthoringToolWorkspaceViewModel>()))
            .Do(_ => { throw new SerializationException("test"); });
        var viewModel = new AuthoringToolWorkspaceViewModel();

        var systemUnderTest = CreatePresenterForTesting(viewModel, presentationLogic: presentationLogic,
            selectedViewModelsProvider: selectedViewModelsProvider, errorService: errorService);

        await systemUnderTest.LoadLearningWorldAsync();
        errorService.Received().SetError("Error while loading world", "test");
    }

    [Test]
    public async Task LoadLearningWorldAsync_InvalidOperationException_CallsErrorService()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var errorService = Substitute.For<IErrorService>();
        presentationLogic
            .When(x => x.LoadLearningWorldAsync(Arg.Any<IAuthoringToolWorkspaceViewModel>()))
            .Do(_ => { throw new InvalidOperationException("test"); });
        var viewModel = new AuthoringToolWorkspaceViewModel();

        var systemUnderTest = CreatePresenterForTesting(viewModel, presentationLogic: presentationLogic,
            selectedViewModelsProvider: selectedViewModelsProvider, errorService: errorService);

        await systemUnderTest.LoadLearningWorldAsync();
        errorService.Received().SetError("Error while loading world", "test");
    }

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
        dialogService
            .ShowAsync<UnsavedWorldDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(), Arg.Any<DialogOptions>())
            .Returns(dialogReference)
            .AndDoes(callinfo =>
            {
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
        dialogService
            .ShowAsync<UnsavedWorldDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(), Arg.Any<DialogOptions>())
            .Returns(dialogReference)
            .AndDoes(_ => { wasCalled = true; });

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

    [Test]
    public async Task OnBeforeShutdown_CallsErrorService_WhenUnexpectedDialogResultTypeReturned()
    {
        var viewModel = new AuthoringToolWorkspaceViewModel();
        var unsavedWorld = new LearningWorldViewModel("unsaved", "f", "f", "f", "f", "f")
        {
            UnsavedChanges = true
        };
        viewModel._learningWorlds.Add(unsavedWorld);
        var args = new BeforeShutdownEventArgs();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var errorService = Substitute.For<IErrorService>();
        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Ok("unexpected"));
        var dialogService = Substitute.For<IDialogService>();
        dialogService
            .ShowAsync<UnsavedWorldDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(), Arg.Any<DialogOptions>())
            .Returns(dialogReference);

        var systemUnderTest = CreatePresenterForTesting(viewModel, presentationLogic: presentationLogic,
            dialogService: dialogService, errorService: errorService);

        await systemUnderTest.OnBeforeShutdownAsync(null, args);

        errorService.Received().SetError(Arg.Is<string>(s => s == "Operation failed"),
            Arg.Is<string>(s => s == "Unexpected dialog result type"));
    }


    private AuthoringToolWorkspacePresenter CreatePresenterForTesting(
        IAuthoringToolWorkspaceViewModel? authoringToolWorkspaceVm = null, IPresentationLogic? presentationLogic = null,
        ILogger<AuthoringToolWorkspacePresenter>? logger = null,
        ISelectedViewModelsProvider? selectedViewModelsProvider = null,
        IShutdownManager? shutdownManager = null,
        IDialogService? dialogService = null, IErrorService? errorService = null)
    {
        authoringToolWorkspaceVm ??= Substitute.For<IAuthoringToolWorkspaceViewModel>();
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        logger ??= Substitute.For<ILogger<AuthoringToolWorkspacePresenter>>();
        selectedViewModelsProvider ??= Substitute.For<ISelectedViewModelsProvider>();
        shutdownManager ??= Substitute.For<IShutdownManager>();
        dialogService ??= Substitute.For<IDialogService>();
        errorService ??= Substitute.For<IErrorService>();
        return new AuthoringToolWorkspacePresenter(authoringToolWorkspaceVm, presentationLogic, logger,
            selectedViewModelsProvider, shutdownManager, dialogService, errorService);
    }
}