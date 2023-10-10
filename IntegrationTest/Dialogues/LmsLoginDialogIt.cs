using System.Linq;
using System.Threading.Tasks;
using Bunit;
using BusinessLogic.ErrorManagement.BackendAccess;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Dialogues;
using Presentation.PresentationLogic.API;
using PresentationTest;
using Shared.Configuration;

namespace IntegrationTest.Dialogues;

[TestFixture]
public class LmsLoginDialogIt : MudDialogTestFixture<LmsLoginDialog>
{
    [SetUp]
    public void SetUp()
    {
        _presentationLogic = Substitute.For<IPresentationLogic>();
        _applicationConfiguration = Substitute.For<IApplicationConfiguration>();
        Context.Services.AddSingleton(_presentationLogic);
        Context.Services.AddSingleton(_applicationConfiguration);
    }

    private IPresentationLogic _presentationLogic = null!;
    private IApplicationConfiguration _applicationConfiguration = null!;

    [Test]
    public async Task DialogCreated_DependenciesInjected()
    {
        await OpenDialogAndGetDialogReferenceAsync();

        var systemUnderTest = DialogProvider.FindComponentOrFail<LmsLoginDialog>();

        Assert.That(systemUnderTest.Instance.PresentationLogic, Is.EqualTo(_presentationLogic));
        Assert.That(systemUnderTest.Instance.Configuration, Is.EqualTo(_applicationConfiguration));
    }

    [Test]
    public async Task EnterDetailsAndClickLoginButton_CallsPresentationLogic()
    {
        _presentationLogic.IsLmsConnected().Returns(false);
        await OpenDialogAndGetDialogReferenceAsync();

        var mudTextFields = DialogProvider.FindComponentsOrFail<MudTextField<string>>().ToArray();
        mudTextFields[0].Find("input").Change("http://foobar.com");
        mudTextFields[1].Find("input").Change("Username");
        mudTextFields[2].Find("input").Change("Password");

        var mudButtons = DialogProvider.FindComponentsOrFail<MudButton>().ToArray();
        mudButtons[0].Find("button").Click();

        _applicationConfiguration.Received()[IApplicationConfiguration.BackendBaseUrl] = "http://foobar.com";
        _applicationConfiguration.Received()[IApplicationConfiguration.BackendUsername] = "Username";
        await _presentationLogic.Received(1).Login("Username", "Password");
    }

    [Test]
    public async Task EnterDetailsAndClickLoginButton_UrlDoesNotIncludeProtocol_ShowsErrorMessage()
    {
        _presentationLogic.IsLmsConnected().Returns(false);
        await OpenDialogAndGetDialogReferenceAsync();

        _presentationLogic.When(x => x.Login(Arg.Any<string>(), Arg.Any<string>()))
            .Throw(new BackendInvalidLoginException());

        var mudTextFields = DialogProvider.FindComponentsOrFail<MudTextField<string>>().ToArray();
        mudTextFields[0].Find("input").Change("NoProto");
        mudTextFields[1].Find("input").Change("Username");
        mudTextFields[2].Find("input").Change("Password");

        var mudButtons = DialogProvider.FindComponentsOrFail<MudButton>().ToArray();
        mudButtons[0].Find("button").Click();

        var mudText = DialogProvider.FindComponents<MudText>();
        Assert.That(mudText[0].Find("h6").InnerHtml, Is.EqualTo("DialogContent.Error.ProtocolMissing"));
    }

    [Test]
    public async Task
        EnterDetailsAndClickLoginButton_PresentationThrowsBackendInvalidLoginException_ExceptionIsHandled()
    {
        _presentationLogic.IsLmsConnected().Returns(false);
        await OpenDialogAndGetDialogReferenceAsync();

        _presentationLogic.When(x => x.Login(Arg.Any<string>(), Arg.Any<string>()))
            .Throw(new BackendInvalidLoginException());

        var mudTextFields = DialogProvider.FindComponentsOrFail<MudTextField<string>>().ToArray();
        mudTextFields[0].Find("input").Change("http://foobar.com");
        mudTextFields[1].Find("input").Change("Username");
        mudTextFields[2].Find("input").Change("Password");

        var mudButtons = DialogProvider.FindComponentsOrFail<MudButton>().ToArray();
        mudButtons[0].Find("button").Click();

        var mudText = DialogProvider.FindComponents<MudText>();
        Assert.That(mudText[0].Find("h6").InnerHtml, Is.EqualTo("DialogContent.Error.WrongUserOrPassword"));
    }

