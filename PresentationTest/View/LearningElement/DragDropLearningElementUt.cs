using System;
using System.Text.RegularExpressions;
using Bunit;
using Microsoft.AspNetCore.Components;
using MudBlazor.Services;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.LearningElement;
using Presentation.View.LearningElement;
using Shared;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.LearningElement;

[TestFixture]
public class DragDropLearningElementUt
{
#pragma warning disable CS8618 //set in setup - n.stich
    private TestContext _ctx;
#pragma warning restore CS8618

    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _ctx.Services.AddMudServices();
        _ctx.JSInterop.SetupVoid("mudPopover.connect", _ => true);
    }

    [Test]
    public void Constructor_SetsParametersCorrectly()
    {
        var learningElement = Substitute.For<ILearningElementViewModel>();
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
    public void Constructor_PassesCorrectValuesToDragDropElement()
    {
        var learningElement = Substitute.For<ILearningElementViewModel>();
        learningElement.Difficulty.Returns(LearningElementDifficultyEnum.Medium);
        learningElement.Name.Returns("foo bar super cool name");
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

        var id = Regex.Match(systemUnderTest.Markup, @"id=""(?<id>[a-z0-9-]*)""").Groups["id"].Value;
        systemUnderTest.Markup.MarkupMatches(
            Regex.Replace(
                @"<div class=""mud-menu"">" +
                @"<div class=""mud-menu-activator"">" +
                @"<div class=""mud-paper mud-elevation-1 mud-card flex"" style="""">" +
                @"<div tabindex=""0"" class=""mud-list-item mud-list-item-gutters"">" +
                @"<div class=""mud-list-item-text"">" +
                @$"<span class=""mud-typography mud-typography-inherit"">{systemUnderTest.Instance.LearningElement.Name}</span>" +
                @"</div>" +
                @"</div>" +
                @"<svg class=""mud-icon-root mud-svg-icon mud-icon-size-medium"" focusable=""false"" viewBox=""0 0 24 24"" aria-hidden=""true"">" +
                @"<svg>" +
                @"<polygon fill=""yellow"" points=""13 1 5 25 24 10 2 10 21 25""></polygon>" +
                @"</svg>" +
                @"</svg>" +
                @"</div>" +
                @"</div>" +
                @"<div id=""popover-RANDOM_ID"" class=""mud-popover-cascading-value""></div>" +
                @"</div>", "id=\"popover-RANDOM_ID\"", $"id=\"{id}\""));
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
    public void GetDifficultyPolygon_InputOutOfRange_ThrowsException()
    {
        Assert.That(() => DragDropLearningElement.GetDifficultyIcon((LearningElementDifficultyEnum) 123),
            Throws.TypeOf<ArgumentOutOfRangeException>());
    }

    [Test]
    [TestCase(LearningElementDifficultyEnum.Easy, "13 1 10 10 2 13 10 16 13 25 16 16 24 13 16 10", "green")]
    [TestCase(LearningElementDifficultyEnum.Medium, "13 1 5 25 24 10 2 10 21 25", "yellow")]
    [TestCase(LearningElementDifficultyEnum.Hard,
        "13 1 10 8 2 7 8 13 2 19 10 18 13 25 16 18 24 19 19 13 24 7 16 8 13 1", "red")]
    [TestCase(LearningElementDifficultyEnum.None, "0", "lightblue")]
    public void GetDifficultyPolygon_ValidInput_ReturnsCorrectPolygon(LearningElementDifficultyEnum difficulty,
        string expectedPoints, string expectedColor)
    {
        var svg = DragDropLearningElement.GetDifficultyIcon(difficulty);
        var pattern = new Regex(@"<polygon fill=""(?<color>.*)"" points=""(?<points>.*)""></polygon>");
        Match match = pattern.Match(svg);
        var actualPoints = match.Groups["points"].Value;
        var actualColor = match.Groups["color"].Value;
        Assert.Multiple(() =>
        {
            Assert.That(actualPoints, Is.EqualTo(expectedPoints));
            Assert.That(actualColor, Is.EqualTo(expectedColor));
        });
    }

    private IRenderedComponent<DragDropLearningElement> GetRenderedDragDropLearningElement(
        ILearningElementViewModel objectViewmodel, Action<ILearningElementViewModel> onClicked,
        Action<ILearningElementViewModel> onDoubleClicked, Action<ILearningElementViewModel> onEditLearningElement,
        Action<ILearningElementViewModel> onDeleteLearningElement,
        Action<ILearningElementViewModel> onShowLearningElementContent)
    {
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