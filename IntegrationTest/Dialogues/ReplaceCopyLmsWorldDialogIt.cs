using System.Threading.Tasks;
using Bunit;
using MudBlazor;
using NUnit.Framework;
using Presentation.Components.Dialogues;
using Shared;

namespace IntegrationTest.Dialogues;

[TestFixture]
public class ReplaceCopyLmsWorldDialogIt : MudDialogTestFixture<ReplaceCopyLmsWorldDialog>
{
    [Test]
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
    public async Task CancelButtonPressed_CallsDialogAndReturnsResult()
    {
        var dialog = await OpenDialogAndGetDialogReferenceAsync();

        var buttons = DialogProvider.FindComponents<MudButton>();
        buttons[2].Find("button").Click();

        var result = await dialog.Result;
        Assert.That(result.Data, Is.Null);
        Assert.That(result.Canceled, Is.True);
    }
}