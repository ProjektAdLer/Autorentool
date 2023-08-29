using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using AngleSharp.Dom;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.ContentFiles;
using Presentation.Components.Dialogues;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningContent.FileContent;
using Presentation.PresentationLogic.LearningContent.LinkContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Mediator;
using TestHelpers;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components.ContentFiles;

[TestFixture]
public class ContentFilesViewUt
{
    private TestContext _testContext;
    private IPresentationLogic _presentationLogic;
    private IDialogService _dialogService;
    private IMediator _mediator;
    private IAuthoringToolWorkspaceViewModel _workspaceViewModel;
    private IStringLocalizer<ContentFilesView> _localizer;
    private IErrorService _errorService;

    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();

        _presentationLogic = Substitute.For<IPresentationLogic>();
        _dialogService = Substitute.For<IDialogService>();
        _mediator = Substitute.For<IMediator>();
        _workspaceViewModel = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        _localizer = Substitute.For<IStringLocalizer<ContentFilesView>>();
        _localizer[Arg.Any<string>()].Returns(ci => new LocalizedString(ci.Arg<string>(), ci.Arg<string>()));
        _localizer[Arg.Any<string>(), Arg.Any<object[]>()].Returns(ci =>
            new LocalizedString(ci.Arg<string>() + string.Concat(ci.Arg<object[]>()),
                ci.Arg<string>() + string.Concat(ci.Arg<object[]>())));
        _errorService = Substitute.For<IErrorService>();

        _testContext.Services.AddSingleton(_presentationLogic);
        _testContext.Services.AddSingleton(_dialogService);
        _testContext.Services.AddSingleton(_mediator);
        _testContext.Services.AddSingleton(_workspaceViewModel);
        _testContext.Services.AddSingleton(_localizer);
        _testContext.Services.AddSingleton(_errorService);

