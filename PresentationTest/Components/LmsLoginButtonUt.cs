using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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
        _context.ComponentFactories.AddStub<MudIconButton>();
        _dialogService = Substitute.For<IDialogService>();
        _context.Services.AddSingleton(_dialogService);
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

            _dialogService.Received().ShowAsync<LmsLoginDialog>();
        }
    }

    private IRenderedComponent<LmsLoginButton> CreateTestableLmsLoginButtonComponent()
    {
        return _context.RenderComponent<LmsLoginButton>();
    }
}