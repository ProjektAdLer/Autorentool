using System.Threading.Tasks;
using Bunit;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using NUnit.Framework;
using Presentation.Components.Dialogues;

namespace IntegrationTest.Dialogues;

[TestFixture]
public class GenericInfoDialogIt : MudDialogTestFixture<GenericInfoDialog>
{
    [Test]
    public async Task Render_RendersParametersCorrectly()
    {
        var parameters = new DialogParameters
        {
            { nameof(GenericInfoDialog.DialogText), (MarkupString)"This is the dialog text" },
            { nameof(GenericInfoDialog.OkButtonText), "Okay Dokey" },
            { nameof(GenericInfoDialog.OkButtonColor), Color.Info }
        };

        _ = await OpenDialogAndGetDialogReferenceAsync(parameters: parameters);

        var mudText = DialogProvider.FindComponents<MudText>()[1];
        var pInner = mudText.Find("p").InnerHtml;
        Assert.That(pInner, Is.EqualTo("This is the dialog text"));

        var buttons = DialogProvider.FindComponents<MudButton>();
        Assert.That(buttons, Has.Count.EqualTo(1));
        //button class tests colour
        var buttonText = buttons[0].Find("button.mud-button-text-info span").InnerHtml;
        Assert.That(buttonText, Is.EqualTo("Okay Dokey"));
    }

    [Test]
    public async Task OkButtonPressed_ClosesDialogWithPositiveResult()
    {
        var dialog = await OpenDialogAndGetDialogReferenceAsync();

        var buttons = DialogProvider.FindComponent<MudButton>();
        buttons.Find("button").Click();

        var result = await dialog.Result;
        Assert.That(result!.Canceled, Is.False);
        Assert.That(result.Data, Is.EqualTo(null));
    }
}