        _testContext.AddMudBlazorTestServices();
    }

    [Test]
    public void Constructor_InjectsDependencies()
    {
        var systemUnderTest = GetRenderedComponent();

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.PresentationLogic, Is.EqualTo(_presentationLogic));
            Assert.That(systemUnderTest.Instance.DialogService, Is.EqualTo(_dialogService));
            Assert.That(systemUnderTest.Instance.Mediator, Is.EqualTo(_mediator));
            Assert.That(systemUnderTest.Instance.WorkspaceViewModel, Is.EqualTo(_workspaceViewModel));
            Assert.That(systemUnderTest.Instance.Localizer, Is.EqualTo(_localizer));
            Assert.That(systemUnderTest.Instance.ErrorService, Is.EqualTo(_errorService));
        });
    }

    [Test]
    public void Render_PresentationLogicReturnsSomeItems_ItemsAreRendered()
    {
        var items = PresentationLogicSetItems();

        var systemUnderTest = GetRenderedComponent();

        var tableRows = systemUnderTest.FindAll("tbody tr");
        Assert.Multiple(() =>
        {
            Assert.That(tableRows, Has.Count.EqualTo(3));
            foreach (var (item, i) in items.Select((item, i) => (item, i)))
            {
                var deleteTd = tableRows[i].Children
                    .FirstOrDefault(child => child.Attributes["data-label"]?.Value == "Delete");
                Assert.That(deleteTd, Is.Not.Null);

                var nameTd = tableRows[i].Children
                    .FirstOrDefault(child => child.Attributes["data-label"]?.Value == "Name");
                var name = nameTd.Children.First(child => child.Matches("div")).Children
                    .First(child => child.Matches("p"))
                    .GetInnerText();
                Assert.That(name, Is.EqualTo(item.Name));

                var typeTd = tableRows[i].Children
                    .FirstOrDefault(child => child.Attributes["data-label"]?.Value == "Type");
                var type = typeTd.GetInnerText();
                Assert.That(type, Is.EqualTo(item is FileContentViewModel fc ? fc.Type : "Link"));

                var pathTd = tableRows[i].Children
                    .FirstOrDefault(child => child.Attributes["data-label"]?.Value == "Filepath/Link");
                Assert.That(pathTd, Is.Null);
            }
        });
    }

    [Test]
    public void ClickDelete_ShowsDialog_DeletesOnOkIfNotInWorlds()
    {
        var items = PresentationLogicSetItems();
        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Ok(true));
        _dialogService.ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>())
            .Returns(dialogReference);

        var systemUnderTest = GetRenderedComponent();

        var tableRows = systemUnderTest.FindAll("tbody tr");
        var deleteButton = tableRows.First().Children.First().Children.First();

        deleteButton.Click();

        _dialogService.Received(1).ShowAsync<GenericCancellationConfirmationDialog>("TaskDelete.DialogService.Title",
            Arg.Is<DialogParameters>(arg => (string)arg["DialogText"] == "Dialog.Delete.DialogTextfile1"),
            Arg.Any<DialogOptions>());
        _dialogService.DidNotReceiveWithAnyArgs().ShowAsync<DeleteContentInUseConfirmationDialog>();
        _presentationLogic.Received(1).RemoveContent(items.First());
    }
    
    [Test]
    public void ClickDelete_ArgumentOutOfRangeException_CallsErrorService()
    {
        var items = PresentationLogicSetItems();
        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Ok(true));
        _dialogService.ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>())
            .Returns(dialogReference);
        
        _presentationLogic.When(x => x.RemoveContent(items.First())).Throw(new ArgumentOutOfRangeException());

        var systemUnderTest = GetRenderedComponent();

        var tableRows = systemUnderTest.FindAll("tbody tr");
        var deleteButton = tableRows.First().Children.First().Children.First();

        deleteButton.Click();

       _errorService.Received(1).SetError("Error deleting content", Arg.Any<string>());
    }
    
    [Test]
    public void ClickDelete_FileNotFoundException_CallsErrorService()
    {
        var items = PresentationLogicSetItems();
        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Ok(true));
        _dialogService.ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>())
            .Returns(dialogReference);
        
        _presentationLogic.When(x => x.RemoveContent(items.First())).Throw(new FileNotFoundException("test"));

        var systemUnderTest = GetRenderedComponent();

        var tableRows = systemUnderTest.FindAll("tbody tr");
        var deleteButton = tableRows.First().Children.First().Children.First();

        deleteButton.Click();

        _errorService.Received(1).SetError("Error deleting content", "test");
    }
    
    [Test]
    public void ClickDelete_SerializationException_CallsErrorService()
    {
        var items = PresentationLogicSetItems();
        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Ok(true));
        _dialogService.ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>())
            .Returns(dialogReference);
        
        _presentationLogic.When(x => x.RemoveContent(items.First())).Throw(new SerializationException("test"));

        var systemUnderTest = GetRenderedComponent();

        var tableRows = systemUnderTest.FindAll("tbody tr");
        var deleteButton = tableRows.First().Children.First().Children.First();

        deleteButton.Click();

        _errorService.Received(1).SetError("Error deleting content", "test");
    }

    [Test]
    public void ClickDelete_ShowsDialog_ShowsAnotherDialogWhenInWorld_CancelDoesNotDelete()
    {
        var items = PresentationLogicSetItems();
        var world = ViewModelProvider.GetLearningWorldWithSpaceWithElement();
        world.LearningSpaces.First().ContainedLearningElements.First().LearningContent = items.First();
        _workspaceViewModel.LearningWorlds.Returns(new ILearningWorldViewModel[] { world });
        var dialogReference1 = Substitute.For<IDialogReference>();
        dialogReference1.Result.Returns(DialogResult.Ok(true));
        var dialogReference2 = Substitute.For<IDialogReference>();
        dialogReference2.Result.Returns(DialogResult.Cancel());
        _dialogService.ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>())
            .Returns(dialogReference1);
        _dialogService.ShowAsync<DeleteContentInUseConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>())
            .Returns(dialogReference2);

        var systemUnderTest = GetRenderedComponent();

        var tableRows = systemUnderTest.FindAll("tbody tr");
        var deleteButton = tableRows.First().Children.First().Children.First();

        deleteButton.Click();

        _dialogService.Received(1).ShowAsync<GenericCancellationConfirmationDialog>("TaskDelete.DialogService.Title",
            Arg.Is<DialogParameters>(arg => (string)arg["DialogText"] == "Dialog.Delete.DialogTextfile1"),
            Arg.Any<DialogOptions>());
        
        Expression<Predicate<DialogParameters>> secondDialogParametersPredicate = d =>
            (string)d[nameof(DeleteContentInUseConfirmationDialog.ContentName)] == "file1" &&
            ((IEnumerable<(ILearningWorldViewModel, ILearningElementViewModel)>)d[
                nameof(DeleteContentInUseConfirmationDialog.WorldElementInUseTuples)])
            .Any(tup =>
                tup.Item1 == world &&
                tup.Item2 == world.LearningSpaces.First().ContainedLearningElements.First());
        
        _dialogService.Received(1).ShowAsync<DeleteContentInUseConfirmationDialog>("WarningDialog.Title",
            Arg.Is(secondDialogParametersPredicate),
            Arg.Any<DialogOptions>());
        _presentationLogic.DidNotReceiveWithAnyArgs().RemoveContent(Arg.Any<ILearningContentViewModel>());
    }

    [Test]
    public void EnterSearchString_FiltersCorrectly()
    {
        PresentationLogicSetItems();
        var searchString = "file1";
        
        var systemUnderTest = GetRenderedComponent();

        systemUnderTest.Instance.SearchString = searchString;
        
        systemUnderTest.Render();
        var tableRows = systemUnderTest.FindAll("tbody tr");
        Assert.That(tableRows, Has.Count.EqualTo(1));
    }

    [Test]
    public void ShowFilepathSwitchPressed_ShowsFilepathOrLink()
    {
        var items = PresentationLogicSetItems();
        
        var systemUnderTest = GetRenderedComponent();
        var mudSwitch = systemUnderTest.FindComponent<MudSwitch<bool>>();
        var showFilepathSwitch = mudSwitch.Find("input");
        showFilepathSwitch.Change(true);
        
        var tableRows = systemUnderTest.FindAll("tbody tr");
        Assert.Multiple(() =>
        {
            Assert.That(tableRows, Has.Count.EqualTo(3));
            foreach (var (item, i) in items.Select((item, i) => (item, i)))
            {
                var pathTd = tableRows[i].Children
                    .FirstOrDefault(child => child.Attributes["data-label"]?.Value == "Filepath/Link");
                var path = pathTd.GetInnerText();
                Assert.That(path, Is.EqualTo(item is FileContentViewModel fc ? fc.Filepath : ((LinkContentViewModel)item).Link));
            }
        });
    }

    [Test]
    public void ClickNewElementButton_CallsPresentationLogicAndMediator()
    {
        var items = PresentationLogicSetItems();
        
        var systemUnderTest = GetRenderedComponent();
        
        var newElementButton = systemUnderTest.Find("button.create-element-with-content");
        newElementButton.Click();
        _presentationLogic.Received().SetSelectedLearningContentViewModel(items.First());
        _mediator.Received().RequestOpenNewElementDialog();
    }

    [Test]
    public void ClickPreviewButton_CallsPresentationLogic()
    {
        var items = PresentationLogicSetItems();
        
        var systemUnderTest = GetRenderedComponent();
        
        var previewButton = systemUnderTest.Find("button.show-content-preview");
        previewButton.Click();
        _presentationLogic.Received().ShowLearningContentAsync(items.First());
    }

    private IEnumerable<ILearningContentViewModel> PresentationLogicSetItems()
    {
        var items = new ILearningContentViewModel[]
        {
            new FileContentViewModel("file1", "type1", "path1"),
            new FileContentViewModel("file2", "type2", "path2"),
            new LinkContentViewModel("link1", "http://link1.com")
        };
        _presentationLogic.GetAllContent().Returns(items);
        return items;
    }

    private IRenderedComponent<ContentFilesView> GetRenderedComponent()
    {
        return _testContext.RenderComponent<ContentFilesView>();
    }
}