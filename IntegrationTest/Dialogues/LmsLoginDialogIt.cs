using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bunit;
using BusinessLogic.ErrorManagement.BackendAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MudBlazor;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Presentation.Components.Dialogues;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using PresentationTest;
using Shared.Configuration;
using Shared.Exceptions;

namespace IntegrationTest.Dialogues;

[TestFixture]
public class LmsLoginDialogIt : MudDialogTestFixture<LmsLoginDialog>
{
    [SetUp]
    public void SetUp()
    {
        _presentationLogic = Substitute.For<IPresentationLogic>();
        _applicationConfiguration = Substitute.For<IApplicationConfiguration>();
        _errorService = Substitute.For<IErrorService>();
        _logger = Substitute.For<ILogger<LmsLoginDialog>>();
        Context.Services.AddSingleton(_presentationLogic);
        Context.Services.AddSingleton(_applicationConfiguration);
        Context.Services.AddSingleton(_errorService);
        Context.Services.AddSingleton(_logger);
    }

    private IPresentationLogic _presentationLogic = null!;
    private IApplicationConfiguration _applicationConfiguration = null!;
    private IErrorService _errorService = null!;
    private ILogger<LmsLoginDialog> _logger = null!;

    [Test]
    public async Task DialogCreated_DependenciesInjected()
    {
        await OpenDialogAndGetDialogReferenceAsync();

        var systemUnderTest = DialogProvider.FindComponentOrFail<LmsLoginDialog>();

        Assert.That(systemUnderTest.Instance.PresentationLogic, Is.EqualTo(_presentationLogic));
        Assert.That(systemUnderTest.Instance.Configuration, Is.EqualTo(_applicationConfiguration));
        Assert.That(systemUnderTest.Instance.ErrorService, Is.EqualTo(_errorService));
    }

    [Test]
    public async Task DialogCreated_CallsPresentationLogicForLmsConnection()
    {
        await OpenDialogAndGetDialogReferenceAsync();

        await _presentationLogic.Received(1).IsLmsConnected();
    }

