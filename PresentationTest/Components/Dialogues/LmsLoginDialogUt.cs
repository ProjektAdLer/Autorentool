using Bunit;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Dialogues;
using Presentation.PresentationLogic.API;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components.Dialogues;

[TestFixture]
public class LmsLoginDialogUt
{
    private TestContext _context;
    private IPresentationLogic _presentationLogic;
    private IDialogService _dialogService;

    [SetUp]
    public void Setup()
    {
        _context = new TestContext();
        _presentationLogic = Substitute.For<IPresentationLogic>();
        _dialogService = Substitute.For<IDialogService>();
        _context.Services.AddSingleton(_presentationLogic);
        _context.Services.AddSingleton(_dialogService);
    }

    [Test]
    public void OnParametersSet_CallsPresentationLogic()
    {
        using (_context)
        {
            var systemUnderTest = CreateTestableLmsLoginDialogComponent();

            _presentationLogic.Received().IsLmsConnected();
        }
    }

    [Test]
    public void Render_IfLoggedIn_ShowLogout()
    {
        using (_context)
        {
            _presentationLogic.IsLmsConnected().Returns(true);
            _presentationLogic.LoginName.Returns("Test");

            var systemUnderTest = CreateTestableLmsLoginDialogComponent();

            //TODO Test does not render the MudDialog. Should search for the Logout button
            var text = systemUnderTest.Find("h3");
            Assert.That(text.TextContent, Is.EqualTo("Logout"));
        }
    }

    [Test]
    public void Render_IfNotLoggedIn_ShowLogin()
    {
        using (_context)
        {
            _presentationLogic.IsLmsConnected().Returns(false);

            var systemUnderTest = CreateTestableLmsLoginDialogComponent();
            
            //TODO Test does not render the MudDialog. Should search for the Login button
            var text = systemUnderTest.Find("h3");
            Assert.That(text.TextContent, Is.EqualTo("Login"));
        }
    }

    private IRenderedComponent<LmsLoginDialog> CreateTestableLmsLoginDialogComponent()
    {
        return _context.RenderComponent<LmsLoginDialog>();
    }
}