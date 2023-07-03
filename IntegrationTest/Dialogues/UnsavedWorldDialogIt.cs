using System.Threading.Tasks;
using Bunit;
using MudBlazor;
using NUnit.Framework;
using Presentation.Components.Dialogues;

namespace IntegrationTest.Dialogues;

[TestFixture]
public class UnsavedWorldDialogIt : MudBlazorTestFixture<UnsavedWorldDialog>
{
    [Test]
    public void Render_RendersParametersCorrectly()
    {
        var parameters = new DialogParameters
        {
            {nameof(UnsavedWorldDialog.WorldName), "TestWorld"},
        };

        var dialog = OpenDialogAndGetDialogReferenceAsync(parameters: parameters);

        var mudText = DialogProvider.FindComponents<MudText>()[1];
        var pInner = mudText.Find("p").InnerHtml;
        Assert.That(pInner, Is.EqualTo("UnsavedWorldDialog.Content.TextTestWorld"));

        var expectedButtonTexts = new[]
            { "UnsavedWorldDialog.Button.Yes", "UnsavedWorldDialog.Button.No", "UnsavedWorldDialog.Button.Cancel", };
        var mudButtons = DialogProvider.FindComponents<MudButton>();
        Assert.That(mudButtons, Has.Count.EqualTo(3));
        for (var index = 0; index < mudButtons.Count; index++)
        {
            var button = mudButtons[index];
            var spanText = button.Find("button span").InnerHtml;
            Assert.That(spanText, Is.EqualTo(expectedButtonTexts[index]));
        }
    }

    [Test]
    public async Task YesButtonPressed_ReturnsPositiveResponse()
    {
        var dialog = await OpenDialogAndGetDialogReferenceAsync();
        DialogProvider.FindComponents<MudButton>()[0].Find("button").Click();
        var result = await dialog.Result;
        Assert.That(result.Canceled, Is.False);
        Assert.That(result.Data, Is.True);
    }

    [Test]
    public async Task NoButtonPressed_ReturnsNegativeResponse()
    {
        var dialog = await OpenDialogAndGetDialogReferenceAsync();
        DialogProvider.FindComponents<MudButton>()[1].Find("button").Click();
        var result = await dialog.Result;
        Assert.That(result.Canceled, Is.False);
        Assert.That(result.Data, Is.False);
    }

    [Test]
    public async Task CancelButtonPressed_ReturnsCancelledResponse()
    {
        var dialog = await OpenDialogAndGetDialogReferenceAsync();
        DialogProvider.FindComponents<MudButton>()[2].Find("button").Click();
        var result = await dialog.Result;
        Assert.That(result.Canceled, Is.True);
        Assert.That(result.Data, Is.EqualTo(null));
    }
}