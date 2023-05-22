using Bunit;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components;
using Presentation.Components.Dialogues;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components;

[TestFixture]
public class LmsLoginButtonUt
{
    private TestContext _context;
    private IDialogService _dialogService;

    [SetUp]
    public void Setup()
    {
        _context = new TestContext();
        _dialogService = Substitute.For<IDialogService>();
        _context.Services.AddSingleton(_dialogService);
    }

    [Test]
    public void OnClick_DialogService()
    {
        using (_context)
        {
            var systemUnderTest = CreateTestableLmsLoginButtonComponent();

            var button = systemUnderTest.Find(".btn-standard");
            button.Click();

            _dialogService.Received().ShowAsync<LmsLoginDialog>();
        }
    }

    private IRenderedComponent<LmsLoginButton> CreateTestableLmsLoginButtonComponent()
    {
        return _context.RenderComponent<LmsLoginButton>();
    }
}