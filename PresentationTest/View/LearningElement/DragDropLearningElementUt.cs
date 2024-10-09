using System;
using System.Linq;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using MudBlazor;
using MudBlazor.Services;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningContent.LinkContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.SelectedViewModels;
using Presentation.View.LearningElement;
using Shared;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.LearningElement;

[TestFixture]
public class DragDropLearningElementUt
{
    [SetUp]
    public void Setup()
    {
        _stringLocalizer = Substitute.For<IStringLocalizer<DragDropLearningElement>>();
        _stringLocalizer[Arg.Any<string>()]
            .Returns(cinfo => new LocalizedString(cinfo.Arg<string>(), cinfo.Arg<string>()));
        _ctx = new TestContext();
        _ctx.Services.AddMudServices();
        _ctx.Services.AddSingleton(_stringLocalizer);
        _ctx.ComponentFactories.AddStub<MudMenu>();
        _ctx.ComponentFactories.AddStub<MudCard>();
        _ctx.ComponentFactories.AddStub<MudCardContent>();
        _ctx.ComponentFactories.AddStub<MudIcon>();
        _ctx.ComponentFactories.AddStub<MudListItem>();
        _ctx.ComponentFactories.AddStub<MudMenuItem>();
        _ctx.JSInterop.SetupVoid("mudPopover.connect", _ => true);
        _selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        _ctx.Services.AddSingleton(_selectedViewModelsProvider);
    }

    [TearDown]
    public void TearDown()
    {
        _ctx.Dispose();
    }

    private TestContext _ctx;
    private IStringLocalizer<DragDropLearningElement> _stringLocalizer;
    private ISelectedViewModelsProvider _selectedViewModelsProvider;

