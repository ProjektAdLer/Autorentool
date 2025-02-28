using System.Threading.Tasks;
using Bunit;
using ElectronWrapper;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Dialogues;

namespace IntegrationTest.Dialogues;

[TestFixture]
public class UploadSuccessfulDialogIt : MudDialogTestFixture<UploadSuccessfulDialog>
{
    [SetUp]
    public new void Setup()
    {
        _shellwrapper = Substitute.For<IShellWrapper>();
        Context.Services.AddSingleton(_shellwrapper);
    }

    [TearDown]
    public new void TearDown()
    {
        Context.Dispose();
    }

    private IShellWrapper _shellwrapper;

    [Test]
    // ANF-ID: [AHO22]
    public async Task OkButtonPressed_ResultNotNull()
    {
        var parameters = new DialogParameters
        {
            { nameof(UploadSuccessfulDialog.Url3D), "This is the 3D URL" },
            { nameof(UploadSuccessfulDialog.UrlMoodle), "This is the Moodle URL" },
            { nameof(UploadSuccessfulDialog.WorldName), "This is the world name" }
        };

        var dialog = await OpenDialogAndGetDialogReferenceAsync(parameters: parameters);

        var buttons = DialogProvider.FindComponents<MudButton>();
        Assert.That(buttons, Has.Count.EqualTo(3));

        buttons[2].Find("button").Click();
        var result = await dialog.Result;
        Assert.That(result, Is.Not.EqualTo(null));
    }

    [Test]
    // ANF-ID: [AHO22]
    public async Task Open3DButtonPressed_CallsShellWrapperOpenUrl()
    {
        var parameters = new DialogParameters
        {
            { nameof(UploadSuccessfulDialog.Url3D), "This is the 3D URL" },
            { nameof(UploadSuccessfulDialog.UrlMoodle), "This is the Moodle URL" },
            { nameof(UploadSuccessfulDialog.WorldName), "This is the world name" }
        };

        _ = await OpenDialogAndGetDialogReferenceAsync(parameters: parameters);

        var buttons = DialogProvider.FindComponents<MudButton>();
        buttons[0].Find("button").Click();

        await _shellwrapper.Received().OpenPathAsync("This is the 3D URL");
    }

    [Test]
    // ANF-ID: [AHO22]
    public async Task OpenMoodleButtonPressed_CallsShellWrapperOpenUrl()
    {
        var parameters = new DialogParameters
        {
            { nameof(UploadSuccessfulDialog.Url3D), "This is the 3D URL" },
            { nameof(UploadSuccessfulDialog.UrlMoodle), "This is the Moodle URL" },
            { nameof(UploadSuccessfulDialog.WorldName), "This is the world name" }
        };

        _ = await OpenDialogAndGetDialogReferenceAsync(parameters: parameters);

        var buttons = DialogProvider.FindComponents<MudButton>();
        buttons[1].Find("button").Click();

        await _shellwrapper.Received().OpenPathAsync("This is the Moodle URL");
    }
}