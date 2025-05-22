using Bunit;
using ElectronWrapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Dialogues.AdministrationDialog;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components.Dialogues.AdministrationDialog;

[TestFixture]
public class HelpDialogUt
{
    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _shellWrapper = Substitute.For<IShellWrapper>();
        var localizer = Substitute.For<IStringLocalizer<HelpDialog>>();
        localizer[Arg.Any<string>()].Returns(ci => new LocalizedString(ci.Arg<string>(), ci.Arg<string>()));

        _ctx.Services.AddSingleton(_shellWrapper);
        _ctx.Services.AddSingleton(localizer);
    }

    [TearDown]
    public void Teardown()
    {
        _ctx.Dispose();
    }

    private TestContext _ctx = null!;
    private IShellWrapper _shellWrapper = null!;

    [Test]
    public void ShowHelpDialog()
    {
        var helpDialog = _ctx.RenderComponent<HelpDialog>();

        var systemUnderTest = helpDialog.Instance;
        Assert.That(systemUnderTest, Is.Not.Null);
        Assert.That(systemUnderTest.ShellWrapper, Is.EqualTo(_shellWrapper));
    }

    [Test]
    [TestCase("p", "DialogContent.Help.UserManual",
        "https://projektadler.github.io/Documentation/manualauthoringautorentool.html")]
    [TestCase("a", "DialogContent.Help.UserManual.SoftwareComponents",
        "https://projektadler.github.io/Documentation/manualauthoringsoftwarekomponenten.html")]
    [TestCase("a", "DialogContent.Help.UserManual.Handling",
        "https://projektadler.github.io/Documentation/manualauthoringbedienung.html")]
    [TestCase("p", "DialogContent.Help.Tutorial",
        "https://projektadler.github.io/Documentation/handreichung-didaktik-bd.html")]
    [TestCase("a", "DialogContent.Help.Didactic.AdaptivityElement",
        "https://projektadler.github.io/Documentation/didaktik-autorentool-adaptivitaetselement-bd.html")]
    [TestCase("a", "DialogContent.Help.Didactic.LearningOutcomeGuide",
        "https://projektadler.github.io/Documentation/didaktik-autorentool-lernziele.html")]
    [TestCase("a", "DialogContent.Help.Didactic.StoryElement",
        "https://projektadler.github.io/Documentation/didaktik-autorentool-game-design-elemente-bd.html")]
    [TestCase("a", "Feedback.MenuItem.UX.Text", "https://www.soscisurvey.de/autorentoolevaluation_gesamt")]
    [TestCase("a", "Feedback.MenuItem.Technical.Text", "https://bugreport.projekt-adler.eu")]
    [TestCase("a", "DialogContent.Help.About", "https://www.projekt-adler.eu")]
    public void OpenLink_ClickButton_OpensCorrectUrl(string htmlType, string linkText, string expectedUrl)
    {
        // Arrange
        var cut = _ctx.RenderComponent<HelpDialog>();

        // Act
        cut.Find($"{htmlType}[class*='text-sm']:contains('{linkText}')").Click();

        // Assert
        _shellWrapper.Received(1).OpenPathAsync(expectedUrl);
    }
}