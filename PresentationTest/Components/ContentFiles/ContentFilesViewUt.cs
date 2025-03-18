using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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

        _testContext.ComponentFactories.AddStub<MudMenu>();
        _testContext.ComponentFactories.AddStub<MudMenuItem>();
        _testContext.ComponentFactories.AddStub<MudPopover>();

        _testContext.Services.AddSingleton(_presentationLogic);
        _testContext.Services.AddSingleton(_dialogService);
        _testContext.Services.AddSingleton(_mediator);
        _testContext.Services.AddSingleton(_workspaceViewModel);
        _testContext.Services.AddSingleton(_localizer);
        _testContext.Services.AddSingleton(_errorService);

        _testContext.AddMudBlazorTestServices();
    }

    [TearDown]
    public void TearDown()
    {
        _testContext.Dispose();
    }

    private TestContext _testContext;
    private IPresentationLogic _presentationLogic;
    private IDialogService _dialogService;
    private IMediator _mediator;
    private IAuthoringToolWorkspaceViewModel _workspaceViewModel;
    private IStringLocalizer<ContentFilesView> _localizer;
    private IErrorService _errorService;

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
                Assert.That(nameTd, Is.Not.Null);
                var name = nameTd!.Children.First(child => child.Matches("div")).Children
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
    // ANF-ID: [AWA0037, AWA0043]
    public void ClickDelete_ShowsDialog_DeletesOnOkIfNotInWorlds()
    {
        var items = PresentationLogicSetItems().ToList();
        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Ok(true));
        _dialogService.ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>())
            .Returns(dialogReference);

        var systemUnderTest = GetRenderedComponent();

        var mudPopovers = systemUnderTest.FindComponentsOrFail<Stub<MudPopover>>();
        var hoverMenu = mudPopovers.First(x => x.Instance.Parameters["AnchorOrigin"].ToString() == "CenterRight");
        var hoverMenuContent = _testContext.Render((RenderFragment)hoverMenu.Instance.Parameters["ChildContent"]);
        var deleteButton = hoverMenuContent.FindAll("button").FirstOrDefault(x =>
            x.Attributes["title"] != null && x.Attributes["title"]!.Value.Contains("Delete"));
        Assert.That(deleteButton, Is.Not.Null);

        deleteButton!.Click();

        _dialogService.Received(1).ShowAsync<GenericCancellationConfirmationDialog>("TaskDelete.DialogService.Title",
            Arg.Is<DialogParameters>(arg => (string)arg["DialogText"]! == "Dialog.Delete.DialogTextfile1"),
            Arg.Any<DialogOptions>());
        _dialogService.DidNotReceiveWithAnyArgs().ShowAsync<DeleteContentInUseConfirmationDialog>();
        _presentationLogic.Received(1).DeleteContent(TODO, items.First());
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

        _presentationLogic.When(x => x.DeleteContent(TODO, items.First())).Throw(new ArgumentOutOfRangeException());

        var systemUnderTest = GetRenderedComponent();

        var mudPopovers = systemUnderTest.FindComponentsOrFail<Stub<MudPopover>>();
        var hoverMenu = mudPopovers.First(x => x.Instance.Parameters["AnchorOrigin"].ToString() == "CenterRight");
        var hoverMenuContent = _testContext.Render((RenderFragment)hoverMenu.Instance.Parameters["ChildContent"]);
        var deleteButton = hoverMenuContent.FindAll("button").FirstOrDefault(x =>
            x.Attributes["title"] != null && x.Attributes["title"]!.Value.Contains("Delete"));
        Assert.That(deleteButton, Is.Not.Null);

        deleteButton!.Click();

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

        _presentationLogic.When(x => x.DeleteContent(TODO, items.First())).Throw(new FileNotFoundException("test"));

        var systemUnderTest = GetRenderedComponent();

        var mudPopovers = systemUnderTest.FindComponentsOrFail<Stub<MudPopover>>();
        var hoverMenu = mudPopovers.First(x => x.Instance.Parameters["AnchorOrigin"].ToString() == "CenterRight");
        var hoverMenuContent = _testContext.Render((RenderFragment)hoverMenu.Instance.Parameters["ChildContent"]);
        var deleteButton = hoverMenuContent.FindAll("button").FirstOrDefault(x =>
            x.Attributes["title"] != null && x.Attributes["title"]!.Value.Contains("Delete"));
        Assert.That(deleteButton, Is.Not.Null);

        deleteButton!.Click();

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

        _presentationLogic.When(x => x.DeleteContent(TODO, items.First())).Throw(new SerializationException("test"));

        var systemUnderTest = GetRenderedComponent();

        var mudPopovers = systemUnderTest.FindComponentsOrFail<Stub<MudPopover>>();
        var hoverMenu = mudPopovers.First(x => x.Instance.Parameters["AnchorOrigin"].ToString() == "CenterRight");
        var hoverMenuContent = _testContext.Render((RenderFragment)hoverMenu.Instance.Parameters["ChildContent"]);
        var deleteButton = hoverMenuContent.FindAll("button").FirstOrDefault(x =>
            x.Attributes["title"] != null && x.Attributes["title"]!.Value.Contains("Delete"));
        Assert.That(deleteButton, Is.Not.Null);

        deleteButton!.Click();

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

        var mudPopovers = systemUnderTest.FindComponentsOrFail<Stub<MudPopover>>();
        var hoverMenu = mudPopovers.First(x => x.Instance.Parameters["AnchorOrigin"].ToString() == "CenterRight");
        var hoverMenuContent = _testContext.Render((RenderFragment)hoverMenu.Instance.Parameters["ChildContent"]);
        var deleteButton = hoverMenuContent.FindAll("button").FirstOrDefault(x =>
            x.Attributes["title"] != null && x.Attributes["title"]!.Value.Contains("Delete"));
        Assert.That(deleteButton, Is.Not.Null);

        deleteButton!.Click();

        _dialogService.Received(1).ShowAsync<GenericCancellationConfirmationDialog>("TaskDelete.DialogService.Title",
            Arg.Is<DialogParameters>(arg => (string)arg["DialogText"]! == "Dialog.Delete.DialogTextfile1"),
            Arg.Any<DialogOptions>());

        Expression<Predicate<DialogParameters>> secondDialogParametersPredicate = d =>
            (string)d[nameof(DeleteContentInUseConfirmationDialog.ContentName)]! == "file1" &&
            ((IEnumerable<(ILearningWorldViewModel, ILearningElementViewModel)>)d[
                nameof(DeleteContentInUseConfirmationDialog.WorldElementInUseTuples)]!)
            .Any(tup =>
                tup.Item1 == world &&
                tup.Item2 == world.LearningSpaces.First().ContainedLearningElements.First());

        _dialogService.Received(1).ShowAsync<DeleteContentInUseConfirmationDialog>("WarningDialog.Title",
            Arg.Is(secondDialogParametersPredicate),
            Arg.Any<DialogOptions>());
        _presentationLogic.DidNotReceiveWithAnyArgs().DeleteContent(TODO, Arg.Any<ILearningContentViewModel>());
    }

    [Test]
    // ANF-ID: [AWA0037, AWA0043]
    public void ClickDelete_FirstDialogCancelled_DoesNotShowSecondDialog()
    {
        var items = PresentationLogicSetItems();
        var world = ViewModelProvider.GetLearningWorldWithSpaceWithElement();
        world.LearningSpaces.First().ContainedLearningElements.First().LearningContent = items.First();
        _workspaceViewModel.LearningWorlds.Returns(new ILearningWorldViewModel[] { world });
        var dialogReference1 = Substitute.For<IDialogReference>();
        dialogReference1.Result.Returns(DialogResult.Cancel());
        _dialogService.ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>())
            .Returns(dialogReference1);

        var systemUnderTest = GetRenderedComponent();

        var mudPopovers = systemUnderTest.FindComponentsOrFail<Stub<MudPopover>>();
        var hoverMenu = mudPopovers.First(x => x.Instance.Parameters["AnchorOrigin"].ToString() == "CenterRight");
        var hoverMenuContent = _testContext.Render((RenderFragment)hoverMenu.Instance.Parameters["ChildContent"]);
        var deleteButton = hoverMenuContent.FindAll("button").FirstOrDefault(x =>
            x.Attributes["title"] != null && x.Attributes["title"]!.Value.Contains("Delete"));
        Assert.That(deleteButton, Is.Not.Null);

        deleteButton!.Click();

        _dialogService.Received(1).ShowAsync<GenericCancellationConfirmationDialog>("TaskDelete.DialogService.Title",
            Arg.Is<DialogParameters>(arg => (string)arg["DialogText"]! == "Dialog.Delete.DialogTextfile1"),
            Arg.Any<DialogOptions>());
        _dialogService.DidNotReceiveWithAnyArgs().ShowAsync<DeleteContentInUseConfirmationDialog>(Arg.Any<string>(),
            Arg.Any<DialogParameters>(), Arg.Any<DialogOptions>());
    }

    [Test]
    public void SelectionCheckbox_Click_ChangeToCorrectState()
    {
        _ = PresentationLogicSetItems();
        var world = ViewModelProvider.GetLearningWorld();
        _workspaceViewModel.LearningWorlds.Returns(new ILearningWorldViewModel[] { world });

        var systemUnderTest = GetRenderedComponent();

        var checkboxes = systemUnderTest.FindComponents<MudCheckBox<bool>>();
        Assert.That(checkboxes, Has.Count.EqualTo(3));
        foreach (var checkbox in checkboxes)
        {
            Assert.That(checkbox.Instance.Value, Is.EqualTo(false));
        }

        var selectionCheckbox = systemUnderTest.FindComponent<MudCheckBox<bool?>>();
        var selectionCheckboxElement = selectionCheckbox.Find("input");
        Assert.That(selectionCheckbox.Instance.Value, Is.EqualTo(false));

        checkboxes[0].Find("input").Change(true);
        Assert.Multiple(() =>
        {
            Assert.That(checkboxes[0].Instance.Value, Is.EqualTo(true));
            Assert.That(checkboxes[1].Instance.Value, Is.EqualTo(false));
            Assert.That(checkboxes[2].Instance.Value, Is.EqualTo(false));

            Assert.That(selectionCheckbox.Instance.Value, Is.EqualTo(null));
        });

        selectionCheckboxElement.Change(true);

        Assert.That(selectionCheckbox.Instance.Value, Is.EqualTo(true));
        foreach (var checkbox in checkboxes)
        {
            Assert.That(checkbox.Instance.Value, Is.EqualTo(true));
        }

        selectionCheckboxElement.Change(true);
        Assert.That(selectionCheckbox.Instance.Value, Is.EqualTo(false));
        foreach (var checkbox in checkboxes)
        {
            Assert.That(checkbox.Instance.Value, Is.EqualTo(false));
        }

        selectionCheckboxElement.Change(true);
        Assert.That(selectionCheckbox.Instance.Value, Is.EqualTo(true));
        foreach (var checkbox in checkboxes)
        {
            Assert.That(checkbox.Instance.Value, Is.EqualTo(true));
        }
    }

    [Test]
    public async Task SelectDropdown_SelectAll_AllContentIsSelected()
    {
        var items = PresentationLogicSetItems().ToArray();
        var world = ViewModelProvider.GetLearningWorldWithSpaceWithElement();
        var world2 = ViewModelProvider.GetLearningWorldWithSpaceWithElement();
        _workspaceViewModel.LearningWorlds.Returns(new ILearningWorldViewModel[] { world, world2 });
        world.LearningSpaces.First().ContainedLearningElements.First().LearningContent = items.First();
        world2.LearningSpaces.First().ContainedLearningElements.First().LearningContent = items.Last();

        var systemUnderTest = GetRenderedComponent();

        var multipleSelectMenu = systemUnderTest.FindComponentOrFail<Stub<MudMenu>>();
        var menuContent = _testContext.Render((RenderFragment)multipleSelectMenu.Instance.Parameters["ChildContent"]);
        var menuContentItems = menuContent.FindComponentsOrFail<Stub<MudMenuItem>>();
        var selectAllMenuItem = (EventCallback<MouseEventArgs>)menuContentItems.First().Instance.Parameters["OnClick"];

        var checkboxes = systemUnderTest.FindComponents<MudCheckBox<bool>>();
        Assert.That(checkboxes, Has.Count.EqualTo(3));
        foreach (var checkbox in checkboxes)
        {
            Assert.That(checkbox.Instance.Value, Is.EqualTo(false));
        }

        await _testContext.Renderer.Dispatcher.InvokeAsync(() =>
            selectAllMenuItem.InvokeAsync(new MouseEventArgs()));
        foreach (var checkbox in checkboxes)
        {
            Assert.That(checkbox.Instance.Value, Is.EqualTo(true));
        }
    }

    [Test]
    public async Task SelectDropdown_SelectAllUnused_AllUnusedContentIsSelected()
    {
        var items = PresentationLogicSetItems().ToArray();
        var world = ViewModelProvider.GetLearningWorldWithSpaceWithElement();
        var world2 = ViewModelProvider.GetLearningWorldWithSpaceWithElement();
        _workspaceViewModel.LearningWorlds.Returns(new ILearningWorldViewModel[] { world, world2 });
        world.LearningSpaces.First().ContainedLearningElements.First().LearningContent = items.First();
        world2.LearningSpaces.First().ContainedLearningElements.First().LearningContent = items.Last();

        var systemUnderTest = GetRenderedComponent();

        var multipleSelectMenu = systemUnderTest.FindComponentOrFail<Stub<MudMenu>>();
        var menuContent = _testContext.Render((RenderFragment)multipleSelectMenu.Instance.Parameters["ChildContent"]);
        var menuContentItems = menuContent.FindComponentsOrFail<Stub<MudMenuItem>>();
        var selectAllUnusedMenuItem =
            (EventCallback<MouseEventArgs>)menuContentItems.Last().Instance.Parameters["OnClick"];

        var checkboxes = systemUnderTest.FindComponents<MudCheckBox<bool>>();
        Assert.That(checkboxes, Has.Count.EqualTo(3));
        foreach (var checkbox in checkboxes)
        {
            Assert.That(checkbox.Instance.Value, Is.EqualTo(false));
        }

        await _testContext.Renderer.Dispatcher.InvokeAsync(() =>
            selectAllUnusedMenuItem.InvokeAsync(new MouseEventArgs()));
        Assert.Multiple(() =>
        {
            Assert.That(checkboxes[0].Instance.Value, Is.EqualTo(false));
            Assert.That(checkboxes[1].Instance.Value, Is.EqualTo(true));
            Assert.That(checkboxes[2].Instance.Value, Is.EqualTo(false));
        });
    }

    [Test]
    // ANF-ID: [AWA0037, AWA0043]
    public void DeleteMultiple_SelectedNone_ButtonIsDisabled()
    {
        var items = PresentationLogicSetItems();
        var world = ViewModelProvider.GetLearningWorldWithSpaceWithElement();
        world.LearningSpaces.First().ContainedLearningElements.First().LearningContent = items.First();
        _workspaceViewModel.LearningWorlds.Returns(new ILearningWorldViewModel[] { world });
        var dialogReference1 = Substitute.For<IDialogReference>();
        dialogReference1.Result.Returns(DialogResult.Cancel());
        _dialogService.ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>())
            .Returns(dialogReference1);

        var systemUnderTest = GetRenderedComponent();

        var iconButtons = systemUnderTest.FindComponent<MudTable<ILearningContentViewModel>>()
            .FindComponents<MudIconButton>();
        // the contained MudIconButtons are: delete, first page, previous page, next page, last page
        Assert.That(iconButtons, Has.Count.EqualTo(5));
        var deleteMultipleButton = iconButtons[0];
        Assert.Multiple(() =>
        {
            Assert.That(deleteMultipleButton.Instance.Icon, Is.EqualTo(Icons.Material.Filled.Delete));
            Assert.That(deleteMultipleButton, Is.Not.Null);
            Assert.That(deleteMultipleButton.Instance.Disabled, Is.EqualTo(true));
        });
    }

    [Test]
    // ANF-ID: [AWA0037, AWA0043]
    public async Task DeleteMultiple_CancelFirstDialog_CallsNothing()
    {
        var items = PresentationLogicSetItems();
        var world = ViewModelProvider.GetLearningWorldWithSpaceWithElement();
        world.LearningSpaces.First().ContainedLearningElements.First().LearningContent = items.First();
        _workspaceViewModel.LearningWorlds.Returns(new ILearningWorldViewModel[] { world });
        var dialogReference1 = Substitute.For<IDialogReference>();
        dialogReference1.Result.Returns(DialogResult.Cancel());
        _dialogService.ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>())
            .Returns(dialogReference1);

        var systemUnderTest = GetRenderedComponent();

        var checkboxes = systemUnderTest.FindComponents<MudCheckBox<bool>>();
        foreach (var checkbox in checkboxes)
        {
            Assert.That(checkbox.Instance.Value, Is.EqualTo(false));
        }

        systemUnderTest.FindComponent<MudCheckBox<bool?>>().Find("input").Change(true);

        foreach (var checkbox in checkboxes)
        {
            Assert.That(checkbox.Instance.Value, Is.EqualTo(true));
        }

        systemUnderTest.FindComponent<MudTable<ILearningContentViewModel>>()
            .FindComponent<MudIconButton>().Find("button").Click();

        await _dialogService.Received()
            .ShowAsync<GenericCancellationConfirmationDialog>("TaskDelete.DialogService.Title",
                Arg.Any<DialogParameters>(), Arg.Any<DialogOptions>());

        foreach (var checkbox in checkboxes)
        {
            Assert.That(checkbox.Instance.Value, Is.EqualTo(true));
        }
    }

    [Test]
    // ANF-ID: [AWA0037, AWA0043]
    public async Task DeleteMultiple_NoContentsInUse_ConfirmFirstDialog_CallsPresentationLogic()
    {
        var items = PresentationLogicSetItems();
        var world = ViewModelProvider.GetLearningWorldWithSpace();
        _workspaceViewModel.LearningWorlds.Returns(new ILearningWorldViewModel[] { world });
        var dialogReference1 = Substitute.For<IDialogReference>();
        dialogReference1.Result.Returns(DialogResult.Ok(true));
        _dialogService.ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>())
            .Returns(dialogReference1);

        var systemUnderTest = GetRenderedComponent();

        systemUnderTest.FindComponent<MudCheckBox<bool?>>().Find("input").Change(true);

        systemUnderTest.FindComponent<MudTable<ILearningContentViewModel>>()
            .FindComponent<MudIconButton>().Find("button").Click();

        await _dialogService.Received()
            .ShowAsync<GenericCancellationConfirmationDialog>("TaskDelete.DialogService.Title",
                Arg.Any<DialogParameters>(), Arg.Any<DialogOptions>());
        _presentationLogic.Received()
            .RemoveMultipleContents(Arg.Is<List<ILearningContentViewModel>>(x => x.SequenceEqual(items)));
    }

    [Test]
    // ANF-ID: [AWA0037, AWA0043]
    public async Task DeleteMultiple_ContentsInUse_SelectAllAndClickDelete_ConfirmFirstDialog_ShowsConfirmationDialog()
    {
        var items = PresentationLogicSetItems().ToArray();
        var world = ViewModelProvider.GetLearningWorldWithSpaceWithElement();
        var world2 = ViewModelProvider.GetLearningWorldWithSpaceWithElement();
        _workspaceViewModel.LearningWorlds.Returns(new ILearningWorldViewModel[] { world, world2 });
        world.LearningSpaces.First().ContainedLearningElements.First().LearningContent = items.First();
        world2.LearningSpaces.First().ContainedLearningElements.First().LearningContent = items.Last();
        var expectedDialogParameters =
            new List<(ILearningContentViewModel, ILearningWorldViewModel, ILearningElementViewModel)>
            {
                (items.First(), world, world.LearningSpaces.First().ContainedLearningElements.First()),
                (items.Last(), world2, world2.LearningSpaces.First().ContainedLearningElements.First())
            };

        var dialogReference1 = Substitute.For<IDialogReference>();
        dialogReference1.Result.Returns(DialogResult.Ok(true));
        _dialogService
            .ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>()).Returns(dialogReference1);

        var dialogReference2 = Substitute.For<IDialogReference>();
        dialogReference2.Result.Returns(DialogResult.Cancel());
        _dialogService
            .ShowAsync<DeleteMultipleContentConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>()).Returns(dialogReference2);

        var systemUnderTest = GetRenderedComponent();

        systemUnderTest.FindComponent<MudCheckBox<bool?>>().Find("input").Change(true);

        systemUnderTest.FindComponent<MudTable<ILearningContentViewModel>>()
            .FindComponent<MudIconButton>().Find("button").Click();

        await _dialogService.Received()
            .ShowAsync<GenericCancellationConfirmationDialog>("TaskDelete.DialogService.Title",
                Arg.Any<DialogParameters>(), Arg.Any<DialogOptions>());
        await _dialogService.Received()
            .ShowAsync<DeleteMultipleContentConfirmationDialog>("TaskDelete.DialogService.Title",
                Arg.Is<DialogParameters>(
                    x => ((List<(ILearningContentViewModel, ILearningWorldViewModel, ILearningElementViewModel)>)
                        x["ContentWorldElementInUseList"]!).SequenceEqual(expectedDialogParameters)),
                Arg.Any<DialogOptions>());
    }

    [Test]
    // ANF-ID: [AWA0037, AWA0043]
    public async Task DeleteMultiple_ContentsInUse_ConfirmFirstDialog_DeleteAll()
    {
        var items = PresentationLogicSetItems().ToArray();
        var world = ViewModelProvider.GetLearningWorldWithSpaceWithElement();
        var world2 = ViewModelProvider.GetLearningWorldWithSpaceWithElement();
        _workspaceViewModel.LearningWorlds.Returns(new ILearningWorldViewModel[] { world, world2 });
        world.LearningSpaces.First().ContainedLearningElements.First().LearningContent = items.First();
        world2.LearningSpaces.First().ContainedLearningElements.First().LearningContent = items.Last();
        var expectedDialogParameters =
            new List<(ILearningContentViewModel, ILearningWorldViewModel, ILearningElementViewModel)>
            {
                (items.First(), world, world.LearningSpaces.First().ContainedLearningElements.First()),
                (items.Last(), world2, world2.LearningSpaces.First().ContainedLearningElements.First())
            };

        var dialogReference1 = Substitute.For<IDialogReference>();
        dialogReference1.Result.Returns(DialogResult.Ok(true));
        _dialogService
            .ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>()).Returns(dialogReference1);

        var dialogReference2 = Substitute.For<IDialogReference>();
        dialogReference2.Result.Returns(DialogResult.Ok(true));
        _dialogService
            .ShowAsync<DeleteMultipleContentConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>()).Returns(dialogReference2);

        var systemUnderTest = GetRenderedComponent();

        systemUnderTest.FindComponent<MudCheckBox<bool?>>().Find("input").Change(true);

        systemUnderTest.FindComponent<MudTable<ILearningContentViewModel>>()
            .FindComponent<MudIconButton>().Find("button").Click();

        await _dialogService.Received()
            .ShowAsync<GenericCancellationConfirmationDialog>("TaskDelete.DialogService.Title",
                Arg.Any<DialogParameters>(), Arg.Any<DialogOptions>());
        await _dialogService.Received()
            .ShowAsync<DeleteMultipleContentConfirmationDialog>("TaskDelete.DialogService.Title",
                Arg.Is<DialogParameters>(
                    x => ((List<(ILearningContentViewModel, ILearningWorldViewModel, ILearningElementViewModel)>)
                        x["ContentWorldElementInUseList"]!).SequenceEqual(expectedDialogParameters)),
                Arg.Any<DialogOptions>());
        _presentationLogic.Received()
            .RemoveMultipleContents(Arg.Is<IEnumerable<ILearningContentViewModel>>(x => x.SequenceEqual(items)));
    }

    [Test]
    // ANF-ID: [AWA0037, AWA0043]
    public async Task DeleteMultiple_ContentsInUse_ConfirmFirstDialog_DeleteUnused()
    {
        var items = PresentationLogicSetItems().ToArray();
        var world = ViewModelProvider.GetLearningWorldWithSpaceWithElement();
        var world2 = ViewModelProvider.GetLearningWorldWithSpaceWithElement();
        _workspaceViewModel.LearningWorlds.Returns(new ILearningWorldViewModel[] { world, world2 });
        world.LearningSpaces.First().ContainedLearningElements.First().LearningContent = items.First();
        world2.LearningSpaces.First().ContainedLearningElements.First().LearningContent = items.Last();
        var expectedDialogParameters =
            new List<(ILearningContentViewModel, ILearningWorldViewModel, ILearningElementViewModel)>
            {
                (items.First(), world, world.LearningSpaces.First().ContainedLearningElements.First()),
                (items.Last(), world2, world2.LearningSpaces.First().ContainedLearningElements.First())
            };

        var dialogReference1 = Substitute.For<IDialogReference>();
        dialogReference1.Result.Returns(DialogResult.Ok(true));
        _dialogService
            .ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>()).Returns(dialogReference1);

        var dialogReference2 = Substitute.For<IDialogReference>();
        dialogReference2.Result.Returns(DialogResult.Ok(false));
        _dialogService
            .ShowAsync<DeleteMultipleContentConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>()).Returns(dialogReference2);

        var systemUnderTest = GetRenderedComponent();

        systemUnderTest.FindComponent<MudCheckBox<bool?>>().Find("input").Change(true);

        systemUnderTest.FindComponent<MudTable<ILearningContentViewModel>>()
            .FindComponent<MudIconButton>().Find("button").Click();

        await _dialogService.Received()
            .ShowAsync<GenericCancellationConfirmationDialog>("TaskDelete.DialogService.Title",
                Arg.Any<DialogParameters>(), Arg.Any<DialogOptions>());
        await _dialogService.Received()
            .ShowAsync<DeleteMultipleContentConfirmationDialog>("TaskDelete.DialogService.Title",
                Arg.Is<DialogParameters>(
                    x => ((List<(ILearningContentViewModel, ILearningWorldViewModel, ILearningElementViewModel)>)
                        x["ContentWorldElementInUseList"]!).SequenceEqual(expectedDialogParameters)),
                Arg.Any<DialogOptions>());
        _presentationLogic.Received()
            .RemoveMultipleContents(
                Arg.Is<IEnumerable<ILearningContentViewModel>>(x => x.SequenceEqual(new[] { items[1] })));
    }

    [Test]
    // ANF-ID: [AWA0037, AWA0043]
    public void DeleteMultiple_ArgumentOutOfRangeException_CallsErrorService()
    {
        PresentationLogicSetItems();
        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Ok(true));
        _dialogService.ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>())
            .Returns(dialogReference);

        _presentationLogic.When(x => x.RemoveMultipleContents(Arg.Any<IEnumerable<ILearningContentViewModel>>()))
            .Throw(new ArgumentOutOfRangeException());

        var systemUnderTest = GetRenderedComponent();

        systemUnderTest.FindComponent<MudCheckBox<bool?>>().Find("input").Change(true);

        systemUnderTest.FindComponent<MudTable<ILearningContentViewModel>>()
            .FindComponent<MudIconButton>().Find("button").Click();

        _errorService.Received(1).SetError("Error deleting content", Arg.Any<string>());
    }

    [Test]
    // ANF-ID: [AWA0037, AWA0043]
    public void DeleteMultiple_FileNotFoundException_CallsErrorService()
    {
        PresentationLogicSetItems();
        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Ok(true));
        _dialogService.ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>())
            .Returns(dialogReference);

        _presentationLogic.When(x => x.RemoveMultipleContents(Arg.Any<IEnumerable<ILearningContentViewModel>>()))
            .Throw(new FileNotFoundException("test"));

        var systemUnderTest = GetRenderedComponent();

        systemUnderTest.FindComponent<MudCheckBox<bool?>>().Find("input").Change(true);

        systemUnderTest.FindComponent<MudTable<ILearningContentViewModel>>()
            .FindComponent<MudIconButton>().Find("button").Click();

        _errorService.Received(1).SetError("Error deleting content", "test");
    }

    [Test]
    // ANF-ID: [AWA0037, AWA0043]
    public void DeleteMultiple_SerializationException_CallsErrorService()
    {
        PresentationLogicSetItems();
        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Ok(true));
        _dialogService.ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>())
            .Returns(dialogReference);

        _presentationLogic.When(x => x.RemoveMultipleContents(Arg.Any<IEnumerable<ILearningContentViewModel>>()))
            .Throw(new SerializationException("test"));

        var systemUnderTest = GetRenderedComponent();

        systemUnderTest.FindComponent<MudCheckBox<bool?>>().Find("input").Change(true);

        systemUnderTest.FindComponent<MudTable<ILearningContentViewModel>>()
            .FindComponent<MudIconButton>().Find("button").Click();

        _errorService.Received(1).SetError("Error deleting content", "test");
    }

    [Test]
    // ANF-ID: [AWA0048, AWA0049]
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
    // ANF-ID: [ASN0028]
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
                Assert.That(path,
                    Is.EqualTo(item is FileContentViewModel fc ? fc.Filepath : ((LinkContentViewModel)item).Link));
            }
        });
    }

    [Test]
    // ANF-ID: [AWA0002]
    public void ClickNewElementButton_CallsPresentationLogicAndMediator()
    {
        var items = PresentationLogicSetItems();

        var systemUnderTest = GetRenderedComponent();

        var mudPopovers = systemUnderTest.FindComponentsOrFail<Stub<MudPopover>>();
        var hoverMenu = mudPopovers.First(x => x.Instance.Parameters["AnchorOrigin"].ToString() == "CenterRight");
        var hoverMenuContent = _testContext.Render((RenderFragment)hoverMenu.Instance.Parameters["ChildContent"]);
        var newElementButton = hoverMenuContent.FindAll("button").FirstOrDefault(x =>
            x.Attributes["title"] != null && x.Attributes["title"]!.Value.Contains("NewElement"));

        Assert.That(newElementButton, Is.Not.Null);
        newElementButton!.Click();

        _presentationLogic.Received().SetSelectedLearningContentViewModel(items.First());
        _mediator.Received().RequestOpenNewElementDialog();
    }

    [Test]
    // ANF-ID: [AWA0038, AWA0044]
    public void ClickPreviewButton_CallsPresentationLogic()
    {
        var items = PresentationLogicSetItems();

        var systemUnderTest = GetRenderedComponent();

        var mudPopovers = systemUnderTest.FindComponentsOrFail<Stub<MudPopover>>();
        var hoverMenu = mudPopovers.First(x => x.Instance.Parameters["AnchorOrigin"].ToString() == "CenterRight");
        var hoverMenuContent = _testContext.Render((RenderFragment)hoverMenu.Instance.Parameters["ChildContent"]);
        var previewButton = hoverMenuContent.FindAll("button").FirstOrDefault(x =>
            x.Attributes["title"] != null && x.Attributes["title"]!.Value.Contains("Preview"));

        Assert.That(previewButton, Is.Not.Null);
        previewButton!.Click();

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
        _presentationLogic.GetAllContentFromDir().Returns(items);
        return items;
    }

    private IRenderedComponent<ContentFilesView> GetRenderedComponent()
    {
        return _testContext.RenderComponent<ContentFilesView>();
    }
}