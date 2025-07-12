using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
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
using Shared.Theme;
using TestHelpers;

namespace PresentationTest.PresentationLogic.AuthoringToolWorkspace;

[TestFixture]
public class AuthoringToolWorkspacePresenterUt
{
    [Test]
    public void StandardConstructor_AllPropertiesInitialized()
    {
        var workspaceVm = Substitute.For<IAuthoringToolWorkspaceViewModel>();

        var systemUnderTest = CreatePresenterForTesting(workspaceVm);
        Assert.Multiple(() => { Assert.That(systemUnderTest.AuthoringToolWorkspaceVm, Is.EqualTo(workspaceVm)); });
    }

    [Test]
    // ANF-ID: [ASE1]
    public void CreateLearningWorld_CallsCreateLearningWorldOnPresentationLogic()
    {
        var workspaceVm = new AuthoringToolWorkspaceViewModel();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        var systemUnderTest = CreatePresenterForTesting(workspaceVm, presentationLogic: presentationLogic,
            selectedViewModelsProvider: selectedViewModelsProvider);

        systemUnderTest.AuthoringToolWorkspaceVm.LearningWorlds.Add(ViewModelProvider.GetLearningWorld());
        var loc = ViewModelProvider.GetLearningOutcomeCollection();

        systemUnderTest.CreateLearningWorld("n", "s", "a", "l", "d", loc, WorldTheme.CampusAschaffenburg, "e", "f",
            "ss", "se");

        presentationLogic.Received(1).CreateLearningWorld(workspaceVm, "n", "s", "a", "l", "d", loc,
            WorldTheme.CampusAschaffenburg, "e", "f", "ss", "se");
    }

    [Test]
    // ANF-ID: [ASE4]
    public async Task DeleteLearningWorld_CallsSaveLearningWorldAsync_WhenUnsavedChangesAndYesResponse()
    {
        var learningWorld = ViewModelProvider.GetLearningWorld(unsavedChanges: true);
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

        presentationLogic.Received().SaveLearningWorld(learningWorld);
    }

    [Test]
    // ANF-ID: [ASE4]
    public async Task DeleteLearningWorld_CallsDeleteLearningWorld_WhenNoUnsavedChangesOrNoResponse()
    {
        var learningWorld = ViewModelProvider.GetLearningWorld(unsavedChanges: false);
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
    // ANF-ID: [ASE4]
    public async Task DeleteLearningWorld_CallsErrorService_WhenUnexpectedDialogResultType()
    {
        var learningWorld = ViewModelProvider.GetLearningWorld(unsavedChanges: true);
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
    // ANF-ID: [ASE4]
    public async Task DeleteLearningWorld_AbortsWhenDialogCanceled()
    {
        var learningWorld = ViewModelProvider.GetLearningWorld(unsavedChanges: true);
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
    // ANF-ID: [ASE6]
    public void SaveLearningWorld_CallsPresentationLogic()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var learningWorld = ViewModelProvider.GetLearningWorld();
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();

        var systemUnderTest = CreatePresenterForTesting(presentationLogic: presentationLogic,
            selectedViewModelsProvider: selectedViewModelsProvider);

        systemUnderTest.SaveLearningWorld(learningWorld);
        presentationLogic.Received().SaveLearningWorld(learningWorld);
    }

    [Test]
    // ANF-ID: [ASE6]
    public void SaveLearningWorld_SerializationException_CallsErrorService()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var learningWorld = ViewModelProvider.GetLearningWorld();
        var errorService = Substitute.For<IErrorService>();
        presentationLogic
            .When(x => x.SaveLearningWorld(Arg.Any<ILearningWorldViewModel>()))
            .Do(_ => throw new SerializationException("test"));

        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic: presentationLogic, errorService: errorService);

        systemUnderTest.SaveLearningWorld(learningWorld);
        errorService.Received().SetError("Error while saving world", "test");
    }

    [Test]
    // ANF-ID: [ASE6]
    public void SaveLearningWorld_InvalidOperationException_CallsErrorService()
    {
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var learningWorld = ViewModelProvider.GetLearningWorld();
        var errorService = Substitute.For<IErrorService>();
        presentationLogic
            .When(x => x.SaveLearningWorld(Arg.Any<ILearningWorldViewModel>()))
            .Do(_ => throw new InvalidOperationException("test"));

        var systemUnderTest =
            CreatePresenterForTesting(presentationLogic: presentationLogic, errorService: errorService);

        systemUnderTest.SaveLearningWorld(learningWorld);
        errorService.Received().SetError("Error while saving world", "test");
    }

    [Test]
    // ANF-ID: [ASE6, ASN0025]
    public async Task OnBeforeShutdown_CallsDialogService_ForEveryUnsavedWorld_AndCallsSaveOnYesResponse()
    {
        var viewModel = new AuthoringToolWorkspaceViewModel();
        var unsavedWorld = ViewModelProvider.GetLearningWorld(unsavedChanges: true);
        var savedWorld = ViewModelProvider.GetLearningWorld(unsavedChanges: false);
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
                    Assert.That(parameters[nameof(UnsavedWorldDialog.WorldName)], Is.EqualTo(unsavedWorld.Name));
                    Assert.That(options.CloseButton, Is.True);
                    Assert.That(options.CloseOnEscapeKey, Is.True);
                    Assert.That(options.BackdropClick, Is.False);
                });
                wasCalled = true;
            });

        var systemUnderTest = CreatePresenterForTesting(viewModel, presentationLogic: presentationLogic,
            dialogService: dialogService);

        await systemUnderTest.OnBeforeShutdownAsync(null, args);
        Assert.Multiple(() =>
        {
            presentationLogic.Received().SaveLearningWorld(unsavedWorld);
            Assert.That(wasCalled);
        });
    }

    [Test]
    // ANF-ID: [ASN0025]
    public async Task OnBeforeShutdown_CallsDialogService_CancelsShutdownOnCancelReturnValue()
    {
        var viewModel = new AuthoringToolWorkspaceViewModel();
        var unsavedWorld = ViewModelProvider.GetLearningWorld(unsavedChanges: true);
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
            presentationLogic.DidNotReceive().SaveLearningWorld(unsavedWorld);
            Assert.That(wasCalled);
            Assert.That(args.CancelShutdownState);
        });
    }

    [Test]
    // ANF-ID: [ASN0025]
    public async Task OnBeforeShutdown_CallsErrorService_WhenUnexpectedDialogResultTypeReturned()
    {
        var viewModel = new AuthoringToolWorkspaceViewModel();
        var unsavedWorld = ViewModelProvider.GetLearningWorld(unsavedChanges: true);
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
        IDialogService? dialogService = null, IErrorService? errorService = null,
        IStringLocalizer<AuthoringToolWorkspacePresenter>? localizer = null)
    {
        authoringToolWorkspaceVm ??= Substitute.For<IAuthoringToolWorkspaceViewModel>();
        presentationLogic ??= Substitute.For<IPresentationLogic>();
        logger ??= Substitute.For<ILogger<AuthoringToolWorkspacePresenter>>();
        selectedViewModelsProvider ??= Substitute.For<ISelectedViewModelsProvider>();
        shutdownManager ??= Substitute.For<IShutdownManager>();
        dialogService ??= Substitute.For<IDialogService>();
        errorService ??= Substitute.For<IErrorService>();
        localizer ??= Substitute.For<IStringLocalizer<AuthoringToolWorkspacePresenter>>();
        return new AuthoringToolWorkspacePresenter(authoringToolWorkspaceVm, presentationLogic, logger,
            selectedViewModelsProvider, shutdownManager, dialogService, errorService, localizer);
    }
}