    [Test]
    public async Task ClickLoginButton_PresentationThrowsBackendInvalidUrlException_ExceptionIsHandled()
    {
        _presentationLogic.IsLmsConnected().Returns(false);
        await OpenDialogAndGetDialogReferenceAsync();

        _presentationLogic.When(x => x.Login(Arg.Any<string>(), Arg.Any<string>()))
            .Throw(new BackendInvalidUrlException("nix gut"));

        var mudTextFields = DialogProvider.FindComponentsOrFail<MudTextField<string>>().ToArray();
        mudTextFields[0].Find("input").Change("http://foobar");
        mudTextFields[1].Find("input").Change("Username");
        mudTextFields[2].Find("input").Change("Password");

        var mudButtons = DialogProvider.FindComponentsOrFail<MudButton>().ToArray();
        mudButtons[0].Find("button").Click();

        var mudText = DialogProvider.FindComponents<MudText>();
        Assert.That(mudText[0].Find("h6").InnerHtml, Is.EqualTo("nix gut"));
    }

    [Test]
    public async Task ClickLoginButton_PresentationThrowsBackendApiUnreachableException_ExceptionIsHandled()
    {
        _presentationLogic.IsLmsConnected().Returns(false);
        await OpenDialogAndGetDialogReferenceAsync();

        _presentationLogic.When(x => x.Login(Arg.Any<string>(), Arg.Any<string>()))
            .Throw(new BackendApiUnreachableException("nix gut"));

        var mudTextFields = DialogProvider.FindComponentsOrFail<MudTextField<string>>().ToArray();
        mudTextFields[0].Find("input").Change("http://foobar");
        mudTextFields[1].Find("input").Change("Username");
        mudTextFields[2].Find("input").Change("Password");

        var mudButtons = DialogProvider.FindComponentsOrFail<MudButton>().ToArray();
        mudButtons[0].Find("button").Click();

        var mudText = DialogProvider.FindComponents<MudText>();
        Assert.That(mudText[0].Find("h6").InnerHtml, Is.EqualTo("DialogContent.Error.APIUnreachable"));
    }

    [Test]
    public async Task ClickLoginButton_PresentationThrowsUnknownException_ExceptionIsHandled()
    {
        _presentationLogic.IsLmsConnected().Returns(false);
        await OpenDialogAndGetDialogReferenceAsync();

        _presentationLogic.When(x => x.Login(Arg.Any<string>(), Arg.Any<string>()))
            .Throw(new System.Exception("nix gut"));

        var mudTextFields = DialogProvider.FindComponentsOrFail<MudTextField<string>>().ToArray();
        mudTextFields[0].Find("input").Change("URL");
        mudTextFields[1].Find("input").Change("Username");
        mudTextFields[2].Find("input").Change("Password");

        var mudButtons = DialogProvider.FindComponentsOrFail<MudButton>().ToArray();
    }

    [Test]
    public async Task IsLmsConnectedTrue_ShowsConnectedDialog()
    {
        _presentationLogic.IsLmsConnected().Returns(true);
        _presentationLogic.LoginName.Returns("MySecretUsername");
        await OpenDialogAndGetDialogReferenceAsync();

        var loggedInPs = DialogProvider.FindAll("div.logged-in-container div p");

        Assert.That(loggedInPs, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(loggedInPs[0].InnerHtml, Is.EqualTo("DialogContent.LoggedIn.Message"));
            Assert.That(loggedInPs[1].InnerHtml, Is.EqualTo("MySecretUsername"));
        });
    }

    [Test]
    public async Task IsLmsConnectedTrue_LogoutButtonClicked_CallsLogout()
    {
        _presentationLogic.IsLmsConnected().Returns(true);
        _presentationLogic.LoginName.Returns("MySecretUsername");
        await OpenDialogAndGetDialogReferenceAsync();

        _presentationLogic.Received(0).Logout();
        var mudButtons = DialogProvider.FindComponentsOrFail<MudButton>().ToArray();
        mudButtons[0].Find("button").Click();
        _presentationLogic.Received(1).Logout();
    }
}