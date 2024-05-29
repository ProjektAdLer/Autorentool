using System;
using System.Linq;
using System.Threading.Tasks;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Adaptivity.Dialogues;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Action;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Trigger;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningWorld;
using Shared.Adaptivity;
using TestHelpers;

namespace IntegrationTest.Components.Adaptivity.Dialogues;

[TestFixture]
public class CreateEditReferenceActionDialogIt : MudDialogTestFixture<CreateEditReferenceActionDialog>
{
    [SetUp]
    public async Task Setup()
    {
        ExistingAction = null;
        PresentationLogic = Substitute.For<IPresentationLogic>();
        LearningWorldPresenter = Substitute.For<ILearningWorldPresenter>();
        Contents = new ILearningContentViewModel[] { ViewModelProvider.GetFileContent() };
        PresentationLogic.GetAllContent().Returns(Contents);
        World = Substitute.For<ILearningWorldViewModel>();
        var element = Substitute.For<ILearningElementViewModel>();
        element.Id.Returns(Guid.NewGuid());
        World.AllLearningElements.Returns(new[] { element });
        LearningWorldPresenter.LearningWorldVm.Returns(World);
        Context.Services.AddSingleton(PresentationLogic);
        Context.Services.AddSingleton(LearningWorldPresenter);
        Question = Substitute.For<IAdaptivityQuestionViewModel>();
        await GetDialogAsync();
    }

    [TearDown]
    public void Teardown()
    {
        DialogProvider.Dispose();
    }

    private IDialogReference Dialog { get; set; }
    private IAdaptivityActionViewModel? ExistingAction { get; set; }
    private IAdaptivityQuestionViewModel Question { get; set; }
    private IPresentationLogic PresentationLogic { get; set; }
    private ILearningWorldPresenter LearningWorldPresenter { get; set; }
    private ILearningWorldViewModel World { get; set; }
    private ILearningContentViewModel[] Contents { get; set; }

    private async Task GetDialogAsync()
    {
        var dialogParameters = new DialogParameters
        {
            { nameof(CreateEditReferenceActionDialog.Question), Question },
        };
        if (ExistingAction != null)
            dialogParameters.Add(nameof(CreateEditReferenceActionDialog.ExistingAction), ExistingAction);
        Dialog = await OpenDialogAndGetDialogReferenceAsync("title", new DialogOptions(),
            dialogParameters);
    }

    [Test]
    // ANF-ID: [AWA0026]
    public async Task NoExistingAction_ContentSelected_CallsCreateAdaptivityRuleWithContentReferenceAction()
    {
        await DialogProvider.Find(".tab-panel-content").ClickAsync(new MouseEventArgs());
        await DialogProvider.Find("div.mud-paper").ClickAsync(new MouseEventArgs());
        await DialogProvider.FindComponent<MudTextField<string>>().Find("textarea")
            .ChangeAsync(new ChangeEventArgs { Value = "foo" });

        await DialogProvider.FindComponent<MudButton>().Find("button").ClickAsync(new MouseEventArgs());

        PresentationLogic.Received().CreateAdaptivityRule(
            Question,
            Arg.Is<CorrectnessTriggerViewModel>(arg => arg.ExpectedAnswer == AnswerResult.Incorrect),
            Arg.Is<ContentReferenceActionViewModel>(arg => arg.Content == Contents[0] && arg.Comment == "foo")
        );
    }

    [Test]
    // ANF-ID: [AWA0026]
    public async Task NoExistingAction_ElementSelected_CallsCreateAdaptivityRuleWithElementReferenceAction()
    {
        await DialogProvider.Find("div.mud-tab.tab-panel-element").ClickAsync(new MouseEventArgs());
        await DialogProvider.Find("div.mud-paper").ClickAsync(new MouseEventArgs());

        await DialogProvider.FindComponent<MudButton>().Find("button").ClickAsync(new MouseEventArgs());

        PresentationLogic.Received().CreateAdaptivityRule(
            Question,
            Arg.Is<CorrectnessTriggerViewModel>(arg => arg.ExpectedAnswer == AnswerResult.Incorrect),
            Arg.Is<ElementReferenceActionViewModel>(arg => arg.ElementId == World.AllLearningElements.First().Id)
        );
    }

    [Test]
    // ANF-ID: [AWA0027]
    public async Task ExistingAction_ContentSelected_CallsUpdateContentReferenceAction()
    {
        var cravm = ViewModelProvider.GetContentReferenceAction();
        cravm.Content = Contents.First();
        ExistingAction = cravm;
        await GetDialogAsync();

        await DialogProvider.Find(".tab-panel-content").ClickAsync(new MouseEventArgs());
        await DialogProvider.FindComponent<MudTextField<string>>().Find("textarea")
            .ChangeAsync(new ChangeEventArgs { Value = "foo" });

        await DialogProvider.FindComponent<MudButton>().Find("button").ClickAsync(new MouseEventArgs());

        PresentationLogic.Received().EditContentReferenceAction(cravm, cravm.Content, "foo");
    }

    [Test]
    // ANF-ID: [AWA0027]
    public async Task ExistingAction_ElementSelected_CallsUpdateElementReferenceAction()
    {
        var eravm = ViewModelProvider.GetElementReferenceAction();
        eravm.ElementId = World.AllLearningElements.First().Id;
        ExistingAction = eravm;
        await GetDialogAsync();

        await DialogProvider.FindComponent<MudTextField<string>>().Find("textarea")
            .ChangeAsync(new ChangeEventArgs { Value = "foo" });

        await DialogProvider.FindComponent<MudButton>().Find("button").ClickAsync(new MouseEventArgs());

        PresentationLogic.Received().EditElementReferenceAction(eravm, eravm.ElementId, "foo");
    }

    [Test]
    // ANF-ID: [AWA0027]
    public async Task ExistingAction_ElementSelected_NoChange_CallsNothing()
    {
        var cravm = ViewModelProvider.GetContentReferenceAction();
        cravm.Content = Contents.First();
        ExistingAction = cravm;
        await GetDialogAsync();

        await DialogProvider.FindComponent<MudButton>().Find("button").ClickAsync(new MouseEventArgs());

        PresentationLogic.DidNotReceiveWithAnyArgs().EditContentReferenceAction(cravm, cravm.Content, "foo");
    }

    [Test]
    // ANF-ID: [AWA0027]
    public async Task ExistingAction_ContentSelected_NoChange_CallsNothing()
    {
        var eravm = ViewModelProvider.GetElementReferenceAction();
        eravm.ElementId = World.AllLearningElements.First().Id;
        ExistingAction = eravm;
        await GetDialogAsync();

        await DialogProvider.FindComponent<MudButton>().Find("button").ClickAsync(new MouseEventArgs());

        PresentationLogic.DidNotReceiveWithAnyArgs().EditElementReferenceAction(eravm, eravm.ElementId, "foo");
    }
}