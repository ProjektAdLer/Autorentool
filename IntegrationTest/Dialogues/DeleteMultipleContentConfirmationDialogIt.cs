using System.Linq;
using System.Threading.Tasks;
using Bunit;
using MudBlazor;
using NUnit.Framework;
using Presentation.Components.Dialogues;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningWorld;
using TestHelpers;

namespace IntegrationTest.Dialogues;

[TestFixture]
public class DeleteMultipleContentConfirmationDialogIt : MudDialogTestFixture<DeleteMultipleContentConfirmationDialog>
{
    [Test]
    public async Task Render_RendersCorrectly()
    {
        var content = ViewModelProvider.GetFileContent();
        var world = ViewModelProvider.GetLearningWorld();
        var element = ViewModelProvider.GetLearningElement();

        var matches = new (ILearningContentViewModel, ILearningWorldViewModel, ILearningElementViewModel)[]
            { (content, world, element) };
        var parameters = new DialogParameters
        {
            { nameof(DeleteMultipleContentConfirmationDialog.ContentWorldElementInUseList), matches },
        };

        _ = await OpenDialogAndGetDialogReferenceAsync(parameters: parameters);

        var mudTexts = DialogProvider.FindComponents<MudText>();
        var pInner = mudTexts[1].Find("p").InnerHtml;
        Assert.That(pInner, Is.EqualTo("Dialog.Text1<br>\n            Dialog.Text2"));

        var tableRows = DialogProvider.FindAll("tbody tr");
        Assert.That(tableRows, Has.Count.EqualTo(1));
        var contentInner = tableRows.First().Children
            .First(ele => ele.Attributes["data-label"]!.Value == "Content")
            .InnerHtml;
        Assert.That(contentInner, Is.EqualTo(content.Name));
        var worldInner = tableRows.First().Children.First(ele => ele.Attributes["data-label"]!.Value == "World")
            .InnerHtml;
        Assert.That(worldInner, Is.EqualTo(world.Name));
        var elementInner = tableRows.First().Children
            .First(ele => ele.Attributes["data-label"]!.Value == "Element")
            .InnerHtml;
        Assert.That(elementInner, Is.EqualTo(element.Name));
    }

    [Test]
    public async Task CancelButtonClicked_DialogResultCancelled()
    {
        var dialog = await OpenDialogAndGetDialogReferenceAsync();

        var buttons = DialogProvider.FindComponents<MudButton>();
        buttons[2].Find("button").Click();

        var result = await dialog.Result;
        Assert.That(result!.Data, Is.Null);
        Assert.That(result.Canceled, Is.True);
    }

    [Test]
    public async Task DeleteUnusedButtonClicked_DialogResultDataIsTrue()
    {
        var dialog = await OpenDialogAndGetDialogReferenceAsync();

        var buttons = DialogProvider.FindComponents<MudButton>();
        buttons[1].Find("button").Click();

        var result = await dialog.Result;
        Assert.That(result!.Data, Is.EqualTo(false));
        Assert.That(result.Canceled, Is.False);
    }

    [Test]
    public async Task DeleteAllButtonClicked_DialogResultDataIsTrue()
    {
        var dialog = await OpenDialogAndGetDialogReferenceAsync();

        var buttons = DialogProvider.FindComponents<MudButton>();
        buttons[0].Find("button").Click();

        var result = await dialog.Result;
        Assert.That(result!.Data, Is.EqualTo(true));
        Assert.That(result.Canceled, Is.False);
    }
}