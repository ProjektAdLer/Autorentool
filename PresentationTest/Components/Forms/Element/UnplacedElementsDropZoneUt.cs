using System;
using System.Collections.Generic;
using System.Linq;
using Bunit;
using Bunit.TestDoubles;
using BusinessLogic.Commands;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms.Element;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent;
using Presentation.PresentationLogic.LearningContent.FileContent;
using Presentation.PresentationLogic.LearningContent.LinkContent;
using Presentation.PresentationLogic.LearningContent.Story;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.SelectedViewModels;
using Presentation.View.LearningElement;
using Shared;
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


    private static ILearningElementViewModel CreateSubstituteForLearningElement(string name,
        ILearningContentViewModel learningContent, string? fileType = null)
    {
        var item = Substitute.For<ILearningElementViewModel>();
        item.Name.Returns(name);
        item.LearningContent = learningContent;
        item.Difficulty = LearningElementDifficultyEnum.None;

        if (fileType != null && learningContent is IFileContentViewModel fileContent)
        {
            fileContent.Type.Returns(fileType);
        }

        return item;
    }

    private static List<ILearningElementViewModel> GetTestItems()
    {
        return new List<ILearningElementViewModel>
        {
            CreateSubstituteForLearningElement("storyItem1", Substitute.For<IStoryContentViewModel>()),
            CreateSubstituteForLearningElement("storyItem2", Substitute.For<IStoryContentViewModel>()),
            CreateSubstituteForLearningElement("adaptivityItem1", Substitute.For<IAdaptivityContentViewModel>()),
            CreateSubstituteForLearningElement("adaptivityItem2", Substitute.For<IAdaptivityContentViewModel>()),
            CreateSubstituteForLearningElement("linkItem1", Substitute.For<ILinkContentViewModel>()),
            CreateSubstituteForLearningElement("linkItem2", Substitute.For<ILinkContentViewModel>()),
            CreateSubstituteForLearningElement("item4", Substitute.For<IFileContentViewModel>(), "txt"),
            CreateSubstituteForLearningElement("item5", Substitute.For<IFileContentViewModel>(), "pdf")
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