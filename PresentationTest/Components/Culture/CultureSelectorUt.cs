using System.Globalization;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Culture;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components.Culture;

[TestFixture]
public class CultureSelectorUt
{
    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
        _navigation = Substitute.For<INavigationManagerWrapper>();
        _localizer = Substitute.For<IStringLocalizer<CultureSelector>>();
        _testContext.Services.AddSingleton(_navigation);
        _testContext.Services.AddSingleton(_localizer);
        _navigation.Uri.Returns("http://localhost:8001/Foo");
    }

    private TestContext _testContext;
    private INavigationManagerWrapper _navigation;
    private IStringLocalizer<CultureSelector> _localizer;

    [Test]
    public void Render_CurrentCultureGerman_GermanHighlighted()
    {
        CultureInfo.CurrentCulture = new CultureInfo("de-DE");

        var systemUnderTest = GetRenderedComponent();

        var buttonGerman = systemUnderTest.Find("button.german");
        var buttonEnglish = systemUnderTest.Find("button.english");
        var pGerman = systemUnderTest.Find("p.german");
        var pEnglish = systemUnderTest.Find("p.english");
        var imageGerman = systemUnderTest.Find("img.german");
        var imageEnglish = systemUnderTest.Find("img.english");

        Assert.Multiple(() =>
        {
            Assert.That(buttonGerman.ClassList, Does.Not.Contain("bg-adlerbggradientto"));
            Assert.That(pGerman.ClassList, Does.Not.Contain("text-adlerbggradientto"));
            Assert.That(imageGerman.ClassList, Does.Not.Contain("opacity-20"));
            Assert.That(buttonEnglish.ClassList, Does.Contain("bg-adlerbggradientto"));
            Assert.That(pEnglish.ClassList, Does.Contain("text-adlerbggradientto"));
            Assert.That(imageEnglish.ClassList, Does.Contain("opacity-20"));
        });
    }

    [Test]
    public void Render_CurrentCultureEnglish_EnglishHighlighted()
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-DE");

        var systemUnderTest = GetRenderedComponent();

        var buttonGerman = systemUnderTest.Find("button.german");
        var buttonEnglish = systemUnderTest.Find("button.english");
        var pGerman = systemUnderTest.Find("p.german");
        var pEnglish = systemUnderTest.Find("p.english");
        var imageGerman = systemUnderTest.Find("img.german");
        var imageEnglish = systemUnderTest.Find("img.english");

        Assert.Multiple(() =>
        {
            Assert.That(buttonGerman.ClassList, Does.Contain("bg-adlerbggradientto"));
            Assert.That(pGerman.ClassList, Does.Contain("text-adlerbggradientto"));
            Assert.That(imageGerman.ClassList, Does.Contain("opacity-20"));
            Assert.That(buttonEnglish.ClassList, Does.Not.Contain("bg-adlerbggradientto"));
            Assert.That(pEnglish.ClassList, Does.Not.Contain("text-adlerbggradientto"));
            Assert.That(imageEnglish.ClassList, Does.Not.Contain("opacity-20"));
        });
    }

    [Test]
    public void ClickGerman_CurrentCultureEnglish_SwitchesToGerman()
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-DE");

        var systemUnderTest = GetRenderedComponent();

        var buttonGerman = systemUnderTest.Find("button.german");
        buttonGerman.Click();

        _navigation.Received().NavigateTo("Culture/Set?culture=de-DE&redirectUri=%2FFoo", true);
    }

    [Test]
    public void ClickGerman_CurrentCultureGerman_NothingHappens()
    {
        CultureInfo.CurrentCulture = new CultureInfo("de-DE");

        var systemUnderTest = GetRenderedComponent();

        var buttonGerman = systemUnderTest.Find("button.german");
        buttonGerman.Click();

        _navigation.DidNotReceive().NavigateTo("Culture/Set?culture=de-DE&redirectUri=%2FFoo", true);
    }


    [Test]
    public void ClickEnglish_CurrentCultureGerman_SwitchesToEnglish()
    {
        CultureInfo.CurrentCulture = new CultureInfo("de-DE");

        var systemUnderTest = GetRenderedComponent();

        var buttonEnglish = systemUnderTest.Find("button.english");
        buttonEnglish.Click();

        _navigation.Received().NavigateTo("Culture/Set?culture=en-DE&redirectUri=%2FFoo", true);
    }

    private IRenderedComponent<CultureSelector> GetRenderedComponent()
    {
        return _testContext.RenderComponent<CultureSelector>();
    }
}