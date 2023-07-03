using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Dialogues;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningWorld;
using TestHelpers;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components.Dialogues;

[TestFixture]
public class DeleteContentInUseConfirmationDialogUt
{
    private TestContext Context { get; set; }
    private IStringLocalizer<DeleteContentInUseConfirmationDialog> _localizer;

    [SetUp]
    public void Setup()
    {
        Context = new TestContext();
        Context.AddMudBlazorTestServices();
        _localizer = Substitute.For<IStringLocalizer<DeleteContentInUseConfirmationDialog>>();
        _localizer[Arg.Any<string>()].Returns(ci => new LocalizedString(ci.Arg<string>(), ci.Arg<string>()));
        _localizer[Arg.Any<string>(), Arg.Any<object[]>()].Returns(ci =>
            new LocalizedString(ci.Arg<string>() + string.Concat(ci.Arg<object[]>()),
                ci.Arg<string>() + string.Concat(ci.Arg<object[]>())));
        Context.Services.AddSingleton(_localizer);
    }

    [Test]
    public async Task Render_RendersCorrectly()
    {
        var comp = GetDialogProvider();
        var service = (DialogService)Context.Services.GetService<IDialogService>()!;

        var world = ViewModelProvider.GetLearningWorld();
        var element = ViewModelProvider.GetLearningElement();

        IDialogReference? dialogReference = null;
        var matches = new (ILearningWorldViewModel, ILearningElementViewModel)[] { (world, element) };
        var parameters = new DialogParameters
        {
            { nameof(DeleteContentInUseConfirmationDialog.ContentName), "ContentName" },
            { nameof(DeleteContentInUseConfirmationDialog.WorldElementInUseTuples), matches }
        };
        await comp.InvokeAsync(() =>
            dialogReference = service.Show<DeleteContentInUseConfirmationDialog>("foo", parameters));
        Assert.That(dialogReference, Is.Not.Null);

        var mainText = comp.Find("p.main-text");
        var sb = new StringBuilder("Dialog.Text1ContentName");
        sb.AppendLine();
        sb.Append("            Dialog.Text2");
        Assert.That(mainText.TrimmedText(), Is.EqualTo(sb.ToString()));

        var tableRows = comp.FindAll("tbody tr");
        Assert.That(tableRows, Has.Count.EqualTo(1));
        var worldInner = tableRows.First().Children.First(element => element.Attributes["data-label"].Value == "World")
            .InnerHtml;
        Assert.That(worldInner, Is.EqualTo(world.Name));
        var elementInner = tableRows.First().Children.First(element => element.Attributes["data-label"].Value == "Element")
            .InnerHtml;
        Assert.That(elementInner, Is.EqualTo(element.Name));
    }

    [Test]
    public async Task CancelButtonClicked_DialogResultCancelled()
    {
        var comp = GetDialogProvider();
        var service = (DialogService)Context.Services.GetService<IDialogService>()!;
        IDialogReference? dialogReference = null;
        await comp.InvokeAsync(() =>
            dialogReference = service.Show<DeleteContentInUseConfirmationDialog>("foo"));
        Assert.That(dialogReference, Is.Not.Null);
        //cancel
        comp.FindAll("button")[0].Click();
        var result = await dialogReference.Result;
        Assert.That(result.Canceled, Is.True);
    }

    [Test]
    public async Task OkButtonClicked_DialogResultNotCancelled_AndDataTrue()
    {
        var comp = GetDialogProvider();
        var service = (DialogService)Context.Services.GetService<IDialogService>()!;
        IDialogReference? dialogReference = null;
        await comp.InvokeAsync(() =>
            dialogReference = service.Show<DeleteContentInUseConfirmationDialog>("foo"));
        Assert.That(dialogReference, Is.Not.Null);
        //confirm
        comp.FindAll("button")[1].Click();
        var result = await dialogReference.Result;
        Assert.That(result.Canceled, Is.False);
        Assert.That(result.Data, Is.EqualTo(true));
    }

    private IRenderedComponent<MudDialogProvider> GetDialogProvider()
    {
        return Context.RenderComponent<MudDialogProvider>();
    }
}