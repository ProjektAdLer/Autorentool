using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bunit;
using Bunit.TestDoubles;
using BusinessLogic.Commands;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms.Element;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.SelectedViewModels;
using Presentation.View.LearningElement;
using Shared;
using TestHelpers;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components.Forms.Element;

[TestFixture]
public class UnplacedElementsDropZoneUt
{
    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();

        _worldPresenter = Substitute.For<ILearningWorldPresenter>();
        _localizer = Substitute.For<IStringLocalizer<UnplacedElementsDropZone>>();
        _localizer[Arg.Any<string>()].Returns(ci => new LocalizedString(ci.Arg<string>(), ci.Arg<string>()));
        _localizer[Arg.Any<string>(), Arg.Any<object[]>()].Returns(ci =>
            new LocalizedString(ci.Arg<string>() + string.Concat(ci.Arg<object[]>()),
                ci.Arg<string>() + string.Concat(ci.Arg<object[]>())));
        _selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        _undoRedoSource = Substitute.For<IOnUndoRedo>();

        var localizerForLearningElementDifficultyHelper =
            Substitute.For<IStringLocalizer<LearningElementDifficultyEnum>>();
        localizerForLearningElementDifficultyHelper[Arg.Any<string>()]
            .Returns(ci => new LocalizedString(ci.Arg<string>(), ci.Arg<string>()));
        localizerForLearningElementDifficultyHelper[Arg.Any<string>(), Arg.Any<object[]>()].Returns(ci =>
            new LocalizedString(ci.Arg<string>() + string.Concat(ci.Arg<object[]>()),
                ci.Arg<string>() + string.Concat(ci.Arg<object[]>())));
        LearningElementDifficultyHelper.Initialize(localizerForLearningElementDifficultyHelper);

        _testContext.Services.AddSingleton(_worldPresenter);
        _testContext.Services.AddSingleton(_localizer);
        _testContext.Services.AddSingleton(_selectedViewModelsProvider);
        _testContext.Services.AddSingleton(_undoRedoSource);

        _testContext.AddMudBlazorTestServices();

