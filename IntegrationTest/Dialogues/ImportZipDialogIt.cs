using System.Collections.Generic;
using System.Threading.Tasks;
using Bunit;
using MudBlazor;
using NUnit.Framework;
using Presentation.Components.Dialogues;

namespace IntegrationTest.Dialogues;

[TestFixture]
public class ImportZipDialogIt : MudDialogTestFixture<ImportZipDialog>
{
    [Test]
    public void Render_RendersParametersCorrectly()
    {
        var parameters = new DialogParameters
        {
            {nameof(ImportZipDialog.FileName), "TestFileName"},
            {nameof(ImportZipDialog.SuccessfulFiles), new List<string> {"TestFile1", "TestFile2"}},
            {nameof(ImportZipDialog.DuplicateFiles), new List<string> {"TestFile3", "TestFile4"}},
            {nameof(ImportZipDialog.UnsupportedFiles), new List<string> {"TestFile5", "TestFile6"}},
            {nameof(ImportZipDialog.ErrorFiles), new List<string> {"TestFile7", "TestFile8"}},
        };

        var dialog = OpenDialogAndGetDialogReferenceAsync(parameters: parameters);

        var mudTexts = DialogProvider.FindComponents<MudText>();
        Assert.That(mudTexts, Has.Count.EqualTo(7));
        Assert.Multiple(() =>
        {
            Assert.That(mudTexts[0].Find("h6").InnerHtml, Is.EqualTo("title"));
            Assert.That(mudTexts[1].Find("p").InnerHtml, Is.EqualTo("ImportZipDialog.Heading.File.Name TestFileName"));
            Assert.That(mudTexts[2].Find("p").InnerHtml, Is.EqualTo("ImportZipDialog.Heading.File.Count8"));
            Assert.That(mudTexts[3].Find("p").InnerHtml, Is.EqualTo("ImportZipDialog.Heading.Files.Successful 2"));
            Assert.That(mudTexts[4].Find("p").InnerHtml, Is.EqualTo("ImportZipDialog.Heading.Files.Duplicate 2"));
            Assert.That(mudTexts[5].Find("p").InnerHtml, Is.EqualTo("ImportZipDialog.Heading.Files.Unsupported 2"));
            Assert.That(mudTexts[6].Find("p").InnerHtml, Is.EqualTo("ImportZipDialog.Heading.Files.Error 2"));
        });

        // var mudLists = DialogProvider.FindComponents<MudList>();
        // Assert.That(mudLists, Has.Count.EqualTo(4));
        // Assert.Multiple(() =>
        // {
        //     Assert.That(mudLists[0].Nodes.First().FirstChild!.ToMarkup(), Is.EqualTo("TestFile1"));
        //     Assert.That(mudLists[1].Nodes.First().FirstChild!.ToMarkup(), Is.EqualTo("TestFile3"));
        //     Assert.That(mudLists[2].Nodes.First().FirstChild!.ToMarkup(), Is.EqualTo("TestFile5"));
        //     Assert.That(mudLists[3].Nodes.First().FirstChild!.ToMarkup(), Is.EqualTo("TestFile7"));
        // });
    }

    [Test]
    public async Task OkButtonPressed_ClosesDialog()
    {
        var dialog = await OpenDialogAndGetDialogReferenceAsync();
        Assert.That(DialogProvider.Markup, Is.Not.Empty);
        DialogProvider.FindComponents<MudButton>()[0].Find("button").Click();
        var result = await dialog.Result;
        Assert.Multiple(() =>
        {
            Assert.That(result.Canceled, Is.False);
            Assert.That(DialogProvider.Markup, Is.Empty);
        });
    }
}