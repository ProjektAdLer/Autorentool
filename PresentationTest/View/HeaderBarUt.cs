using System;
using System.IO;
using System.Linq;
using System.Text;
using Bunit;
using Bunit.Rendering;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Presentation.Components;
using Presentation.Components.Culture;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Mediator;
using Presentation.PresentationLogic.SelectedViewModels;
using Presentation.View;
using Shared;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View;

[TestFixture]
public class HeaderBarUt
{
    private TestContext _testContext;
    private IPresentationLogic _presentationLogic;
    private ISelectedViewModelsProvider _selectedViewModelsProvider;
    private IMediator _mediator;
    private IStringLocalizer<HeaderBar> _stringLocalizer;
    private ISnackbar _snackbar;
    private IDialogService _dialogService;
    private IErrorService _errorService;

    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
        _testContext.ComponentFactories.AddStub<CloseAppButton>();
        _testContext.ComponentFactories.AddStub<CultureSelector>();
        _testContext.ComponentFactories.AddStub<LmsLoginButton>();
        _testContext.ComponentFactories.AddStub<MudPopover>();
        _testContext.ComponentFactories.AddStub<MudIconButton>();
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
        _testContext.Services.AddSingleton(_presentationLogic);
        _testContext.Services.AddSingleton(_stringLocalizer);
        _testContext.Services.AddSingleton(_selectedViewModelsProvider);
        _testContext.Services.AddSingleton(_mediator);
        _testContext.Services.AddSingleton(_snackbar);
        _testContext.Services.AddSingleton(_dialogService);
        _testContext.Services.AddSingleton(_errorService);
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

