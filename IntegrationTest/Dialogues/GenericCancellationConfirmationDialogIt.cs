using System.Threading.Tasks;
using Bunit;
using MudBlazor;
using NUnit.Framework;
using Presentation.Components.Dialogues;

namespace IntegrationTest.Dialogues;

[TestFixture]
public class GenericCancellationConfirmationDialogIt : MudBlazorTestFixture<GenericCancellationConfirmationDialog>
{
    [Test]
    public async Task Render_RendersParametersCorrectly()
    {
        var parameters = new DialogParameters
        {
            {nameof(GenericCancellationConfirmationDialog.DialogText), "This is the text of the dialog"},
            {nameof(GenericCancellationConfirmationDialog.SubmitButtonText), "Subshmit"},
            {nameof(GenericCancellationConfirmationDialog.SubmitButtonColor), Color.Info},
        };
        
        var dialog = await OpenDialogAndGetDialogReferenceAsync(parameters: parameters);
        
        var mudText = DialogProvider.FindComponents<MudText>()[1];
        var pInner = mudText.Find("p").InnerHtml;
        Assert.That(pInner, Is.EqualTo("This is the text of the dialog"));

        var buttons = DialogProvider.FindComponents<MudButton>();
        Assert.That(buttons, Has.Count.EqualTo(2));
        var buttonText = buttons[0].Find("button span").InnerHtml;
        Assert.That(buttonText, Is.EqualTo("GenericCancellationConfirmationDialog.Button.Cancel.Text"));
        //button class tests colour
        buttonText = buttons[1].Find("button.mud-button-text-info span").InnerHtml;
        Assert.That(buttonText, Is.EqualTo("Subshmit"));
    }

    [Test]
    public async Task CancelButtonPressed_ReturnsCancelledResult()
    {
        var dialog = await OpenDialogAndGetDialogReferenceAsync();
        
        var buttons = DialogProvider.FindComponents<MudButton>();
        buttons[0].Find("button").Click();
        var result = await dialog.Result;
        Assert.That(result.Canceled, Is.True);
        Assert.That(result.Data, Is.EqualTo(null));
    }

    [Test]
    public async Task SubmitButtonPressed_ReturnsPositiveResult()
    {
        var dialog = await OpenDialogAndGetDialogReferenceAsync();
        
        var buttons = DialogProvider.FindComponents<MudButton>();
        buttons[1].Find("button").Click();
        var result = await dialog.Result;
        Assert.That(result.Canceled, Is.False);
        Assert.That(result.Data, Is.EqualTo(true));
    }
}