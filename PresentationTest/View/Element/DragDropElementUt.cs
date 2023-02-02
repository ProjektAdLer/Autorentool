using System;
using System.Text.RegularExpressions;
using Bunit;
using Microsoft.AspNetCore.Components;
using MudBlazor.Services;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.Element;
using Presentation.View.Element;
using Shared;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.Element;

[TestFixture]
public class DragDropElementUt
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
        var element = Substitute.For<IElementViewModel>();
        var onClicked = new Action<IElementViewModel>(_ => { });
        var onDoubleClicked = new Action<IElementViewModel>(_ => { });
        var onEditElement = new Action<IElementViewModel>(_ => { });
        var onDeleteElement = new Action<IElementViewModel>(_ => { });
        var onShowElementContent = new Action<IElementViewModel>(_ => { });
        var systemUnderTest =
            GetRenderedDragDropElement(element, onClicked, onDoubleClicked, onEditElement,
                onDeleteElement, onShowElementContent);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.Element, Is.EqualTo(element));
            //overriding nullability warning because we know target isn't null as onClicked isn't a static method but instead a lambda -n.stich
            Assert.That(systemUnderTest.Instance.OnClicked,
                Is.EqualTo(EventCallback.Factory.Create(onClicked.Target!, onClicked)));
            Assert.That(systemUnderTest.Instance.OnDoubleClicked,
                Is.EqualTo(EventCallback.Factory.Create(onDoubleClicked.Target!, onDoubleClicked)));
            Assert.That(systemUnderTest.Instance.OnEditElement,
                Is.EqualTo(EventCallback.Factory.Create(onEditElement.Target!, onEditElement)));
            Assert.That(systemUnderTest.Instance.OnDeleteElement,
                Is.EqualTo(EventCallback.Factory.Create(onDeleteElement.Target!, onDeleteElement)));
            Assert.That(systemUnderTest.Instance.OnShowElementContent,
                Is.EqualTo(EventCallback.Factory.Create(onShowElementContent.Target!,
                    onShowElementContent)));
        });
    }

    [Test]
    public void Constructor_PassesCorrectValuesToDragDropElement()
    {
        var element = Substitute.For<IElementViewModel>();
        element.Difficulty.Returns(ElementDifficultyEnum.Medium);
        element.Name.Returns("foo bar super cool name");
        var onClicked = new Action<IElementViewModel>(_ => { });
        var onDoubleClicked = new Action<IElementViewModel>(_ => { });
        var onEditElement = new Action<IElementViewModel>(_ => { });
        var onDeleteElement = new Action<IElementViewModel>(_ => { });
        var onShowElementContent = new Action<IElementViewModel>(_ => { });
        var systemUnderTest =
            GetRenderedDragDropElement(element, onClicked, onDoubleClicked, onEditElement,
                onDeleteElement, onShowElementContent);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.Element, Is.EqualTo(element));
            Assert.That(systemUnderTest.Instance.OnClicked,
                Is.EqualTo(EventCallback.Factory.Create(onClicked.Target!, onClicked)));
            Assert.That(systemUnderTest.Instance.OnDoubleClicked,
                Is.EqualTo(EventCallback.Factory.Create(onDoubleClicked.Target!, onDoubleClicked)));
            Assert.That(systemUnderTest.Instance.OnEditElement,
                Is.EqualTo(EventCallback.Factory.Create(onEditElement.Target!, onEditElement)));
            Assert.That(systemUnderTest.Instance.OnDeleteElement,
                Is.EqualTo(EventCallback.Factory.Create(onDeleteElement.Target!, onDeleteElement)));
            Assert.That(systemUnderTest.Instance.OnShowElementContent,
                Is.EqualTo(EventCallback.Factory.Create(onShowElementContent.Target!,
                    onShowElementContent)));
        });

        var id = Regex.Match(systemUnderTest.Markup, @"id=""(?<id>[a-z0-9-]*)""").Groups["id"].Value;
        systemUnderTest.Markup.MarkupMatches(
            Regex.Replace(
                @"<div class=""mud-menu"">" +
                @"<div class=""mud-menu-activator"">" +
                @"<div class=""mud-paper mud-elevation-1 mud-card flex"" style="""">" +
                @"<div tabindex=""0"" class=""mud-list-item mud-list-item-gutters"">" +
                @"<div class=""mud-list-item-text"">" +
                @$"<span class=""mud-typography mud-typography-inherit"">{systemUnderTest.Instance.Element.Name}</span>" +
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
            () => GetRenderedDragDropElement(null!, _ => { }, _ => { }, _ => { }, _ => { },
                _ => { }), Throws.ArgumentNullException);
    }

    [Test]
    public void GetDifficultyPolygon_InputOutOfRange_ThrowsException()
    {
        Assert.That(() => DragDropElement.GetDifficultyIcon((ElementDifficultyEnum) 123),
            Throws.TypeOf<ArgumentOutOfRangeException>());
    }

    [Test]
    [TestCase(ElementDifficultyEnum.Easy, "13 1 10 10 2 13 10 16 13 25 16 16 24 13 16 10", "green")]
    [TestCase(ElementDifficultyEnum.Medium, "13 1 5 25 24 10 2 10 21 25", "yellow")]
    [TestCase(ElementDifficultyEnum.Hard,
        "13 1 10 8 2 7 8 13 2 19 10 18 13 25 16 18 24 19 19 13 24 7 16 8 13 1", "red")]
    [TestCase(ElementDifficultyEnum.None, "0", "lightblue")]
    public void GetDifficultyPolygon_ValidInput_ReturnsCorrectPolygon(ElementDifficultyEnum difficulty,
        string expectedPoints, string expectedColor)
    {
        var svg = DragDropElement.GetDifficultyIcon(difficulty);
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

    private IRenderedComponent<DragDropElement> GetRenderedDragDropElement(
        IElementViewModel objectViewmodel, Action<IElementViewModel> onClicked,
        Action<IElementViewModel> onDoubleClicked, Action<IElementViewModel> onEditElement,
        Action<IElementViewModel> onDeleteElement,
        Action<IElementViewModel> onShowElementContent)
    {
        return _ctx.RenderComponent<DragDropElement>(parameters => parameters
            .Add(p => p.Element, objectViewmodel)
            .Add(p => p.OnClicked, onClicked)
            .Add(p => p.OnDoubleClicked, onDoubleClicked)
            .Add(p => p.OnEditElement, onEditElement)
            .Add(p => p.OnDeleteElement, onDeleteElement)
            .Add(p => p.OnShowElementContent, onShowElementContent)
        );
    }
}