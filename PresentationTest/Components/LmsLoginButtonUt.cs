using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
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
    [SetUp]
    public void Setup()
    {
        _context = new TestContext();
        _context.ComponentFactories.AddStub<MudIconButton>();
        _dialogService = Substitute.For<IDialogService>();
        _localizer = Substitute.For<IStringLocalizer<LmsLoginButton>>();
        _context.Services.AddSingleton(_dialogService);
        _context.Services.AddSingleton(_localizer);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    private TestContext _context;
    private IDialogService _dialogService;
    private IStringLocalizer<LmsLoginButton> _localizer;

    [Test]
    public void Constructor_InjectsDependencies()
    {
        var systemUnderTest = CreateTestableLmsLoginButtonComponent();

        Assert.That(systemUnderTest.Instance.DialogService, Is.EqualTo(_dialogService));
        Assert.That(systemUnderTest.Instance.Localizer, Is.EqualTo(_localizer));
    }

    [Test]
    public void OnClick_DialogService()
    {
        using (_context)
        {
            var systemUnderTest = CreateTestableLmsLoginButtonComponent();

            var button = systemUnderTest.FindComponent<Stub<MudIconButton>>();
            systemUnderTest.InvokeAsync(() =>
                ((EventCallback<MouseEventArgs>)button.Instance.Parameters["OnClick"]).InvokeAsync(null));

            _dialogService.Received().ShowAsync<LmsLoginDialog>(Arg.Any<string>(), Arg.Any<DialogOptions>());
        }
    }

    private IRenderedComponent<LmsLoginButton> CreateTestableLmsLoginButtonComponent()
    {
        return _context.RenderComponent<LmsLoginButton>();
    }
}