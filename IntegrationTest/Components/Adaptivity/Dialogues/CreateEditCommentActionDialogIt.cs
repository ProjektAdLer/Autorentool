using System.Threading.Tasks;
using Bunit;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Adaptivity.Dialogues;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Action;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Trigger;
using Shared.Adaptivity;
using TestHelpers;

namespace IntegrationTest.Components.Adaptivity.Dialogues;

[TestFixture]
public class CreateEditCommentActionDialogIt : MudDialogTestFixture<CreateEditCommentActionDialog>
{
    [SetUp]
    public new async Task Setup()
    {
        ExistingAction = null;
        PresentationLogic = Substitute.For<IPresentationLogic>();
        Context.Services.AddSingleton(PresentationLogic);
        Question = Substitute.For<IAdaptivityQuestionViewModel>();
        await GetDialogAsync();
    }

    [TearDown]
    public void Teardown()
    {
        DialogProvider.Dispose();
    }

    private IDialogReference Dialog { get; set; }
    private CommentActionViewModel? ExistingAction { get; set; }
    private IAdaptivityQuestionViewModel Question { get; set; }
    private IPresentationLogic PresentationLogic { get; set; }

    private async Task GetDialogAsync()
    {
        var dialogParameters = new DialogParameters
        {
            { nameof(CreateEditCommentActionDialog.Question), Question },
        };
        if (ExistingAction != null)
            dialogParameters.Add(nameof(CreateEditCommentActionDialog.ExistingAction), ExistingAction);
        Dialog = await OpenDialogAndGetDialogReferenceAsync("title", new DialogOptions(),
            dialogParameters);
    }

    [Test]
    // ANF-ID: [AWA0031]
    public async Task NoExistingAction_TextSet_CallsCreateAdaptivityRule()
    {
        var textField = DialogProvider.FindComponent<MudTextField<string>>();
        textField.Find("textarea").Change("foo");

        await DialogProvider.FindComponent<MudButton>().Find("button").ClickAsync(new MouseEventArgs());

        PresentationLogic.Received(1).CreateAdaptivityRule(Question,
            Arg.Is<CorrectnessTriggerViewModel>(arg => arg.ExpectedAnswer == AnswerResult.Incorrect),
            Arg.Is<CommentActionViewModel>(arg => arg.Comment == "foo"));
    }

    [Test]
    // ANF-ID: [AWA0032]
    public async Task ExistingAction_TextSet_CallsEditCommentAction()
    {
        ExistingAction = ViewModelProvider.GetCommentAction();
        await GetDialogAsync();

        var textField = DialogProvider.FindComponent<MudTextField<string>>();
        textField.Find("textarea").Change("foo");

        await DialogProvider.FindComponent<MudButton>().Find("button").ClickAsync(new MouseEventArgs());

        PresentationLogic.Received(1).EditCommentAction(ExistingAction, "foo");
    }

    [Test]
    // ANF-ID: [AWA0032]
    public async Task ExistingAction_SameTextSet_DoesNotCallEditCommentAction()
    {
        ExistingAction = ViewModelProvider.GetCommentAction();
        ExistingAction.Comment = "foo";
        await GetDialogAsync();

        var textField = DialogProvider.FindComponent<MudTextField<string>>();
        textField.Find("textarea").Change(ExistingAction.Comment);

        await DialogProvider.FindComponent<MudButton>().Find("button").ClickAsync(new MouseEventArgs());

        PresentationLogic.DidNotReceive().EditCommentAction(Arg.Any<CommentActionViewModel>(), Arg.Any<string>());
    }
}