    [Test]
    public async Task DialogCreated_LmsNotConnected_RenderLmsLoginDialogWithForm()
    {
        _presentationLogic.IsLmsConnected().Returns(false);
        await OpenDialogAndGetDialogReferenceAsync();

        var mudTexts = DialogProvider.FindComponents<MudText>();
        Assert.That(mudTexts, Has.Count.EqualTo(4));
        Assert.Multiple(() =>
        {
            Assert.That(mudTexts[0].Markup, Contains.Substring("DialogContent.Header"));
            Assert.That(mudTexts[1].Markup, Contains.Substring(""));
            Assert.That(mudTexts[2].Markup, Contains.Substring(""));
            Assert.That(mudTexts[3].Markup, Contains.Substring("DialogContent.Button.Login"));
        });

        var mudTextFields = DialogProvider.FindComponents<MudTextField<string>>();
        Assert.That(mudTextFields, Has.Count.EqualTo(3));
        Assert.Multiple(() =>
        {
            Assert.That(mudTextFields[0].Instance.Label, Is.EqualTo("DialogContent.Field.BackendUrl"));
            Assert.That(mudTextFields[1].Instance.Label, Is.EqualTo("DialogContent.Field.Username"));
            Assert.That(mudTextFields[2].Instance.Label, Is.EqualTo("DialogContent.Field.Password"));
        });

        var mudButtons = DialogProvider.FindComponents<MudButton>();
        Assert.That(mudButtons, Has.Count.EqualTo(1));

        var mudLists = DialogProvider.FindComponents<MudList>();
        //Left sidebar (LoginDialog, PersonDialog)
        Assert.That(mudLists, Has.Count.EqualTo(1));
        var mudListItems = mudLists[0].FindComponents<MudListItem>();
        Assert.That(mudListItems, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task DialogCreated_LmsConnected_RenderLmsLoginDialogWithWorldsAndLogoutButton()
    {
        _presentationLogic.IsLmsConnected().Returns(true);
        _presentationLogic.LoginName.Returns("MyUsername");
        _applicationConfiguration[IApplicationConfiguration.BackendBaseUrl].Returns("http://foobar.com");
        var worldList = new List<LmsWorldViewModel>()
        {
            new() { WorldId = 0, WorldName = "world1" },
            new() { WorldId = 1, WorldName = "world2" },
            new() { WorldId = 2, WorldName = "world3" }
        };
        _presentationLogic.GetLmsWorldList().Returns(worldList);
        await OpenDialogAndGetDialogReferenceAsync();

        var mudTexts = DialogProvider.FindComponents<MudText>();
        Assert.That(mudTexts, Has.Count.EqualTo(8));
        Assert.Multiple(() =>
        {
            Assert.That(mudTexts[0].Markup, Contains.Substring("DialogContent.Header"));
            Assert.That(mudTexts[1].Markup, Contains.Substring(""));
            Assert.That(mudTexts[2].Markup, Contains.Substring(""));
            Assert.That(mudTexts[3].Markup, Contains.Substring("DialogContent.Button.Logout"));
            Assert.That(mudTexts[4].Markup, Contains.Substring("DialogContent.Delete.Subtitle"));
            Assert.That(mudTexts[5].Markup, Contains.Substring("DialogContent.Delete.MoodleCourse"));
            Assert.That(mudTexts[6].Markup, Contains.Substring("DialogContent.Delete.MoodleCourse"));
            Assert.That(mudTexts[7].Markup, Contains.Substring("DialogContent.Delete.MoodleCourse"));
        });

        var mudTextFields = DialogProvider.FindComponents<MudTextField<string>>();
        Assert.That(mudTextFields, Has.Count.EqualTo(0));

        var mudButtons = DialogProvider.FindComponents<MudButton>();
        Assert.That(mudButtons, Has.Count.EqualTo(1));

        var mudLists = DialogProvider.FindComponents<MudList>();
        Assert.That(mudLists, Has.Count.EqualTo(2));
        var mudListItems = mudLists[1].FindComponents<MudListItem>();
        Assert.That(mudListItems, Has.Count.EqualTo(3));
        Assert.Multiple(() =>
        {
            Assert.That(mudListItems[0].Instance.Text, Is.EqualTo("world1"));
            Assert.That(mudListItems[1].Instance.Text, Is.EqualTo("world2"));
            Assert.That(mudListItems[2].Instance.Text, Is.EqualTo("world3"));

            Assert.That(mudListItems[0].FindComponent<MudIconButton>().Instance.Title,
                Is.EqualTo("DialogContent.Delete.MoodleCourse"));
            Assert.That(mudListItems[1].FindComponent<MudIconButton>().Instance.Title,
                Is.EqualTo("DialogContent.Delete.MoodleCourse"));
            Assert.That(mudListItems[2].FindComponent<MudIconButton>().Instance.Title,
                Is.EqualTo("DialogContent.Delete.MoodleCourse"));
        });
    }

    [Test]
    public async Task DialogCreated_IsLmsConnectedThrowsBackendApiUnreachableException_ShowsErrorMessage()
    {
        _presentationLogic.IsLmsConnected().Throws(x => throw new BackendApiUnreachableException());
        
        Localizer["DialogContent.Error.APIUnreachable"]
            .Returns(new LocalizedString("DialogContent.Error.APIUnreachable","API is unreachable"));
        
        await OpenDialogAndGetDialogReferenceAsync();

        var mudTexts = DialogProvider.FindComponents<MudText>();
        Assert.That(mudTexts, Has.Count.EqualTo(5));
        DialogProvider.WaitForAssertion(() =>
        {
            var errorElement = DialogProvider.Find("h6.mud-error-text");
            Assert.That(errorElement, Is.Not.Null);

            var errorText = errorElement.TextContent.Trim();
            Assert.That(errorText, Is.EqualTo("API is unreachable"));
        });
    }

    [Test]
    public async Task DialogCreated_IsLmsConnectedThrowsBackendInvalidTokenException_ShowsErrorMessageAndCallsLogout()
    {
        var exceptionThrown = false;
        _presentationLogic.IsLmsConnected().ReturnsForAnyArgs(x =>
        {
            if (exceptionThrown)
            {
                return false;
            }

            exceptionThrown = true;
            throw new BackendInvalidTokenException();
        });
        await OpenDialogAndGetDialogReferenceAsync();

        var mudTexts = DialogProvider.FindComponents<MudText>();
        Assert.That(mudTexts, Has.Count.EqualTo(5));
        DialogProvider.WaitForAssertion(() =>
        {
            var errorElement = DialogProvider.Find("h6.mud-error-text");
            Assert.That(errorElement, Is.Not.Null);

            var errorText = errorElement.TextContent.Trim();
            Assert.That(errorText, Is.EqualTo("DialogContent.Error.TokenInvalid"));
        });

        await _presentationLogic.Received(2).IsLmsConnected();
        _presentationLogic.Received(1).Logout();
    }

    [Test]
    public async Task DialogCreated_LmsIsConnected_GetLmsWorldListThrowsBackendException_SetErrorInErrorService()
    {
        Localizer["DialogContent.AdLerServer.ErrorMessage.Refresh"]
            .Returns(new LocalizedString("DialogContent.AdLerServer.ErrorMessage.Refresh","Error while trying to get the LMS world list"));
        
        _presentationLogic.IsLmsConnected().Returns(true);
        _presentationLogic.GetLmsWorldList().Throws(new BackendException("nix gut"));
        await OpenDialogAndGetDialogReferenceAsync();

        _errorService.Received(1).SetError("Error while trying to get the LMS world list", "nix gut");
    }

    [Test]
    public async Task EnterDetailsAndClickLoginButton_MissingValue_NothingHappens()
    {
        _presentationLogic.IsLmsConnected().Returns(false);
        await OpenDialogAndGetDialogReferenceAsync();

        var mudTextFields = DialogProvider.FindComponentsOrFail<MudTextField<string>>().ToArray();
        mudTextFields[0].Find("input").Change("http://foobar.com");
        mudTextFields[1].Find("input").Change("Username");

        var mudButtons = DialogProvider.FindComponentsOrFail<MudButton>().ToArray();
        mudButtons[0].Find("button").Click();

        await _presentationLogic.Received(0).Login(Arg.Any<string>(), Arg.Any<string>());
    }

    [Test]
    // ANF-ID: [AHO21]
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
    public async Task EnterDetailsAndPressEnterInPasswordField_CallsPresentationLogic()
    {
        _presentationLogic.IsLmsConnected().Returns(false);
        await OpenDialogAndGetDialogReferenceAsync();

        var mudTextFields = DialogProvider.FindComponentsOrFail<MudTextField<string>>().ToArray();
        mudTextFields[0].Find("input").Change("https://foobar.com");
        mudTextFields[1].Find("input").Change("Username");
        mudTextFields[2].Find("input").Change("Password");
        mudTextFields[2].Find("input").KeyPress(Key.Enter);

        _applicationConfiguration.Received()[IApplicationConfiguration.BackendBaseUrl] = "https://foobar.com";
        _applicationConfiguration.Received()[IApplicationConfiguration.BackendUsername] = "Username";
        await _presentationLogic.Received(1).Login("Username", "Password");
    }

    [Test]
    public async Task EnterDetailsAndClickLoginButton_CorrectCredentials_CallsGetLmsWorldListInPresentationLogic()
    {
        _presentationLogic.IsLmsConnected().Returns(false);
        await OpenDialogAndGetDialogReferenceAsync();

        _presentationLogic.Login(Arg.Any<string>(), Arg.Any<string>()).Returns(Task.CompletedTask);
        _presentationLogic.When(x => x.Login(Arg.Any<string>(), Arg.Any<string>()))
            .Do(x => _presentationLogic.IsLmsConnected().Returns(true));

        var worldList = new List<LmsWorldViewModel>();
        _presentationLogic.GetLmsWorldList().Returns(worldList);

        var mudTextFields = DialogProvider.FindComponentsOrFail<MudTextField<string>>().ToArray();
        mudTextFields[0].Find("input").Change("http://foobar.com");
        mudTextFields[1].Find("input").Change("Username");
        mudTextFields[2].Find("input").Change("Password");

        var mudButtons = DialogProvider.FindComponentsOrFail<MudButton>().ToArray();
        mudButtons[0].Find("button").Click();

        await _presentationLogic.Received(1).Login("Username", "Password");

        await _presentationLogic.Received(1).GetLmsWorldList();
    }

    [Test]
    // ANF-ID: [AHO21]
    public async Task EnterDetailsAndClickLoginButton_UrlDoesNotIncludeProtocol_ShowsErrorMessage()
    {
        Localizer["DialogContent.Error.ProtocolMissing"]
            .Returns(new LocalizedString("DialogContent.Error.ProtocolMissing","Protocol is missing in the URL"));
        
        _presentationLogic.IsLmsConnected().Returns(false);
        await OpenDialogAndGetDialogReferenceAsync();

        var mudTextFields = DialogProvider.FindComponentsOrFail<MudTextField<string>>().ToArray();
        mudTextFields[0].Find("input").Change("NoProto");
        mudTextFields[1].Find("input").Change("Username");
        mudTextFields[2].Find("input").Change("Password");

        var mudButtons = DialogProvider.FindComponentsOrFail<MudButton>().ToArray();
        mudButtons[0].Find("button").Click();

        await _presentationLogic.Received(0).Login(Arg.Any<string>(), Arg.Any<string>());
        DialogProvider.WaitForAssertion(() =>
        {
            var errorElement = DialogProvider.Find("h6.mud-error-text");
            Assert.That(errorElement, Is.Not.Null);

            var errorText = errorElement.TextContent.Trim();
            Assert.That(errorText, Is.EqualTo("Protocol is missing in the URL"));
        });
    }

    [Test]
    // ANF-ID: [AHO21]
    public async Task
        EnterDetailsAndClickLoginButton_PresentationThrowsBackendInvalidLoginException_ExceptionIsHandled()
    {
        Localizer["DialogContent.Error.WrongUserOrPassword"]
            .Returns(new LocalizedString("DialogContent.Error.WrongUserOrPassword","Wrong username or password"));
        
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

        DialogProvider.WaitForAssertion(() =>
        {
            var errorElement = DialogProvider.Find("h6.mud-error-text");
            Assert.That(errorElement, Is.Not.Null);

            var errorText = errorElement.TextContent.Trim();
            Assert.That(errorText, Is.EqualTo("Wrong username or password"));
        });
    }

    [Test]
    // ANF-ID: [AHO21]
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

        DialogProvider.WaitForAssertion(() =>
        {
            var errorElement = DialogProvider.Find("h6.mud-error-text");
            Assert.That(errorElement, Is.Not.Null);

            var errorText = errorElement.TextContent.Trim();
            Assert.That(errorText, Is.EqualTo("nix gut"));
        });
    }

    [Test]
    // ANF-ID: [AHO21]
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
        
        DialogProvider.WaitForAssertion(() =>
        {
            var errorElement = DialogProvider.Find("h6.invalid-login-error");
            Assert.That(errorElement,Is.Not.Null);

            var errorText = errorElement.TextContent.Trim();
            Assert.That(errorText, Is.EqualTo("DialogContent.Error.APIUnreachable"));
        });
    }