        _testContext.ComponentFactories.AddStub<MudMenu>();
        _testContext.ComponentFactories.AddStub<MudMenuItem>();
        _testContext.ComponentFactories.AddStub<DragDropLearningElement>();
    }

    [TearDown]
    public void TearDown()
    {
        _testContext.Dispose();
    }

    private TestContext _testContext;
    private ILearningWorldPresenter _worldPresenter;
    private IStringLocalizer<UnplacedElementsDropZone> _localizer;
    private ISelectedViewModelsProvider _selectedViewModelsProvider;
    private IOnUndoRedo _undoRedoSource;

    private List<ILearningElementViewModel> _itemList;

    [Test]
    public void Constructor_InjectsDependencies()
    {
        var systemUnderTest = GetRenderedComponent();

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.WorldPresenter, Is.EqualTo(_worldPresenter));
            Assert.That(systemUnderTest.Instance.Localizer, Is.EqualTo(_localizer));
            Assert.That(systemUnderTest.Instance.SelectedViewModelsProvider, Is.EqualTo(_selectedViewModelsProvider));
            Assert.That(systemUnderTest.Instance.UndoRedoSource, Is.EqualTo(_undoRedoSource));
        });
    }

    [Test]
    public void Render_RendersUnplacedItems()
    {
        var expectedItem1 = Substitute.For<ILearningElementViewModel>();
        expectedItem1.Name = "item1";
        expectedItem1.LearningContent = Substitute.For<IAdaptivityContentViewModel>();
        var expectedItem2 = Substitute.For<ILearningElementViewModel>();
        expectedItem2.Name = "item2";
        expectedItem2.LearningContent = Substitute.For<IAdaptivityContentViewModel>();
        var items = new List<ILearningElementViewModel>() { expectedItem1, expectedItem2 };
        var systemUnderTest = GetRenderedComponent(items);

        var mudDropZone = systemUnderTest.FindComponentOrFail<MudDropZone<ILearningElementViewModel>>();

        var dragDropLearningElements = mudDropZone.FindComponentsOrFail<Stub<DragDropLearningElement>>().ToList();

        Assert.That(dragDropLearningElements, Has.Count.EqualTo(2));
        var element1 = dragDropLearningElements.First();
        var element2 = dragDropLearningElements.Last();
        Assert.That(element1.Instance.Parameters["LearningElement"], Is.EqualTo(expectedItem1));
        Assert.That(element2.Instance.Parameters["LearningElement"], Is.EqualTo(expectedItem2));
    }

    [Test]
    public void Render_RendersFilters()
    {
        var systemUnderTest = GetRenderedComponent();

        var mudMenus = systemUnderTest.FindComponentsOrFail<Stub<MudMenu>>().ToList();
        var searchBar = systemUnderTest.FindComponentOrFail<MudTextField<string>>();

        Assert.That(mudMenus, Has.Count.EqualTo(3));

        // Element Type Filter
        var elementTypeFilterHeader =
            _testContext.Render((RenderFragment)mudMenus[0].Instance.Parameters["ActivatorContent"]);
        var elementTypeFilterChildContent =
            _testContext.Render((RenderFragment)mudMenus[0].Instance.Parameters["ChildContent"]);
        var elementTypeFilterEntries = elementTypeFilterChildContent.FindComponentsOrFail<Stub<MudMenuItem>>().ToList();
        Assert.Multiple(() =>
        {
            Assert.That(elementTypeFilterHeader.Markup, Contains.Substring("UnplacedElementsDropZone.Filter.Element"));
            Assert.That(elementTypeFilterEntries, Has.Count.EqualTo(4));
            Assert.That(
                _testContext.Render((RenderFragment)elementTypeFilterEntries[0].Instance.Parameters["ChildContent"])
                    .Markup, Is.EqualTo("UnplacedElementsDropZone.Filter.All"));
            Assert.That(
                _testContext.Render((RenderFragment)elementTypeFilterEntries[1].Instance.Parameters["ChildContent"])
                    .Markup, Is.EqualTo("UnplacedElementsDropZone.Filter.LearningElement"));
            Assert.That(
                _testContext.Render((RenderFragment)elementTypeFilterEntries[2].Instance.Parameters["ChildContent"])
                    .Markup, Is.EqualTo("UnplacedElementsDropZone.Filter.AdaptivityElement"));
            Assert.That(
                _testContext.Render((RenderFragment)elementTypeFilterEntries[3].Instance.Parameters["ChildContent"])
                    .Markup, Is.EqualTo("UnplacedElementsDropZone.Filter.StoryElement"));
        });

        // Content Type Filter
        var contentTypeFilterHeader =
            _testContext.Render((RenderFragment)mudMenus[1].Instance.Parameters["ActivatorContent"]);
        var contentTypeFilterChildContent =
            _testContext.Render((RenderFragment)mudMenus[1].Instance.Parameters["ChildContent"]);
        var contentTypeFilterEntries = contentTypeFilterChildContent.FindComponentsOrFail<Stub<MudMenuItem>>().ToList();
        Assert.Multiple(() =>
        {
            Assert.That(contentTypeFilterHeader.Markup, Contains.Substring("UnplacedElementsDropZone.Filter.Type"));
            Assert.That(contentTypeFilterEntries, Has.Count.EqualTo(Enum.GetNames(typeof(ContentTypeEnum)).Length + 1));
            Assert.That(
                _testContext.Render((RenderFragment)contentTypeFilterEntries[0].Instance.Parameters["ChildContent"])
                    .Markup, Is.EqualTo("UnplacedElementsDropZone.Filter.All"));
        });
        foreach (var contentTypeFilterEntry in contentTypeFilterEntries.Skip(1))
        {
            var entryMarkup = _testContext
                .Render((RenderFragment)contentTypeFilterEntry.Instance.Parameters["ChildContent"]).Markup;
            Assert.That(entryMarkup, Does.StartWith("UnplacedElementsDropZone.Type."));
            entryMarkup = entryMarkup.Remove(0, "UnplacedElementsDropZone.Type.".Length);
            Assert.That(Enum.GetNames(typeof(ContentTypeEnum)), Contains.Item(entryMarkup));
        }

        // Difficulty Filter
        var difficultyFilterHeader =
            _testContext.Render((RenderFragment)mudMenus[2].Instance.Parameters["ActivatorContent"]);
        var difficultyFilterChildContent =
            _testContext.Render((RenderFragment)mudMenus[2].Instance.Parameters["ChildContent"]);
        var difficultyFilterEntries = difficultyFilterChildContent.FindComponentsOrFail<Stub<MudMenuItem>>().ToList();
        Assert.Multiple(() =>
        {
            Assert.That(difficultyFilterHeader.Markup,
                Contains.Substring("UnplacedElementsDropZone.Filter.Difficulty"));
            Assert.That(difficultyFilterEntries, Has.Count.EqualTo(5));
            Assert.That(
                _testContext.Render((RenderFragment)difficultyFilterEntries[0].Instance.Parameters["ChildContent"])
                    .Markup, Is.EqualTo("UnplacedElementsDropZone.Filter.All"));
            Assert.That(
                _testContext.Render((RenderFragment)difficultyFilterEntries[1].Instance.Parameters["ChildContent"])
                    .Markup, Is.EqualTo("Enum.LearningElementDifficultyEnum.None"));
            Assert.That(
                _testContext.Render((RenderFragment)difficultyFilterEntries[2].Instance.Parameters["ChildContent"])
                    .Markup, Is.EqualTo("Enum.LearningElementDifficultyEnum.Easy"));
            Assert.That(
                _testContext.Render((RenderFragment)difficultyFilterEntries[3].Instance.Parameters["ChildContent"])
                    .Markup, Is.EqualTo("Enum.LearningElementDifficultyEnum.Medium"));
            Assert.That(
                _testContext.Render((RenderFragment)difficultyFilterEntries[4].Instance.Parameters["ChildContent"])
                    .Markup, Is.EqualTo("Enum.LearningElementDifficultyEnum.Hard"));
        });

        // Search Bar
        Assert.That(searchBar.Instance.Placeholder, Is.EqualTo("UnplacedElementsDropZone.SearchBar.PlaceHolder"));
    }

    [Test]
    [TestCase("")]
    [TestCase("1")]
    [TestCase("xyz")]
    public async Task Filter_SearchText_ShowFilteredItems(string searchString)
    {
        var items = GetTestItems();
        var systemUnderTest = GetRenderedComponent(items: items);

        var searchBar = systemUnderTest.FindComponentOrFail<MudTextField<string>>();

        await systemUnderTest.InvokeAsync(async () => { await searchBar.Instance.SetText(searchString); });

        var mudDropZone = systemUnderTest.FindComponentOrFail<MudDropZone<ILearningElementViewModel>>();

        var dragDropLearningElements = mudDropZone.FindComponents<Stub<DragDropLearningElement>>().ToList();


        var expectedItems = items.Where(x => x.Name.Contains(searchString)).ToList();
        Assert.That(dragDropLearningElements, Has.Count.EqualTo(expectedItems.Count));
        foreach (var element in expectedItems)
        {
            Assert.That(dragDropLearningElements.Select(x => x.Instance.Parameters["LearningElement"]),
                Contains.Item(element));
        }
    }

    [Test]
    [TestCase(new[] { "All" }, 11)]
    [TestCase(new[] { "StoryElement" }, 2)]
    [TestCase(new[] { "AdaptivityElement" }, 2)]
    [TestCase(new[] { "LearningElement" }, 7)]
    [TestCase(new[] { "StoryElement", "AdaptivityElement" }, 4)]
    [TestCase(new[] { "StoryElement", "LearningElement" }, 9)]
    [TestCase(new[] { "AdaptivityElement", "LearningElement" }, 9)]
    [TestCase(new[] { "StoryElement", "AdaptivityElement", "LearningElement" }, 11)]
    public async Task Filter_ElementType_ShowFilteredItems(string[] elementTypes, int expectedCount)
    {
        // Arrange
        var items = GetTestItems();
        var systemUnderTest = GetRenderedComponent(items: items);

        var filterDropDowns = systemUnderTest.FindComponentsOrFail<Stub<MudMenu>>().ToList();

        var elementTypeFilterChildContent =
            _testContext.Render((RenderFragment)filterDropDowns[0].Instance
                .Parameters["ChildContent"]);

        // Act
        foreach (var elementType in elementTypes)
        {
            var currentOnClick = (EventCallback<MouseEventArgs>)(elementTypeFilterChildContent
                .FindComponents<Stub<MudMenuItem>>().First(item =>
                    _testContext.Render((RenderFragment)item.Instance.Parameters["ChildContent"]).Markup
                        .Contains(elementType))).Instance.Parameters["OnClick"];
            await systemUnderTest.InvokeAsync(() => currentOnClick.InvokeAsync());
        }

        var mudDropZone = systemUnderTest.FindComponentOrFail<MudDropZone<ILearningElementViewModel>>();
        var dragDropLearningElements = mudDropZone.FindComponents<Stub<DragDropLearningElement>>().ToList();

        // Assert
        Assert.That(dragDropLearningElements, Has.Count.EqualTo(expectedCount));
    }

    [Test]
    public async Task Filter_ElementType_AddAndRemoveFilters()
    {
        var items = GetTestItems();
        var systemUnderTest = GetRenderedComponent(items: items);

        var filterDropDowns = systemUnderTest.FindComponentsOrFail<Stub<MudMenu>>().ToList();

        var elementTypeFilterChildContent =
            _testContext.Render((RenderFragment)filterDropDowns[0].Instance
                .Parameters["ChildContent"]);

        var mudDropZone = systemUnderTest.FindComponentOrFail<MudDropZone<ILearningElementViewModel>>();
        var dragDropLearningElements = mudDropZone.FindComponents<Stub<DragDropLearningElement>>().ToList();
        Assert.That(dragDropLearningElements, Has.Count.EqualTo(11));

        var storyElementOnClick = (EventCallback<MouseEventArgs>)(elementTypeFilterChildContent
            .FindComponents<Stub<MudMenuItem>>().First(item =>
                _testContext.Render((RenderFragment)item.Instance.Parameters["ChildContent"]).Markup
                    .Contains("StoryElement"))).Instance.Parameters["OnClick"];
        var learningElementOnClick = (EventCallback<MouseEventArgs>)(elementTypeFilterChildContent
            .FindComponents<Stub<MudMenuItem>>().First(item =>
                _testContext.Render((RenderFragment)item.Instance.Parameters["ChildContent"]).Markup
                    .Contains("LearningElement"))).Instance.Parameters["OnClick"];

        // Add StoryElement filter
        await systemUnderTest.InvokeAsync(() => storyElementOnClick.InvokeAsync());

        dragDropLearningElements = mudDropZone.FindComponents<Stub<DragDropLearningElement>>().ToList();
        Assert.That(dragDropLearningElements, Has.Count.EqualTo(2));

        // Add LearningElement filter
        await systemUnderTest.InvokeAsync(() => learningElementOnClick.InvokeAsync());

        dragDropLearningElements = mudDropZone.FindComponents<Stub<DragDropLearningElement>>().ToList();
        Assert.That(dragDropLearningElements, Has.Count.EqualTo(9));

        // Remove StoryElement filter
        await systemUnderTest.InvokeAsync(() => storyElementOnClick.InvokeAsync());

        dragDropLearningElements = mudDropZone.FindComponents<Stub<DragDropLearningElement>>().ToList();
        Assert.That(dragDropLearningElements, Has.Count.EqualTo(7));

        // Remove LearningElement filter
        await systemUnderTest.InvokeAsync(() => learningElementOnClick.InvokeAsync());

        dragDropLearningElements = mudDropZone.FindComponents<Stub<DragDropLearningElement>>().ToList();
        Assert.That(dragDropLearningElements, Has.Count.EqualTo(11));
    }

    [Test]
    [TestCase(new[] { "All" }, 11)]
    [TestCase(new[] { "Text" }, 2)]
    [TestCase(new[] { "Image" }, 2)]
    [TestCase(new[] { "Video" }, 2)]
    [TestCase(new[] { "H5P" }, 1)]
    [TestCase(new[] { "Adaptivity" }, 2)]
    [TestCase(new[] { "Story" }, 2)]
    [TestCase(new[] { "Text", "Image" }, 4)]
    [TestCase(new[] { "Image", "Video", "H5P" }, 5)]
    [TestCase(new[] { "Adaptivity", "Story" }, 4)]
    [TestCase(new[] { "Text", "Image", "Video", "H5P" }, 7)]
    [TestCase(new[] { "Text", "Image", "Video", "H5P", "Adaptivity", "Story" }, 11)]
    public async Task Filter_ContentType_ShowFilteredItems(string[] contentTypes, int expectedCount)
    {
        // Arrange
        var items = GetTestItems();
        var systemUnderTest = GetRenderedComponent(items: items);

        var filterDropDowns = systemUnderTest.FindComponentsOrFail<Stub<MudMenu>>().ToList();

        var contentTypeFilterChildContent =
            _testContext.Render((RenderFragment)filterDropDowns[1].Instance
                .Parameters["ChildContent"]);

        // Act
        foreach (var contentType in contentTypes)
        {
            var currentOnClick = (EventCallback<MouseEventArgs>)(contentTypeFilterChildContent
                .FindComponents<Stub<MudMenuItem>>().First(item =>
                    _testContext.Render((RenderFragment)item.Instance.Parameters["ChildContent"]).Markup
                        .Contains(contentType))).Instance.Parameters["OnClick"];
            await systemUnderTest.InvokeAsync(() => currentOnClick.InvokeAsync());
        }

        var mudDropZone = systemUnderTest.FindComponentOrFail<MudDropZone<ILearningElementViewModel>>();
        var dragDropLearningElements = mudDropZone.FindComponents<Stub<DragDropLearningElement>>().ToList();

        // Assert
        Assert.That(dragDropLearningElements, Has.Count.EqualTo(expectedCount));
    }

    [Test]
    public async Task Filter_ContentType_AddAndRemoveFilters()
    {
        var items = GetTestItems();
        var systemUnderTest = GetRenderedComponent(items: items);

        var filterDropDowns = systemUnderTest.FindComponentsOrFail<Stub<MudMenu>>().ToList();

        var contentTypeFilterChildContent =
            _testContext.Render((RenderFragment)filterDropDowns[1].Instance
                .Parameters["ChildContent"]);

        var mudDropZone = systemUnderTest.FindComponentOrFail<MudDropZone<ILearningElementViewModel>>();
        var dragDropLearningElements = mudDropZone.FindComponents<Stub<DragDropLearningElement>>().ToList();
        Assert.That(dragDropLearningElements, Has.Count.EqualTo(11));

        var textOnClick = (EventCallback<MouseEventArgs>)(contentTypeFilterChildContent
            .FindComponents<Stub<MudMenuItem>>().First(item =>
                _testContext.Render((RenderFragment)item.Instance.Parameters["ChildContent"]).Markup
                    .Contains("Text"))).Instance.Parameters["OnClick"];
        var imageOnClick = (EventCallback<MouseEventArgs>)(contentTypeFilterChildContent
            .FindComponents<Stub<MudMenuItem>>().First(item =>
                _testContext.Render((RenderFragment)item.Instance.Parameters["ChildContent"]).Markup
                    .Contains("Image"))).Instance.Parameters["OnClick"];

        // Add Text filter
        await systemUnderTest.InvokeAsync(() => textOnClick.InvokeAsync());

        dragDropLearningElements = mudDropZone.FindComponents<Stub<DragDropLearningElement>>().ToList();
        Assert.That(dragDropLearningElements, Has.Count.EqualTo(2));

        // Add Image filter
        await systemUnderTest.InvokeAsync(() => imageOnClick.InvokeAsync());

        dragDropLearningElements = mudDropZone.FindComponents<Stub<DragDropLearningElement>>().ToList();
        Assert.That(dragDropLearningElements, Has.Count.EqualTo(4));

        // Remove Text filter
        await systemUnderTest.InvokeAsync(() => textOnClick.InvokeAsync());

        dragDropLearningElements = mudDropZone.FindComponents<Stub<DragDropLearningElement>>().ToList();
        Assert.That(dragDropLearningElements, Has.Count.EqualTo(2));

        // Remove Image filter
        await systemUnderTest.InvokeAsync(() => imageOnClick.InvokeAsync());

        dragDropLearningElements = mudDropZone.FindComponents<Stub<DragDropLearningElement>>().ToList();
        Assert.That(dragDropLearningElements, Has.Count.EqualTo(11));
    }

    [Test]
    [TestCase(new[] { "All" }, 11)]
    [TestCase(new[] { "None" }, 2)]
    [TestCase(new[] { "Easy" }, 4)]
    [TestCase(new[] { "Medium" }, 3)]
    [TestCase(new[] { "Hard" }, 2)]
    [TestCase(new[] { "Easy", "Medium", "Hard" }, 9)]
    public async Task Filter_Difficulty_ShowFilteredItems(string[] contentTypes,
        int expectedCount)
    {
        // Arrange
        var items = GetTestItems();
        var systemUnderTest = GetRenderedComponent(items: items);

        var filterDropDowns = systemUnderTest.FindComponentsOrFail<Stub<MudMenu>>().ToList();

        var difficultyFilterChildContent =
            _testContext.Render((RenderFragment)filterDropDowns[2].Instance
                .Parameters["ChildContent"]);

        // Act
        foreach (var difficulty in contentTypes)
        {
            var currentOnClick = (EventCallback<MouseEventArgs>)(difficultyFilterChildContent
                .FindComponents<Stub<MudMenuItem>>().First(item =>
                    _testContext.Render((RenderFragment)item.Instance.Parameters["ChildContent"]).Markup
                        .Contains(difficulty))).Instance.Parameters["OnClick"];
            await systemUnderTest.InvokeAsync(() => currentOnClick.InvokeAsync());
        }

        var mudDropZone = systemUnderTest.FindComponentOrFail<MudDropZone<ILearningElementViewModel>>();
        var dragDropLearningElements = mudDropZone.FindComponents<Stub<DragDropLearningElement>>().ToList();

        // Assert
        Assert.That(dragDropLearningElements, Has.Count.EqualTo(expectedCount));
    }

    [Test]
    public async Task Filter_Difficulty_AddAndRemoveFilters()
    {
        var items = GetTestItems();
        var systemUnderTest = GetRenderedComponent(items: items);

        var filterDropDowns = systemUnderTest.FindComponentsOrFail<Stub<MudMenu>>().ToList();

        var difficultyFilterChildContent =
            _testContext.Render((RenderFragment)filterDropDowns[2].Instance
                .Parameters["ChildContent"]);

        var mudDropZone = systemUnderTest.FindComponentOrFail<MudDropZone<ILearningElementViewModel>>();
        var dragDropLearningElements = mudDropZone.FindComponents<Stub<DragDropLearningElement>>().ToList();
        Assert.That(dragDropLearningElements, Has.Count.EqualTo(11));

        var easyOnClick = (EventCallback<MouseEventArgs>)(difficultyFilterChildContent
            .FindComponents<Stub<MudMenuItem>>().First(item =>
                _testContext.Render((RenderFragment)item.Instance.Parameters["ChildContent"]).Markup
                    .Contains("Easy"))).Instance.Parameters["OnClick"];
        var mediumOnClick = (EventCallback<MouseEventArgs>)(difficultyFilterChildContent
            .FindComponents<Stub<MudMenuItem>>().First(item =>
                _testContext.Render((RenderFragment)item.Instance.Parameters["ChildContent"]).Markup
                    .Contains("Medium"))).Instance.Parameters["OnClick"];

        // Add Easy filter
        await systemUnderTest.InvokeAsync(() => easyOnClick.InvokeAsync());

        dragDropLearningElements = mudDropZone.FindComponents<Stub<DragDropLearningElement>>().ToList();
        Assert.That(dragDropLearningElements, Has.Count.EqualTo(4));

        // Add Medium filter
        await systemUnderTest.InvokeAsync(() => mediumOnClick.InvokeAsync());

        dragDropLearningElements = mudDropZone.FindComponents<Stub<DragDropLearningElement>>().ToList();
        Assert.That(dragDropLearningElements, Has.Count.EqualTo(7));

        // Remove Easy filter
        await systemUnderTest.InvokeAsync(() => easyOnClick.InvokeAsync());

        dragDropLearningElements = mudDropZone.FindComponents<Stub<DragDropLearningElement>>().ToList();
        Assert.That(dragDropLearningElements, Has.Count.EqualTo(3));

        // Remove Medium filter
        await systemUnderTest.InvokeAsync(() => mediumOnClick.InvokeAsync());

        dragDropLearningElements = mudDropZone.FindComponents<Stub<DragDropLearningElement>>().ToList();
        Assert.That(dragDropLearningElements, Has.Count.EqualTo(11));
    }


    private static ILearningElementViewModel CreateSubstituteForLearningElement(string name,
        ILearningContentViewModel learningContent,
        LearningElementDifficultyEnum difficulty = LearningElementDifficultyEnum.None)
    {
        var item = Substitute.For<ILearningElementViewModel>();
        item.Name.Returns(name);
        item.LearningContent = learningContent;
        item.Difficulty = difficulty;

        return item;
    }

    private static List<ILearningElementViewModel> GetTestItems()
    {
        return new List<ILearningElementViewModel>
        {
            CreateSubstituteForLearningElement("storyItem1", ViewModelProvider.GetStoryContent(),
                LearningElementDifficultyEnum.Easy),
            CreateSubstituteForLearningElement("storyItem2", ViewModelProvider.GetStoryContent(),
                LearningElementDifficultyEnum.Easy),
            CreateSubstituteForLearningElement("adaptivityItem1", ViewModelProvider.GetAdaptivityContent(),
                LearningElementDifficultyEnum.Medium),
            CreateSubstituteForLearningElement("adaptivityItem2", ViewModelProvider.GetAdaptivityContent()),
            CreateSubstituteForLearningElement("linkItem1", ViewModelProvider.GetLinkContent(),
                LearningElementDifficultyEnum.Easy),
            CreateSubstituteForLearningElement("linkItem2", ViewModelProvider.GetLinkContent(),
                LearningElementDifficultyEnum.Medium),
            CreateSubstituteForLearningElement("item4", ViewModelProvider.GetFileContent(type: "txt"),
                LearningElementDifficultyEnum.Hard),
            CreateSubstituteForLearningElement("item5", ViewModelProvider.GetFileContent(type: "pdf")),
            CreateSubstituteForLearningElement("item6", ViewModelProvider.GetFileContent(type: "jpg"),
                LearningElementDifficultyEnum.Easy),
            CreateSubstituteForLearningElement("item7", ViewModelProvider.GetFileContent(type: "bmp"),
                LearningElementDifficultyEnum.Medium),
            CreateSubstituteForLearningElement("item8", ViewModelProvider.GetFileContent(type: "h5p"),
                LearningElementDifficultyEnum.Hard)
        };
    }

    private IRenderedComponent<UnplacedElementsDropZone> GetRenderedComponent(
        List<ILearningElementViewModel>? items = null)
    {
        _itemList = items ?? GetTestItems();
        _testContext.RenderTree.Add<MudDropContainer<ILearningElementViewModel>>(parameterBuilder: builder =>
        {
            builder.Add(p => p.Items, _itemList);
            builder.Add(p => p.ItemsSelector, (model, s) => true);
        });
        return _testContext.RenderComponent<UnplacedElementsDropZone>();
    }
}