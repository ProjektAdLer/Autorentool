using System.Threading.Tasks;
using Bunit;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using NUnit.Framework;
using Presentation.Components.Dialogues;

namespace IntegrationTest.Dialogues;

[TestFixture]
public class GenericCancellationConfirmationDialogIt : MudDialogTestFixture<GenericCancellationConfirmationDialog>
{
    [Test]
    public async Task Render_WithDialogText_RendersParametersCorrectly()
    {
        var parameters = new DialogParameters
        {
            { nameof(GenericCancellationConfirmationDialog.DialogText), "This is the text of the dialog" },
            { nameof(GenericCancellationConfirmationDialog.SubmitButtonText), "Subshmit" },
            { nameof(GenericCancellationConfirmationDialog.SubmitButtonColor), Color.Info },
        };

        await OpenDialogAndGetDialogReferenceAsync(parameters: parameters);

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
    public async Task Render_WithChildContent_RendersParametersCorrectly()
    {
        RenderFragment childContent = builder =>
        {
            builder.OpenElement(0, "p");
            builder.AddAttribute(1, "class", "foobar");
            builder.AddContent(2, "This is the child content");
            builder.CloseElement();
        };
        var parameters = new DialogParameters
        {
            { nameof(GenericCancellationConfirmationDialog.DialogText), "" },
            { nameof(GenericCancellationConfirmationDialog.ChildContent), childContent },
            { nameof(GenericCancellationConfirmationDialog.SubmitButtonText), "Subshmit" },
            { nameof(GenericCancellationConfirmationDialog.SubmitButtonColor), Color.Info },
        };

        await OpenDialogAndGetDialogReferenceAsync(parameters: parameters);

        var mudTexts = DialogProvider.FindComponents<MudText>();
        Assert.That(mudTexts, Has.Count.EqualTo(1));
        var p = DialogProvider.Find("p.foobar");

        var pInner = p.InnerHtml;
        Assert.That(pInner, Is.EqualTo("This is the child content"));

        var buttons = DialogProvider.FindComponents<MudButton>();
        Assert.That(buttons, Has.Count.EqualTo(2));
        var buttonText = buttons[0].Find("button span").InnerHtml;
        Assert.That(buttonText, Is.EqualTo("GenericCancellationConfirmationDialog.Button.Cancel.Text"));
        //button class tests colour
        buttonText = buttons[1].Find("button.mud-button-text-info span").InnerHtml;
        Assert.That(buttonText, Is.EqualTo("Subshmit"));
    }

    [Test]
    public async Task Render_WithChildContentAndDialogText_RendersParametersCorrectly()
    {
        RenderFragment childContent = builder =>
        {
            builder.OpenElement(0, "p");
            builder.AddAttribute(1, "class", "foobar");
            builder.AddContent(2, "This is the child content");
            builder.CloseElement();
        };
        var parameters = new DialogParameters
        {
            { nameof(GenericCancellationConfirmationDialog.DialogText), "This is the text of the dialog" },
            { nameof(GenericCancellationConfirmationDialog.ChildContent), childContent },
            { nameof(GenericCancellationConfirmationDialog.SubmitButtonText), "Subshmit" },
            { nameof(GenericCancellationConfirmationDialog.SubmitButtonColor), Color.Info },
        };

        await OpenDialogAndGetDialogReferenceAsync(parameters: parameters);

        var mudText = DialogProvider.FindComponents<MudText>()[1];
        var pInner = mudText.Find("p").InnerHtml;
        Assert.That(pInner, Is.EqualTo("This is the text of the dialog"));
        var p = DialogProvider.Find("p.foobar");
        pInner = p.InnerHtml;
        Assert.That(pInner, Is.EqualTo("This is the child content"));

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