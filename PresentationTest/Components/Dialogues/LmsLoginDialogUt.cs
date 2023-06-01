using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
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
        _context.Services.AddMudServices();
        _context.ComponentFactories.AddStub<MudDialog>();
        _context.ComponentFactories.AddStub<MudForm>();
        _context.ComponentFactories.AddStub<MudText>();
        _context.ComponentFactories.AddStub<MudButton>();
        _context.ComponentFactories.AddStub<MudTextField<string>>();
        _context.ComponentFactories.AddStub<MudDialog>();
        // DialogContent belongs to MudDialog
        _context.ComponentFactories.AddStub<MudText>();
        _context.ComponentFactories.AddStub<MudButton>();
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

            var dialogContent = _context.Render((RenderFragment) systemUnderTest.FindComponent<Stub<MudDialog>>()
                .Instance.Parameters["DialogContent"]);
            var mudTextStub = _context.Render((RenderFragment) dialogContent.FindComponent<Stub<MudText>>().Instance
                .Parameters["ChildContent"]);
            var mudButtonStub = _context.Render((RenderFragment) dialogContent.FindComponent<Stub<MudButton>>().Instance
                .Parameters["ChildContent"]);

            Assert.That(mudTextStub.Markup, Is.EqualTo("Logged in as Test"));
            Assert.That(mudButtonStub.Markup, Is.EqualTo("Logout"));
        }
    }

    [Test]
    public void Render_IfNotLoggedIn_ShowLogin()
    {
        using (_context)
        {
            _presentationLogic.IsLmsConnected().Returns(false);

            var systemUnderTest = CreateTestableLmsLoginDialogComponent();

            var dialogContent = _context.Render((RenderFragment) systemUnderTest.FindComponent<Stub<MudDialog>>()
                .Instance.Parameters["DialogContent"]);
            var mudFormStub = _context.Render((RenderFragment) dialogContent.FindComponent<Stub<MudForm>>().Instance
                .Parameters["ChildContent"]);
            var mudButtonStub = _context.Render((RenderFragment) mudFormStub.FindComponent<Stub<MudButton>>().Instance
                .Parameters["ChildContent"]);

            Assert.That(mudButtonStub.Markup, Is.EqualTo("Login"));
        }
    }

    private IRenderedComponent<LmsLoginDialog> CreateTestableLmsLoginDialogComponent()
    {
        return _context.RenderComponent<LmsLoginDialog>(p => p
            .AddCascadingValue(_context.RenderComponent<MudDialogInstance>().Instance));
    }
}