        var element = systemUnderTest.Find("header div h1");
        element.MarkupMatches(@"<h1 class=""font-bold text-lg"">TestName v3</h1>");
    }

    [Test]
    public void ExportButton_Clicked_CallsPresentationLogic()
    {
        var world = new LearningWorldViewModel("a", "f", "d", "e", "f", "d");
        var space = new LearningSpaceViewModel("a", "f", "d", Theme.Campus, 1);
        var element = new LearningElementViewModel("a", null!, "s", "e", LearningElementDifficultyEnum.Easy,
            ElementModel.l_h5p_blackboard_1, points: 1);
        space.LearningSpaceLayout.LearningElements.Add(0, element);
        world.LearningSpaces.Add(space);
        _selectedViewModelsProvider.LearningWorld.Returns(world);
        var systemUnderTest = GetRenderedComponent();

        var button = systemUnderTest.FindOrFail("button[title='3DWorld.Generate.Hover']");
        button.Click();
        _presentationLogic.Received().ConstructBackupAsync(world);
        _snackbar.Received().Add(Arg.Is<string>(s => s == "Export.SnackBar.Message "), Arg.Any<Severity>());
    }

    [Test]
    public void ExportButton_Clicked_WorldHasNoSpaces_ErrorServiceCalled()
    {
        var world = new LearningWorldViewModel("a", "f", "d", "e", "f", "d");
        _selectedViewModelsProvider.LearningWorld.Returns(world);
        var systemUnderTest = GetRenderedComponent();

        var button = systemUnderTest.FindOrFail("button[title='3DWorld.Generate.Hover']");
        button.Click();

        var mockStringBuilder = new StringBuilder();
        mockStringBuilder.Append("<ul><li>");
        mockStringBuilder.AppendLine(" ErrorString.Missing.LearningSpace.Message </li>");
        mockStringBuilder.Append("</ul>");


        _errorService.Received().SetError("Exception.InvalidLearningWorld.Message", mockStringBuilder.ToString());
    }

    [Test]
    public void ExportButton_Clicked_WorldSpaceHasNoElementsAndInsufficientPoints_ErrorServiceCalled()
    {
        var world = new LearningWorldViewModel("a", "f", "d", "e", "f", "d");
        var space1 = new LearningSpaceViewModel("a", "f", "d", Theme.Campus, 2);
        var space2 = new LearningSpaceViewModel("ah", "fi", "dh", Theme.Campus, 3);
        var element1 = new LearningElementViewModel("a", null!, "s", "e", LearningElementDifficultyEnum.Easy,
            ElementModel.l_h5p_blackboard_1, points: 1);
        space1.LearningSpaceLayout.LearningElements.Add(0, element1);
        world.LearningSpaces.Add(space1);
        world.LearningSpaces.Add(space2);
        _selectedViewModelsProvider.LearningWorld.Returns(world);
        var systemUnderTest = GetRenderedComponent();

        var button = systemUnderTest.FindOrFail("button[title='3DWorld.Generate.Hover']");
        button.Click();

        var mockStringBuilder = new StringBuilder();
        mockStringBuilder.Append("<ul>");
        mockStringBuilder.AppendLine($"<li> ErrorString.Insufficient.Points.Message {space1.Name} </li>");
        mockStringBuilder.AppendLine($"<li> ErrorString.Missing.LearningElements.Message {space2.Name} </li>");
        mockStringBuilder.AppendLine($"<li> ErrorString.Insufficient.Points.Message {space2.Name} </li>");
        mockStringBuilder.Append("</ul>");

        _errorService.Received().SetError("Exception.InvalidLearningWorld.Message", mockStringBuilder.ToString());
    }

    private IRenderedComponent<HeaderBar> GetRenderedComponent()
    {
        return _testContext.RenderComponent<HeaderBar>();
    }

    [Test]
    public void ExportButton_Clicked_ConstructBackupThrowsOperationCanceledException_SnackbarWarningAdded()
    {
        var world = new LearningWorldViewModel("a", "f", "d", "e", "f", "d");
        var space = new LearningSpaceViewModel("a", "f", "d", Theme.Campus, 1);
        var element = new LearningElementViewModel("a", null!, "s", "e", LearningElementDifficultyEnum.Easy,
            ElementModel.l_h5p_blackboard_1, points: 1);
        space.LearningSpaceLayout.LearningElements.Add(0, element);
        world.LearningSpaces.Add(space);
        _selectedViewModelsProvider.LearningWorld.Returns(world);
        _presentationLogic.ConstructBackupAsync(world).Throws(new OperationCanceledException());
        var systemUnderTest = GetRenderedComponent();

        var button = systemUnderTest.FindOrFail("button[title='3DWorld.Generate.Hover']");
        button.Click();

        _snackbar.Received().Add(Arg.Is<string>(s => s == "ExportCanceled.Snackbar.Message"), Arg.Any<Severity>());
    }

    [Test]
    public void ExportButton_Clicked_ConstructBackupThrowsArgumentOutOfRangeException_ErrorServiceCalled()
    {
        var world = new LearningWorldViewModel("a", "f", "d", "e", "f", "d");
        var space = new LearningSpaceViewModel("a", "f", "d", Theme.Campus, 1);
        var element = new LearningElementViewModel("a", null!, "s", "e", LearningElementDifficultyEnum.Easy,
            ElementModel.l_h5p_blackboard_1, points: 1);
        space.LearningSpaceLayout.LearningElements.Add(0, element);
        world.LearningSpaces.Add(space);
        _selectedViewModelsProvider.LearningWorld.Returns(world);
        _presentationLogic.ConstructBackupAsync(world).Throws(new ArgumentOutOfRangeException());
        var systemUnderTest = GetRenderedComponent();

        var button = systemUnderTest.FindOrFail("button[title='3DWorld.Generate.Hover']");
        button.Click();

        _errorService.Received().SetError("An Error has occured during creation of a Backup File", Arg.Any<string>());
    }

    [Test]
    public void ExportButton_Clicked_ConstructBackupThrowsFileNotFoundException_ErrorServiceCalled()
    {
        var world = new LearningWorldViewModel("a", "f", "d", "e", "f", "d");
        var space = new LearningSpaceViewModel("a", "f", "d", Theme.Campus, 1);
        var element = new LearningElementViewModel("a", null!, "s", "e", LearningElementDifficultyEnum.Easy,
            ElementModel.l_h5p_blackboard_1, points: 1);
        space.LearningSpaceLayout.LearningElements.Add(0, element);
        world.LearningSpaces.Add(space);
        _selectedViewModelsProvider.LearningWorld.Returns(world);
        _presentationLogic.ConstructBackupAsync(world).Throws(new FileNotFoundException());
        var systemUnderTest = GetRenderedComponent();

        var button = systemUnderTest.FindOrFail("button[title='3DWorld.Generate.Hover']");
        button.Click();

        _errorService.Received().SetError("An Error has occured during creation of a Backup File", Arg.Any<string>());
    }
}