    [Test]
    public void Constructor_SetsParametersCorrectly()
    {
        var learningElement = Substitute.For<ILearningElementViewModel>();
        var content = Substitute.For<ILinkContentViewModel>();
        learningElement.LearningContent = content;
        var onClicked = new Action<ILearningElementViewModel>(_ => { });
        var onDoubleClicked = new Action<ILearningElementViewModel>(_ => { });
        var onEditLearningElement = new Action<ILearningElementViewModel>(_ => { });
        var onDeleteLearningElement = new Action<ILearningElementViewModel>(_ => { });
        var onShowLearningElementContent = new Action<ILearningElementViewModel>(_ => { });
        var systemUnderTest =
            GetRenderedDragDropLearningElement(learningElement, onClicked, onDoubleClicked, onEditLearningElement,
                onDeleteLearningElement, onShowLearningElementContent);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.LearningElement, Is.EqualTo(learningElement));
            //overriding nullability warning because we know target isn't null as onClicked isn't a static method but instead a lambda -n.stich
            Assert.That(systemUnderTest.Instance.OnClicked,
                Is.EqualTo(EventCallback.Factory.Create(onClicked.Target!, onClicked)));
            Assert.That(systemUnderTest.Instance.OnDoubleClicked,
                Is.EqualTo(EventCallback.Factory.Create(onDoubleClicked.Target!, onDoubleClicked)));
            Assert.That(systemUnderTest.Instance.OnEditLearningElement,
                Is.EqualTo(EventCallback.Factory.Create(onEditLearningElement.Target!, onEditLearningElement)));
            Assert.That(systemUnderTest.Instance.OnDeleteLearningElement,
                Is.EqualTo(EventCallback.Factory.Create(onDeleteLearningElement.Target!, onDeleteLearningElement)));
            Assert.That(systemUnderTest.Instance.OnShowLearningElementContent,
                Is.EqualTo(EventCallback.Factory.Create(onShowLearningElementContent.Target!,
                    onShowLearningElementContent)));
        });
    }

    [Test]
    public void Constructor_RendersCorrectlyAndPassesCorrectParametersToChildComponents()
    {
        var learningElement = Substitute.For<ILearningElementViewModel>();
        learningElement.Difficulty.Returns(LearningElementDifficultyEnum.Medium);
        learningElement.Name.Returns("foo bar super cool name");
        var content = Substitute.For<ILinkContentViewModel>();
        learningElement.LearningContent = content;

        var onClicked = new Action<ILearningElementViewModel>(_ => { });
        var onDoubleClicked = new Action<ILearningElementViewModel>(_ => { });
        var onEditLearningElement = new Action<ILearningElementViewModel>(_ => { });
        var onDeleteLearningElement = new Action<ILearningElementViewModel>(_ => { });
        var onShowLearningElementContent = new Action<ILearningElementViewModel>(_ => { });
        var systemUnderTest =
            GetRenderedDragDropLearningElement(learningElement, onClicked, onDoubleClicked, onEditLearningElement,
                onDeleteLearningElement, onShowLearningElementContent);

        var card = systemUnderTest.FindComponentOrFail<Stub<MudCard>>();
        var mudCardContent = _ctx.Render((RenderFragment)card.Instance.Parameters["ChildContent"]);
        var cardContent = _ctx.Render((RenderFragment)mudCardContent.FindComponentOrFail<Stub<MudCardContent>>()
            .Instance.Parameters["ChildContent"]);
        var icons = cardContent.FindComponentsOrFail<Stub<MudIcon>>().ToList();

        var elementIcon = icons.First(icon => ((string)icon.Instance.Parameters["Class"]).Contains("element-icon"));
        var difficultyIcon =
            icons.First(icon => ((string)icon.Instance.Parameters["Class"]).Contains("difficulty-icon"));
        Assert.Multiple(() =>
        {
            Assert.That(elementIcon.Instance.Parameters["Icon"], Is.EqualTo(CustomIcons.VideoElementIcon));
            Assert.That(difficultyIcon.Instance.Parameters["Icon"], Is.EqualTo(CustomIcons.DifficultyPolygonMedium));
        });
    }

    [Test]
    public void RenderMudCardContent_DifficultyOutOfRange_ThrowsException()
    {
        var element = Substitute.For<ILearningElementViewModel>();
        var content = Substitute.For<ILinkContentViewModel>();
        element.LearningContent.Returns(content);
        element.Difficulty.Returns((LearningElementDifficultyEnum)123);
        var systemUnderTest = GetRenderedDragDropLearningElement(element);
        var mudCardContent =
            _ctx.Render((RenderFragment)systemUnderTest.FindComponent<Stub<MudCard>>().Instance
                .Parameters["ChildContent"]);
        Assert.That(() => _ctx.Render((RenderFragment)mudCardContent.FindComponent<Stub<MudCardContent>>().Instance
                .Parameters["ChildContent"]),
            Throws.TypeOf<ArgumentOutOfRangeException>());
    }

    [Test]
    public void Constructor_ElementNull_ThrowsException()
    {
        //Override warning for this test as we are testing exactly what happens when we break the nullability contract - n.stich
        Assert.That(
            () => GetRenderedDragDropLearningElement(null!, _ => { }, _ => { }, _ => { }, _ => { },
                _ => { }), Throws.ArgumentNullException);
    }

    [Test]
    public void RenderMudCardContent_ContentNull_ThrowsArgumentOutOfRangeException()
    {
        var element = Substitute.For<ILearningElementViewModel>();
        element.LearningContent.Returns((ILearningContentViewModel)null!);
        var systemUnderTest = GetRenderedDragDropLearningElement(element);
        var mudCardContent =
            _ctx.Render((RenderFragment)systemUnderTest.FindComponent<Stub<MudCard>>().Instance
                .Parameters["ChildContent"]);
        Assert.That(() => _ctx.Render((RenderFragment)mudCardContent.FindComponent<Stub<MudCardContent>>().Instance
            .Parameters["ChildContent"]), Throws.TypeOf<ArgumentOutOfRangeException>());
    }

    [Test]
    [TestCase(LearningElementDifficultyEnum.Easy, CustomIcons.DifficultyPolygonEasy)]
    [TestCase(LearningElementDifficultyEnum.Medium, CustomIcons.DifficultyPolygonMedium)]
    [TestCase(LearningElementDifficultyEnum.Hard, CustomIcons.DifficultyPolygonHard)]
    [TestCase(LearningElementDifficultyEnum.None, CustomIcons.DifficultyPolygonNone)]
    public void GetDifficultyPolygon_ValidInput_ReturnsCorrectPolygon(LearningElementDifficultyEnum difficulty,
        string expectedIconString)
    {
        var element = Substitute.For<ILearningElementViewModel>();
        var content = Substitute.For<ILinkContentViewModel>();
        element.LearningContent.Returns(content);
        element.Difficulty.Returns(difficulty);
        var systemUnderTest = GetRenderedDragDropLearningElement(element);
        var card = systemUnderTest.FindComponentOrFail<Stub<MudCard>>();
        var mudCardContent = _ctx.Render((RenderFragment)_ctx
            .Render((RenderFragment)card.Instance.Parameters["ChildContent"])
            .FindComponentOrFail<Stub<MudCardContent>>().Instance.Parameters["ChildContent"]);
        var difficultyIcon = mudCardContent.FindComponentsOrFail<Stub<MudIcon>>()
            .First(icon => ((string)icon.Instance.Parameters["Class"]).Contains("difficulty-icon"));
        Assert.That(difficultyIcon.Instance.Parameters["Icon"], Is.EqualTo(expectedIconString));
    }

    private IRenderedComponent<DragDropLearningElement> GetRenderedDragDropLearningElement(
        ILearningElementViewModel objectViewmodel,
        Action<ILearningElementViewModel>? onClicked = null,
        Action<ILearningElementViewModel>? onDoubleClicked = null,
        Action<ILearningElementViewModel>? onEditLearningElement = null,
        Action<ILearningElementViewModel>? onDeleteLearningElement = null,
        Action<ILearningElementViewModel>? onShowLearningElementContent = null)
    {
        onClicked ??= _ => { };
        onDoubleClicked ??= _ => { };
        onEditLearningElement ??= _ => { };
        onDeleteLearningElement ??= _ => { };
        onShowLearningElementContent ??= _ => { };

        return _ctx.RenderComponent<DragDropLearningElement>(parameters => parameters
            .Add(p => p.LearningElement, objectViewmodel)
            .Add(p => p.OnClicked, onClicked)
            .Add(p => p.OnDoubleClicked, onDoubleClicked)
            .Add(p => p.OnEditLearningElement, onEditLearningElement)
            .Add(p => p.OnDeleteLearningElement, onDeleteLearningElement)
            .Add(p => p.OnShowLearningElementContent, onShowLearningElementContent)
        );
    }
}