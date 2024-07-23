using System.Threading.Tasks;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Dialogues;
using Shared;

namespace IntegrationTest.Dialogues;

[TestFixture]
public class ReplaceCopyLmsWorldDialogIt : MudDialogTestFixture<ReplaceCopyLmsWorldDialog>
{
    [SetUp]
    public void Setup()
    {
        var localizer = Substitute.For<IStringLocalizer<GenericCancellationConfirmationDialog>>();
        localizer[Arg.Any<string>()].Returns(ci => new LocalizedString(ci.Arg<string>(), ci.Arg<string>()));
        Context.Services.AddSingleton(localizer);
    }

    [TearDown]
    public void Teardown()
    {
        Context.Dispose();
    }

    [Test]
    // ANF-ID: [AHO22]
    public async Task CopyButtonPressed_CallsDialogAndReturnsResult()
    {
        var dialog = await OpenDialogAndGetDialogReferenceAsync();


        var buttons = DialogProvider.FindComponents<MudButton>();
        buttons[1].Find("button").Click();

        var result = await dialog.Result;
        Assert.That(result.Data, Is.EqualTo(ReplaceCopyLmsWorldDialogResult.Copy));
        Assert.That(result.Canceled, Is.False);
    }

    [Test]
    // ANF-ID: [AHO22]
    public async Task CancelButtonPressed_CallsDialogAndReturnsResult()
    {
        var dialog = await OpenDialogAndGetDialogReferenceAsync();

        var buttons = DialogProvider.FindComponents<MudButton>();
        buttons[2].Find("button").Click();

        var result = await dialog.Result;
        Assert.That(result.Data, Is.Null);
        Assert.That(result.Canceled, Is.True);
    }

    [Test]
    // ANF-ID: [AHO22]
    public async Task ReplaceButtonPressed_CallsDialogAndReturnsResult()
    {
        var dialog = await OpenDialogAndGetDialogReferenceAsync();

        var buttons = DialogProvider.FindComponents<MudButton>();
        buttons[0].Find("button").Click();

        // button[0] opens another dialog, so we have to close it first by clicking "Submit"
        var buttons2 = DialogProvider.FindComponents<MudButton>();
        buttons2[4].Find("button").Click();

        var result = await dialog.Result;
        Assert.That(result.Data, Is.EqualTo(ReplaceCopyLmsWorldDialogResult.Replace));
        Assert.That(result.Canceled, Is.False);
    }
}