    [Test]
    // ANF-ID: [AHO21]
    public async Task ClickLoginButton_PresentationThrowsUnknownException_ExceptionIsHandled()
    {
        _presentationLogic.IsLmsConnected().Returns(false);
        await OpenDialogAndGetDialogReferenceAsync();

        _presentationLogic.When(x => x.Login(Arg.Any<string>(), Arg.Any<string>()))
            .Throw(new Exception("nix gut"));

        var mudTextFields = DialogProvider.FindComponentsOrFail<MudTextField<string>>().ToArray();
        mudTextFields[0].Find("input").Change("http://foobar.com");
        mudTextFields[1].Find("input").Change("Username");
        mudTextFields[2].Find("input").Change("Password");

        Assert.That(_logger.ReceivedCalls().Count(), Is.EqualTo(0));

        var mudButtons = DialogProvider.FindComponentsOrFail<MudButton>().ToArray();
        mudButtons[0].Find("button").Click();

        Assert.That(_logger.ReceivedCalls().Count(), Is.EqualTo(1));
    }

    [Test]
    // ANF-ID: [AHO21, AHO25]
    public async Task IsLmsConnectedTrue_ShowsConnectedDialog()
    {
        _presentationLogic.IsLmsConnected().Returns(true);
        _presentationLogic.LoginName.Returns("MySecretUsername");
        _presentationLogic.GetLmsWorldList().Returns(new List<LmsWorldViewModel>());

        _applicationConfiguration[IApplicationConfiguration.BackendBaseUrl] = "http://foobar.com";
        await OpenDialogAndGetDialogReferenceAsync();

        var loggedInPs = DialogProvider.FindAll("div.logged-in-container div p");

        Assert.That(loggedInPs, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(loggedInPs[0].InnerHtml, Is.EqualTo("DialogContent.LoggedIn.Message"));
            Assert.That(loggedInPs[1].InnerHtml, Is.EqualTo("MySecretUsername (http://foobar.com)"));
        });
    }

    [Test]
    // ANF-ID: [AHO25]
    public async Task IsLmsConnectedTrue_LogoutButtonClicked_CallsLogout()
    {
        Context.ComponentFactories.AddStub<MudList>();
        Context.ComponentFactories.AddStub<MudListItem>();
        _presentationLogic.IsLmsConnected().Returns(true);
        _presentationLogic.LoginName.Returns("MySecretUsername");
        await OpenDialogAndGetDialogReferenceAsync();

        _presentationLogic.Received(0).Logout();
        var mudButtons = DialogProvider.FindComponentsOrFail<MudButton>().ToArray();
        mudButtons[0].Find("button").Click();
        _presentationLogic.Received(1).Logout();
    }

    [Test]
    public async Task DeleteWorldButtonClicked_CancelDialog_NothingHappens()
    {
        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Cancel());
        var dialogService = Substitute.For<IDialogService>();
        dialogService.ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>())
            .Returns(dialogReference);

        _presentationLogic.IsLmsConnected().Returns(true);
        var worldList = new List<LmsWorldViewModel>()
        {
            new() { WorldId = 0, WorldName = "world1" },
            new() { WorldId = 1, WorldName = "world2" },
            new() { WorldId = 2, WorldName = "world3" }
        };
        _presentationLogic.GetLmsWorldList().Returns(worldList);
        var reference = await OpenDialogAndGetDialogReferenceAsync();
        ((LmsLoginDialog)reference.Dialog).DialogService = dialogService;


        var mudListItems = DialogProvider.FindComponents<MudList>()[1].FindComponents<MudListItem>();
        Assert.That(mudListItems, Has.Count.EqualTo(3));
        mudListItems[0].FindComponent<MudIconButton>().Find("button").Click();

        await dialogService.Received(1)
            .ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>());
        await _presentationLogic.Received(0).DeleteLmsWorld(Arg.Any<LmsWorldViewModel>());
    }

    [Test]
    public async Task DeleteWorldButtonClicked_ConfirmDialog_CallsDeleteLmsWorldAndGetLmsWorldList()
    {
        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Ok(true));
        var dialogService = Substitute.For<IDialogService>();
        dialogService.ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>())
            .Returns(dialogReference);

        _presentationLogic.IsLmsConnected().Returns(true);
        var worldList = new List<LmsWorldViewModel>()
        {
            new() { WorldId = 0, WorldName = "world1" },
            new() { WorldId = 1, WorldName = "world2" },
            new() { WorldId = 2, WorldName = "world3" }
        };
        _presentationLogic.GetLmsWorldList().Returns(worldList);
        var reference = await OpenDialogAndGetDialogReferenceAsync();
        ((LmsLoginDialog)reference.Dialog).DialogService = dialogService;
        await _presentationLogic.Received(1).GetLmsWorldList();
        _presentationLogic.ClearReceivedCalls();

        var mudListItems = DialogProvider.FindComponents<MudList>()[1].FindComponents<MudListItem>();
        Assert.That(mudListItems, Has.Count.EqualTo(3));
        mudListItems[0].FindComponent<MudIconButton>().Find("button").Click();

        await dialogService.Received(1)
            .ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>());
        await _presentationLogic.Received(1).DeleteLmsWorld(Arg.Any<LmsWorldViewModel>());
        await _presentationLogic.Received(1).GetLmsWorldList();
    }

    [Test]
    public async Task
        DeleteWorldButtonClicked_ConfirmDialog_PresentationLogicThrowsBackendException_SetsErrorInErrorService()
    {
        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Ok(true));
        var dialogService = Substitute.For<IDialogService>();
        dialogService.ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>())
            .Returns(dialogReference);

        Localizer["DialogContent.AdLerServer.ErrorMessage.Delete"]
            .Returns(new LocalizedString("DialogContent.AdLerServer.ErrorMessage.Delete","Error while trying to delete the LMS world"));

        _presentationLogic.DeleteLmsWorld(Arg.Any<LmsWorldViewModel>()).Throws(new BackendException("nix gut"));

        _presentationLogic.IsLmsConnected().Returns(true);
        var worldList = new List<LmsWorldViewModel>()
        {
            new() { WorldId = 0, WorldName = "world1" },
            new() { WorldId = 1, WorldName = "world2" },
            new() { WorldId = 2, WorldName = "world3" }
        };
        _presentationLogic.GetLmsWorldList().Returns(worldList);
        var reference = await OpenDialogAndGetDialogReferenceAsync();
        ((LmsLoginDialog)reference.Dialog).DialogService = dialogService;

        var mudListItems = DialogProvider.FindComponents<MudList>()[1].FindComponents<MudListItem>();
        Assert.That(mudListItems, Has.Count.EqualTo(3));
        mudListItems[0].FindComponent<MudIconButton>().Find("button").Click();

        await dialogService.Received(1)
            .ShowAsync<GenericCancellationConfirmationDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>());
        _errorService.Received(1).SetError("Error while trying to delete the LMS world", "nix gut");
    }

    [Test]
    public async Task ClickCloseDialogButton_CallsCloseDialog()
    {
        var dialog = await OpenDialogAndGetDialogReferenceAsync();

        var mudIconButtons = DialogProvider.FindComponents<MudIconButton>();
        Assert.Multiple(() =>
        {
            Assert.That(mudIconButtons[0].Instance.Icon, Is.EqualTo(Icons.Material.Filled.Close));
            Assert.That(DialogProvider.Markup, Is.Not.Empty);
        });

        mudIconButtons[0].Find("button").Click();

        var result = await dialog.Result;
        Assert.Multiple(() =>
        {
            Assert.That(result.Canceled, Is.False);
            Assert.That(DialogProvider.Markup, Is.Empty);
        });
    }
}