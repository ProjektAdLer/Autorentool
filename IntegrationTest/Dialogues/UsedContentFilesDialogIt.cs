using System.Collections.Generic;
using System.Threading.Tasks;
using Bunit;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Dialogues;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningWorld;
using PresentationTest;

namespace IntegrationTest.Dialogues;

[TestFixture]
public class UsedContentFilesDialogIt : MudDialogTestFixture<UsedContentFilesDialog>
{
    [Test]
    public async Task DialogCreated_DependenciesInjected()
    {
        await OpenDialogAndGetDialogReferenceAsync();

        var systemUnderTest = DialogProvider.FindComponentOrFail<UsedContentFilesDialog>();

        Assert.That(systemUnderTest.Instance.Localizer, Is.EqualTo(Localizer));
    }

    [Test]
    public async Task DialogCreated_RendersCorrectly()
    {
        var content = Substitute.For<ILearningContentViewModel>();
        var world1 = Substitute.For<ILearningWorldViewModel>();
        world1.Name.Returns("world1");
        var world2 = Substitute.For<ILearningWorldViewModel>();
        world2.Name.Returns("world2");
        var element1 = Substitute.For<ILearningElementViewModel>();
        element1.Name.Returns("element1");
        var element2 = Substitute.For<ILearningElementViewModel>();
        element2.Name.Returns("element2");
        var usages = new List<(ILearningWorldViewModel, ILearningElementViewModel)>
            { (world1, element1), (world2, element2) };
        var dialogParameters = new DialogParameters
        {
            { "LearningContent", content },
            { "Usages", usages }
        };

        await OpenDialogAndGetDialogReferenceAsync(parameters: dialogParameters);

        var systemUnderTest = DialogProvider.FindComponentOrFail<UsedContentFilesDialog>();

        Assert.That(systemUnderTest.Instance.LearningContent, Is.EqualTo(content));
        Assert.That(systemUnderTest.Instance.Usages, Is.EqualTo(usages));

        var tds = systemUnderTest.FindAll("td");
        Assert.That(tds, Has.Count.EqualTo(4));

        var expectedValues = new List<string> { "world1", "element1", "world2", "element2" };
        foreach (var td in tds)
        {
            Assert.That(expectedValues, Contains.Item(td.InnerHtml));
            expectedValues.Remove(td.InnerHtml);
        }

        Assert.That(expectedValues, Is.Empty);
    }
}