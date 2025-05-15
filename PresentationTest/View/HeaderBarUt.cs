using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bunit;
using Bunit.Rendering;
using Bunit.TestDoubles;
using ElectronWrapper;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MudBlazor;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Presentation.Components;
using Presentation.Components.Culture;
using Presentation.Components.Dialogues;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Action;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Mediator;
using Presentation.PresentationLogic.SelectedViewModels;
using Presentation.View;
using Shared;
using Shared.Exceptions;
using TestHelpers;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View;

[TestFixture]
public class HeaderBarUt
{
    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
        _testContext.ComponentFactories.AddStub<CloseAppButton>();
        _testContext.ComponentFactories.AddStub<CultureSelector>();
        _testContext.ComponentFactories.AddStub<LmsLoginButton>();
        _testContext.ComponentFactories.AddStub<MudPopover>();
        _testContext.ComponentFactories.AddStub<MudDivider>();
        _testContext.ComponentFactories.AddStub<MudMenu>();
        _testContext.ComponentFactories.AddStub<MudMenuItem>();
        _presentationLogic = Substitute.For<IPresentationLogic>();
        _selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        _mediator = Substitute.For<IMediator>();
        _stringLocalizer = Substitute.For<IStringLocalizer<HeaderBar>>();
        _stringLocalizer[Arg.Any<string>()].Returns(ci => new LocalizedString(ci.Arg<string>(), ci.Arg<string>()));
        _stringLocalizer[Arg.Any<string>(), Arg.Any<object[]>()].Returns(ci =>
            new LocalizedString(ci.Arg<string>(), FormatStringLocalizerValue(ci)));
        _snackbar = Substitute.For<ISnackbar>();
        _dialogService = Substitute.For<IDialogService>();
        _errorService = Substitute.For<IErrorService>();
        _logger = Substitute.For<ILogger>();
        _shellwrapper = Substitute.For<IShellWrapper>();
        _testContext.Services.AddSingleton(_presentationLogic);
        _testContext.Services.AddSingleton(_stringLocalizer);
        _testContext.Services.AddSingleton(_selectedViewModelsProvider);
        _testContext.Services.AddSingleton(_mediator);
        _testContext.Services.AddSingleton(_snackbar);
        _testContext.Services.AddSingleton(_dialogService);
        _testContext.Services.AddSingleton(_errorService);
        _testContext.Services.AddSingleton(_logger);
        _testContext.Services.AddSingleton(_shellwrapper);
    }

    [TearDown]
    public void TearDown()
    {
        _testContext.Dispose();
        _snackbar.Dispose();
    }

    private static string FormatStringLocalizerValue(CallInfo ci)
    {
        return ci.Arg<string>() + " " + string.Join(" ", ci.Arg<object[]>().Select(obj => obj.ToString()));
    }

    [Test]
    public void Render_RunningElectronTrue_ContainsCloseAppButtonStub()
    {
        _presentationLogic.RunningElectron.Returns(true);

        var systemUnderTest = GetRenderedComponent();

        Assert.That(() => systemUnderTest.FindComponentOrFail<Stub<CloseAppButton>>(), Throws.Nothing);
    }

    [Test]
    public void Render_RunningElectronFalse_ContainsNoCloseAppButtonStub()
    {
        _presentationLogic.RunningElectron.Returns(false);

        var systemUnderTest = GetRenderedComponent();

        Assert.That(() => systemUnderTest.FindComponent<Stub<CloseAppButton>>(),
            Throws.TypeOf<ComponentNotFoundException>());
    }

    [Test]
    public void Render_ContainsCultureSelectorStub()
    {
        var systemUnderTest = GetRenderedComponent();

        Assert.That(() => systemUnderTest.FindComponent<Stub<CultureSelector>>(), Throws.Nothing);
    }

    [Test]
    public void Render_ContainsLmsLoginButtonStub()
    {
        var systemUnderTest = GetRenderedComponent();

        Assert.That(() => systemUnderTest.FindComponent<Stub<LmsLoginButton>>(), Throws.Nothing);
    }

    [Test]
    public void Render_ShowsLocalizedAuthoringToolName()
    {
        _stringLocalizer["AuthoringTool.Text"].Returns(new LocalizedString("AuthoringTool.Text", "TestName"));
        _stringLocalizer["AuthoringTool.Version"].Returns(new LocalizedString("AuthoringTool.Version", "v3"));
        _selectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);

        var systemUnderTest = GetRenderedComponent();

        var element = systemUnderTest.FindAll("div p")[1];
        element.MarkupMatches(
            @$"<p class=""font-bold text-base 2xl:text-lg text-adlertitledarkblue"">TestName {Constants.ApplicationVersion}</h1>");
    }

    [Test]
    // ANF-ID: [AHO22]
    public void ExportButton_Clicked_LMSConnected_PositiveDialogResponse_CallsPresentationLogic()
    {
        var world = new LearningWorldViewModel("a", "f", "d", "e", "f", "d", "h", "i", "j", "k");
        var space = new LearningSpaceViewModel("a", "f", Theme.CampusAschaffenburg, 1);
        var element = new LearningElementViewModel("a", ViewModelProvider.GetFileContent(), "s", "e", LearningElementDifficultyEnum.Easy,
            ElementModel.l_h5p_blackboard_1, points: 1);
        space.LearningSpaceLayout.LearningElements.Add(0, element);
        world.LearningSpaces.Add(space);
        _selectedViewModelsProvider.LearningWorld.Returns(world);
        _presentationLogic.IsLmsConnected().Returns(true);
        _presentationLogic.GetLmsWorldList().Returns(new List<LmsWorldViewModel>());
        _presentationLogic.GetAllContent().Returns(new List<ILearningContentViewModel>
            {element.LearningContent });
        var uploadResponseViewModel = new UploadResponseViewModel
        {
            WorldNameInLms = "worldName",
            WorldLmsUrl = "worldLmsUrl",
            World3DUrl = "world3DUrl"
        };
        _presentationLogic
            .ConstructAndUploadBackupAsync(world, Arg.Any<IProgress<int>>(), Arg.Any<CancellationToken>())
            .Returns(uploadResponseViewModel);
        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Ok(true));
        _dialogService
            .ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>())
            .Returns(dialogReference);
        var systemUnderTest = GetRenderedComponent();

        var button = systemUnderTest.FindOrFail("button[title='3DWorld.Generate.Hover']");
        button.Click();
        _presentationLogic.Received()
            .ConstructAndUploadBackupAsync(world, Arg.Any<IProgress<int>>(), Arg.Any<CancellationToken>());
        _snackbar.Received().Add("Export.SnackBar.Message", Arg.Any<Severity>());
        _dialogService.Received().ShowAsync<UploadSuccessfulDialog>(Arg.Any<string>(), Arg.Is<DialogParameters>(d =>
                ReferenceEquals(d[nameof(UploadSuccessfulDialog.Url3D)], uploadResponseViewModel.World3DUrl) &&
                ReferenceEquals(d[nameof(UploadSuccessfulDialog.UrlMoodle)], uploadResponseViewModel.WorldLmsUrl) &&
                ReferenceEquals(d[nameof(UploadSuccessfulDialog.WorldName)], uploadResponseViewModel.WorldNameInLms)),
            Arg.Any<DialogOptions>());
    }

    [Test]
    public void ExportButton_Clicked_WorldHasNoSpaces_ErrorServiceCalled()
    {
        var world = new LearningWorldViewModel("a", "f", "d", "e", "f", "d", "h", "i", "j", "k");
        _selectedViewModelsProvider.LearningWorld.Returns(world);
        var systemUnderTest = GetRenderedComponent();

        var button = systemUnderTest.FindOrFail("button[title='3DWorld.Generate.Hover']");
        button.Click();
        
        var errors = new List<string>();
        
        errors.Add("<li> ErrorString.Missing.LearningSpace.Message </li>");
        
        var errorString = $"<ul>{string.Join(Environment.NewLine, errors)}</ul>";

        _errorService.Received().SetError("Exception.InvalidLearningWorld.Message", errorString);
    }

    [Test]
    public void ExportButton_Clicked_WorldSpaceHasNoElementsAndInsufficientPointsAndLearningContentNotExistsInDirectory_ErrorServiceCalled()
    {
        var world = new LearningWorldViewModel("a", "f", "d", "e", "f", "d", "h", "i", "j", "k");
        var space1 = new LearningSpaceViewModel("a", "f", Theme.CampusAschaffenburg, 2);
        var space2 = new LearningSpaceViewModel("ah", "fi", Theme.CampusAschaffenburg, 3);
        var element1 = new LearningElementViewModel("a", ViewModelProvider.GetFileContent(), "s", "e", LearningElementDifficultyEnum.Easy,
            ElementModel.l_h5p_blackboard_1, points: 1);
        space1.LearningSpaceLayout.LearningElements.Add(0, element1);
        world.LearningSpaces.Add(space1);
        world.LearningSpaces.Add(space2);
        _selectedViewModelsProvider.LearningWorld.Returns(world);
        var systemUnderTest = GetRenderedComponent();

        var button = systemUnderTest.FindOrFail("button[title='3DWorld.Generate.Hover']");
        button.Click();
        
        var errors = new List<string>();
        
        errors.Add($"<li> ErrorString.Insufficient.Points.Message {space1.Name} </li>");
        errors.Add($"<li> ErrorString.Missing.LearningContent.Message {element1.Name} </li>");
        errors.Add($"<li> ErrorString.Missing.LearningElements.Message {space2.Name} </li>");
        errors.Add($"<li> ErrorString.Insufficient.Points.Message {space2.Name} </li>");
        
        var errorString = $"<ul>{string.Join(Environment.NewLine, errors)}</ul>";

        _errorService.Received().SetError("Exception.InvalidLearningWorld.Message", errorString);
    }

    [Test]
    public async Task ExportButton_Clicked_AdaptivityContentWithNoTasks_ErrorServiceCalled()
    {
        var world = ViewModelProvider.GetLearningWorldWithSpaceWithElement();
        var space = world.LearningSpaces.First();
        var element = space.LearningSpaceLayout.LearningElements.First().Value;
        var adaptivityContent = ViewModelProvider.GetAdaptivityContent();
        element.LearningContent = adaptivityContent;
        adaptivityContent.Tasks.Clear();

        _selectedViewModelsProvider.LearningWorld.Returns(world);

        var systemUnderTest = GetRenderedComponent();

        var button = systemUnderTest.FindOrFail("button[title='3DWorld.Generate.Hover']");
        await button.ClickAsync(new MouseEventArgs());
        
        var errors = new List<string>();
        
        errors.Add($"<li> ErrorString.NoTasks.Message {element.Name} </li>");
        
        var errorString = $"<ul>{string.Join(Environment.NewLine, errors)}</ul>";

        _errorService.Received().SetError("Exception.InvalidLearningWorld.Message", errorString);
    }

    [Test]
    public async Task ExportButton_Clicked_AdaptivityContentReferencesNonExistantElement_ErrorServiceCalled()
    {
        var world = ViewModelProvider.GetLearningWorldWithSpaceWithElement();
        var space = world.LearningSpaces.First();
        var element = space.LearningSpaceLayout.LearningElements.First().Value;
        var adaptivityContent = ViewModelProvider.GetAdaptivityContent();
        adaptivityContent.Tasks.First().Questions.First().Rules.First().Action =
            new ElementReferenceActionViewModel(Guid.NewGuid(), "foobar");
        element.LearningContent = adaptivityContent;

        _selectedViewModelsProvider.LearningWorld.Returns(world);

        var systemUnderTest = GetRenderedComponent();

        var button = systemUnderTest.FindOrFail("button[title='3DWorld.Generate.Hover']");
        await button.ClickAsync(new MouseEventArgs());
        
        var errors = new List<string>();
        
        errors.Add($"<li> ErrorString.TaskReferencesNonexistantElement.Message {element.Name} </li>");
        
        var errorString = $"<ul>{string.Join(Environment.NewLine, errors)}</ul>";

        _errorService.Received().SetError("Exception.InvalidLearningWorld.Message", errorString);
    }

    [Test]
    public async Task ExportButton_Clicked_AdaptivityContentReferencesUnplacedElement_ErrorServiceCalled()
    {
        var world = ViewModelProvider.GetLearningWorldWithSpaceWithElement();
        var space = world.LearningSpaces.First();
        var element = space.LearningSpaceLayout.LearningElements.First().Value;
        var adaptivityContent = ViewModelProvider.GetAdaptivityContent();
        var unplacedElement = ViewModelProvider.GetLearningElement();
        world.UnplacedLearningElements.Add(unplacedElement);
        adaptivityContent.Tasks.First().Questions.First().Rules.First().Action =
            new ElementReferenceActionViewModel(unplacedElement.Id, "foobar");
        element.LearningContent = adaptivityContent;

        _selectedViewModelsProvider.LearningWorld.Returns(world);

        var systemUnderTest = GetRenderedComponent();

        var button = systemUnderTest.FindOrFail("button[title='3DWorld.Generate.Hover']");
        await button.ClickAsync(new MouseEventArgs());
        
        var errors = new List<string>();
        
        errors.Add($"<li> ErrorString.TaskReferencesUnplacedElement.Message {element.Name} </li>");
        
        var errorString = $"<ul>{string.Join(Environment.NewLine, errors)}</ul>";

        _errorService.Received().SetError("Exception.InvalidLearningWorld.Message", errorString);
    }

    [Test]
    public async Task ExportButton_Clicked_AdaptivityContentReferencesElementInSpaceAfterOwnSpace_ErrorServiceCalled()
    {
        var world = ViewModelProvider.GetLearningWorldWithSpaceWithElement();
        var space = world.LearningSpaces.First();
        var element = space.LearningSpaceLayout.LearningElements.First().Value;
        var adaptivityContent = ViewModelProvider.GetAdaptivityContent();
        var laterElement = ViewModelProvider.GetLearningElement();
        laterElement.Points = 777;
        var laterSpace = ViewModelProvider.GetLearningSpace();
        laterSpace.LearningSpaceLayout.LearningElements.Add(0, laterElement);
        space.OutBoundObjects.Add(laterSpace);
        laterSpace.InBoundObjects.Add(space);
        world.LearningSpaces.Add(laterSpace);
        adaptivityContent.Tasks.First().Questions.First().Rules.First().Action =
            new ElementReferenceActionViewModel(laterElement.Id, "foobar");
        element.LearningContent = adaptivityContent;

        _presentationLogic.GetAllContent().Returns(new List<ILearningContentViewModel>
            {laterElement.LearningContent});

        _selectedViewModelsProvider.LearningWorld.Returns(world);

        var systemUnderTest = GetRenderedComponent();

        var button = systemUnderTest.FindOrFail("button[title='3DWorld.Generate.Hover']");
        await button.ClickAsync(new MouseEventArgs());

        var errors = new List<string>();
        
        errors.Add($"<li> ErrorString.TaskReferencesElementInSpaceAfterOwnSpace.Message {element.Name} {laterSpace.Name} {laterElement.Name} </li>");
        
        var errorString = $"<ul>{string.Join(Environment.NewLine, errors)}</ul>";

        _errorService.Received().SetError("Exception.InvalidLearningWorld.Message", errorString);
    }

    [Test]
    public void ExportButton_Clicked_ConstructBackupThrowsOperationCanceledException_SnackbarWarningAdded()
    {
        var world = new LearningWorldViewModel("a", "f", "d", "e", "f", "d", "h", "i", "j", "k");
        var space = new LearningSpaceViewModel("a", "f", Theme.CampusAschaffenburg, 1);
        var element = new LearningElementViewModel("a", ViewModelProvider.GetFileContent(), "s", "e", LearningElementDifficultyEnum.Easy,
            ElementModel.l_h5p_blackboard_1, points: 1);
        space.LearningSpaceLayout.LearningElements.Add(0, element);
        world.LearningSpaces.Add(space);
        _selectedViewModelsProvider.LearningWorld.Returns(world);
        _presentationLogic.IsLmsConnected().Returns(true);
        _presentationLogic.GetLmsWorldList().Returns(new List<LmsWorldViewModel>());
        _presentationLogic
            .ConstructAndUploadBackupAsync(world, Arg.Any<IProgress<int>>(), Arg.Any<CancellationToken>())
            .Throws(new OperationCanceledException());
        _presentationLogic.GetAllContent().Returns([element.LearningContent]);
        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Ok(true));
        _dialogService
            .ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>())
            .Returns(dialogReference);
        var systemUnderTest = GetRenderedComponent();

        var button = systemUnderTest.FindOrFail("button[title='3DWorld.Generate.Hover']");
        button.Click();

        _snackbar.Received().Add(Arg.Is<string>(s => s == "ExportCanceled.Snackbar.Message"), Arg.Any<Severity>());
    }

    [Test]
    public void ExportButton_Clicked_ConstructBackupThrowsGeneratorException_ErrorServiceCalled()
    {
        var world = new LearningWorldViewModel("a", "f", "d", "e", "f", "d", "h", "i", "j", "k");
        var space = new LearningSpaceViewModel("a", "f", Theme.CampusAschaffenburg, 1);
        var element = new LearningElementViewModel("a", ViewModelProvider.GetFileContent(), "s", "e", LearningElementDifficultyEnum.Easy,
            ElementModel.l_h5p_blackboard_1, points: 1);
        space.LearningSpaceLayout.LearningElements.Add(0, element);
        world.LearningSpaces.Add(space);
        _selectedViewModelsProvider.LearningWorld.Returns(world);
        _presentationLogic.GetAllContent().Returns(new List<ILearningContentViewModel>
            {element.LearningContent });
        _presentationLogic.IsLmsConnected().Returns(true);
        _presentationLogic.GetLmsWorldList().Returns(new List<LmsWorldViewModel>());
        _presentationLogic
            .ConstructAndUploadBackupAsync(world, Arg.Any<IProgress<int>>(), Arg.Any<CancellationToken>())
            .Throws(new GeneratorException());
        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Ok(true));
        _dialogService
            .ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>())
            .Returns(dialogReference);
        var systemUnderTest = GetRenderedComponent();

        var button = systemUnderTest.FindOrFail("button[title='3DWorld.Generate.Hover']");
        button.Click();

        _errorService.Received().SetError("An Error has occured during creation of a Backup File", Arg.Any<string>());
    }

    [Test]
    public void ExportButton_Clicked_SelectedWorldNull_Aborts()
    {
        _selectedViewModelsProvider.LearningWorld.Returns((LearningWorldViewModel)null!);

        var systemUnderTest = GetRenderedComponent();

        var button = systemUnderTest.FindOrFail("button[title='3DWorld.Generate.Hover']");
        button.Click();

        _dialogService.DidNotReceive().ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(),
            Arg.Any<DialogParameters>(), Arg.Any<DialogOptions>());
        _presentationLogic.DidNotReceive().ConstructAndUploadBackupAsync(Arg.Any<ILearningWorldViewModel>(),
            Arg.Any<IProgress<int>>(), Arg.Any<CancellationToken>());
        _snackbar.DidNotReceive().Add(Arg.Any<string>(), Arg.Any<Severity>());
    }

    [Test]
    public void ExportButton_Clicked_LMSNotConnected_Aborts()
    {
        var world = ViewModelProvider.GetLearningWorldWithSpaceWithElement();
        _selectedViewModelsProvider.LearningWorld.Returns(world);
        _presentationLogic.IsLmsConnected().Returns(false);

        var systemUnderTest = GetRenderedComponent();

        var button = systemUnderTest.FindOrFail("button[title='3DWorld.Generate.Hover']");
        button.Click();

        _dialogService.DidNotReceive().ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(),
            Arg.Any<DialogParameters>(), Arg.Any<DialogOptions>());
        _presentationLogic.DidNotReceive().ConstructAndUploadBackupAsync(Arg.Any<ILearningWorldViewModel>(),
            Arg.Any<IProgress<int>>(), Arg.Any<CancellationToken>());
        _snackbar.DidNotReceive().Add(Arg.Any<string>(), Arg.Any<Severity>());
    }

    [Test]
    public void ExportButton_Clicked_CancelDialog_Aborts()
    {
        var world = ViewModelProvider.GetLearningWorldWithSpaceWithElement();
        _selectedViewModelsProvider.LearningWorld.Returns(world);
        _presentationLogic.IsLmsConnected().Returns(true);

        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Cancel());
        _dialogService
            .ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>())
            .Returns(dialogReference);

        var systemUnderTest = GetRenderedComponent();
        var button = systemUnderTest.FindOrFail("button[title='3DWorld.Generate.Hover']");
        button.Click();

        _presentationLogic.DidNotReceive().ConstructAndUploadBackupAsync(Arg.Any<ILearningWorldViewModel>(),
            Arg.Any<IProgress<int>>(), Arg.Any<CancellationToken>());
        _snackbar.DidNotReceive().Add(Arg.Any<string>(), Arg.Any<Severity>());
    }

    [Test]
    // ANF-ID: [AHO22]
    public void ExportButton_Clicked_ExistingWorld_Replace_CallsPresentationLogic()
    {
        var world = new LearningWorldViewModel("a", "f", "d", "e", "f", "d", "h", "i", "j", "k");
        var space = new LearningSpaceViewModel("a", "f", Theme.CampusAschaffenburg, 1);
        var element = new LearningElementViewModel("a", ViewModelProvider.GetFileContent(), "s", "e", LearningElementDifficultyEnum.Easy,
            ElementModel.l_h5p_blackboard_1, points: 1);
        space.LearningSpaceLayout.LearningElements.Add(0, element);
        world.LearningSpaces.Add(space);
        _selectedViewModelsProvider.LearningWorld.Returns(world);
        _presentationLogic.IsLmsConnected().Returns(true);
        var lmsWorldList = new List<LmsWorldViewModel>
            { new LmsWorldViewModel { WorldId = 1, WorldName = world.Name } };
        _presentationLogic.GetLmsWorldList().Returns(lmsWorldList);
        var genericCancellationConfirmationDialogReference = Substitute.For<IDialogReference>();
        genericCancellationConfirmationDialogReference.Result.Returns(DialogResult.Ok(true));
        _dialogService
            .ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>())
            .Returns(genericCancellationConfirmationDialogReference);
        _presentationLogic.GetAllContent().Returns(new List<ILearningContentViewModel>(){element.LearningContent});
        var replaceCopyLmsWorldDialogReference = Substitute.For<IDialogReference>();
        replaceCopyLmsWorldDialogReference.Result.Returns(DialogResult.Ok(ReplaceCopyLmsWorldDialogResult.Replace));
        _dialogService
            .ShowAsync<ReplaceCopyLmsWorldDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>())
            .Returns(replaceCopyLmsWorldDialogReference);
        var systemUnderTest = GetRenderedComponent();

        var button = systemUnderTest.FindOrFail("button[title='3DWorld.Generate.Hover']");
        button.Click();
        _presentationLogic.Received().DeleteLmsWorld(lmsWorldList.First());
        _presentationLogic.Received()
            .ConstructAndUploadBackupAsync(world, Arg.Any<IProgress<int>>(), Arg.Any<CancellationToken>());
        _snackbar.Received().Add("Export.SnackBar.Message", Arg.Any<Severity>());
    }

    [Test]
    // ANF-ID: [AHO22]
    public void ExportButton_Clicked_ExistingWorld_Copy_CallsPresentationLogic()
    {
        var world = new LearningWorldViewModel("a", "f", "d", "e", "f", "d", "h", "i", "j", "k");
        var space = new LearningSpaceViewModel("a", "f", Theme.CampusAschaffenburg, 1);
        var element = new LearningElementViewModel("a", ViewModelProvider.GetLinkContent(), "s", "e", LearningElementDifficultyEnum.Easy,
            ElementModel.l_h5p_blackboard_1, points: 1);
        space.LearningSpaceLayout.LearningElements.Add(0, element);
        world.LearningSpaces.Add(space);
        _selectedViewModelsProvider.LearningWorld.Returns(world);
        _presentationLogic.IsLmsConnected().Returns(true);
        var lmsWorldList = new List<LmsWorldViewModel>
            { new LmsWorldViewModel { WorldId = 1, WorldName = world.Name } };
        _presentationLogic.GetLmsWorldList().Returns(lmsWorldList);
        var genericCancellationConfirmationDialogReference = Substitute.For<IDialogReference>();
        genericCancellationConfirmationDialogReference.Result.Returns(DialogResult.Ok(true));
        _dialogService
            .ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>())
            .Returns(genericCancellationConfirmationDialogReference);
        _presentationLogic.GetAllContent().Returns(new List<ILearningContentViewModel>(){element.LearningContent });
        var replaceCopyLmsWorldDialogReference = Substitute.For<IDialogReference>();
        replaceCopyLmsWorldDialogReference.Result.Returns(DialogResult.Ok(ReplaceCopyLmsWorldDialogResult.Copy));
        _dialogService
            .ShowAsync<ReplaceCopyLmsWorldDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>())
            .Returns(replaceCopyLmsWorldDialogReference);
        var systemUnderTest = GetRenderedComponent();

        var button = systemUnderTest.FindOrFail("button[title='3DWorld.Generate.Hover']");
        button.Click();
        _presentationLogic.DidNotReceive().DeleteLmsWorld(lmsWorldList.First());
        _presentationLogic.Received()
            .ConstructAndUploadBackupAsync(world, Arg.Any<IProgress<int>>(), Arg.Any<CancellationToken>());
        _snackbar.Received().Add("Export.SnackBar.Message", Arg.Any<Severity>());
    }

    [Test]
    public void ExportButton_Clicked_ExistingWorld_CancelReplaceCopyDialog_Aborts()
    {
        var world = new LearningWorldViewModel("a", "f", "d", "e", "f", "d", "h", "i", "j", "k");
        var space = new LearningSpaceViewModel("a", "f", Theme.CampusAschaffenburg, 1);
        var element = new LearningElementViewModel("a", null!, "s", "e", LearningElementDifficultyEnum.Easy,
            ElementModel.l_h5p_blackboard_1, points: 1);
        space.LearningSpaceLayout.LearningElements.Add(0, element);
        world.LearningSpaces.Add(space);
        _selectedViewModelsProvider.LearningWorld.Returns(world);
        _presentationLogic.IsLmsConnected().Returns(true);
        var lmsWorldList = new List<LmsWorldViewModel>
            { new LmsWorldViewModel { WorldId = 1, WorldName = world.Name } };
        _presentationLogic.GetLmsWorldList().Returns(lmsWorldList);
        var genericCancellationConfirmationDialogReference = Substitute.For<IDialogReference>();
        genericCancellationConfirmationDialogReference.Result.Returns(DialogResult.Ok(true));
        _dialogService
            .ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>())
            .Returns(genericCancellationConfirmationDialogReference);

        var replaceCopyLmsWorldDialogReference = Substitute.For<IDialogReference>();
        replaceCopyLmsWorldDialogReference.Result.Returns(DialogResult.Cancel());
        _dialogService
            .ShowAsync<ReplaceCopyLmsWorldDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>())
            .Returns(replaceCopyLmsWorldDialogReference);
        var systemUnderTest = GetRenderedComponent();

        var button = systemUnderTest.FindOrFail("button[title='3DWorld.Generate.Hover']");
        button.Click();
        _presentationLogic.DidNotReceive().DeleteLmsWorld(lmsWorldList.First());
        _presentationLogic.DidNotReceive()
            .ConstructAndUploadBackupAsync(world, Arg.Any<IProgress<int>>(), Arg.Any<CancellationToken>());
    }

    [Test]
    // ANF-ID: [ASN0003]
    public void UndoButton_Clicked_CallsPresentationLogic()
    {
        _presentationLogic.CanUndo.Returns(true);
        var systemUnderTest = GetRenderedComponent();

        systemUnderTest.FindComponentWithMarkup<MudIconButton>("undo")
            .Find("button").Click();

        _presentationLogic.Received().UndoCommand();
    }

    [Test]
    public void UndoButton_Clicked_UndoCommandThrowsUndoException_ErrorServiceCalled()
    {
        _presentationLogic.CanUndo.Returns(true);
        _presentationLogic.When(x => x.UndoCommand()).Do(_ => throw new UndoException());
        var systemUnderTest = GetRenderedComponent();

        systemUnderTest.FindComponentWithMarkup<MudIconButton>("undo")
            .Find("button").Click();

        _errorService.Received()
            .SetError("An error occurred while attempting to undo the last action", Arg.Any<string>());
    }

    [Test]
    // ANF-ID: [ASN0004]
    public void RedoButton_Clicked_CallsPresentationLogic()
    {
        _presentationLogic.CanRedo.Returns(true);
        var systemUnderTest = GetRenderedComponent();

        systemUnderTest.FindComponentWithMarkup<MudIconButton>("redo")
            .Find("button").Click();

        _presentationLogic.Received().RedoCommand();
    }

    [Test]
    public void RedoButton_Clicked_RedoCommandThrowsRedoException_ErrorServiceCalled()
    {
        _presentationLogic.CanRedo.Returns(true);
        _presentationLogic.When(x => x.RedoCommand()).Do(_ => throw new RedoException());
        var systemUnderTest = GetRenderedComponent();

        systemUnderTest.FindComponentWithMarkup<MudIconButton>("redo")
            .Find("button").Click();

        _errorService.Received().SetError("An error occurred while attempting to redo the last undone action",
            Arg.Any<string>());
    }

    private IRenderedComponent<HeaderBar> GetRenderedComponent()
    {
        return _testContext.RenderComponent<HeaderBar>();
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private TestContext _testContext;
    private IPresentationLogic _presentationLogic;
    private ISelectedViewModelsProvider _selectedViewModelsProvider;
    private IMediator _mediator;
    private IStringLocalizer<HeaderBar> _stringLocalizer;
    private ISnackbar _snackbar;
    private IDialogService _dialogService;
    private IErrorService _errorService;
    private ILogger _logger;
    private IShellWrapper _shellwrapper;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}