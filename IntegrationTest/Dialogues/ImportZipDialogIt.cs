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
    // ANF-ID: [AWA0047]
    public void Render_RendersParametersCorrectly()
    {
        var parameters = new DialogParameters
        {
            { nameof(ImportZipDialog.FileName), "TestFileName" },
            { nameof(ImportZipDialog.SuccessfulFiles), new List<string> { "TestFile1", "TestFile2" } },
            { nameof(ImportZipDialog.DuplicateFiles), new List<string> { "TestFile3", "TestFile4" } },
            { nameof(ImportZipDialog.UnsupportedFiles), new List<string> { "TestFile5", "TestFile6" } },
            { nameof(ImportZipDialog.ErrorFiles), new List<string> { "TestFile7", "TestFile8" } },
        };

        var dialog = OpenDialogAndGetDialogReferenceAsync(parameters: parameters);

        var title = DialogProvider.Find("h6");
        Assert.That(title.InnerHtml, Is.EqualTo("title"));
        var mudHeadings = DialogProvider.FindAll("p.mud-typography-body1");
        Assert.That(mudHeadings, Has.Count.EqualTo(6));
        Assert.Multiple(() =>
        {
            Assert.That(mudHeadings[0].InnerHtml, Is.EqualTo("ImportZipDialog.Heading.File.Name TestFileName"));
            Assert.That(mudHeadings[1].InnerHtml, Is.EqualTo("ImportZipDialog.Heading.File.Count8"));
            Assert.That(mudHeadings[2].InnerHtml, Is.EqualTo("ImportZipDialog.Heading.Files.Successful 2"));
            Assert.That(mudHeadings[3].InnerHtml, Is.EqualTo("ImportZipDialog.Heading.Files.Duplicate 2"));
            Assert.That(mudHeadings[4].InnerHtml, Is.EqualTo("ImportZipDialog.Heading.Files.Unsupported 2"));
            Assert.That(mudHeadings[5].InnerHtml, Is.EqualTo("ImportZipDialog.Heading.Files.Error 2"));
        });

        var mudLists = DialogProvider.FindComponents<MudList<string>>();
        Assert.That(mudLists, Has.Count.EqualTo(4));
        var listSuccessful = mudLists[0].FindComponents<MudListItem<string>>();
        var listDuplicate = mudLists[1].FindComponents<MudListItem<string>>();
        var listUnsupported = mudLists[2].FindComponents<MudListItem<string>>();
        var listError = mudLists[3].FindComponents<MudListItem<string>>();
        Assert.Multiple(() =>
        {
            Assert.That(listSuccessful, Has.Count.EqualTo(2));
            Assert.That(listDuplicate, Has.Count.EqualTo(2));
            Assert.That(listUnsupported, Has.Count.EqualTo(2));
            Assert.That(listError, Has.Count.EqualTo(2));
        });
        Assert.Multiple(() =>
        {
            Assert.That(listSuccessful[0].Find("p").InnerHtml, Is.EqualTo("TestFile1"));
            Assert.That(listSuccessful[1].Find("p").InnerHtml, Is.EqualTo("TestFile2"));
            Assert.That(listDuplicate[0].Find("p").InnerHtml, Is.EqualTo("TestFile3"));
            Assert.That(listDuplicate[1].Find("p").InnerHtml, Is.EqualTo("TestFile4"));
            Assert.That(listUnsupported[0].Find("p").InnerHtml, Is.EqualTo("TestFile5"));
            Assert.That(listUnsupported[1].Find("p").InnerHtml, Is.EqualTo("TestFile6"));
            Assert.That(listError[0].Find("p").InnerHtml, Is.EqualTo("TestFile7"));
            Assert.That(listError[1].Find("p").InnerHtml, Is.EqualTo("TestFile8"));
        });
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