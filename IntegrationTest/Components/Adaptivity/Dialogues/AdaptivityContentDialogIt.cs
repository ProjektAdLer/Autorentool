using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bunit;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Adaptivity.Dialogues;
using Presentation.Components.Forms;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;
using Shared.Adaptivity;

namespace IntegrationTest.Components.Adaptivity.Dialogues;

[TestFixture]
public class AdaptivityContentDialogIt : MudDialogTestFixture<AdaptivityContentDialog>
{
    [SetUp]
    public void Setup()
    {
        PresentationLogic = Substitute.For<IPresentationLogic>();
        Context.Services.AddSingleton(PresentationLogic);
        FormDataContainer = Substitute.For<IFormDataContainer<AdaptivityContentFormModel, AdaptivityContent>>();
        FormDataContainer.FormModel.Returns(new AdaptivityContentFormModel());
        Context.Services.AddSingleton(FormDataContainer);
        AdaptivityContent = Substitute.For<IAdaptivityContentViewModel>();
        Tasks = new List<IAdaptivityTaskViewModel>();
        AdaptivityContent.Tasks.Returns(Tasks);
        Mapper = Substitute.For<IMapper>();
        Mapper.When(x => x.Map(Arg.Any<IAdaptivityContentViewModel>(), Arg.Any<AdaptivityContentFormModel>())).Do(y =>
        {
            y.Arg<AdaptivityContentFormModel>().Tasks = Tasks;
        });
        Context.Services.AddSingleton(Mapper);
        PresentationLogic.When(x => x.CreateAdaptivityTask(AdaptivityContent, Arg.Any<string>())).Do(y =>
        {
            var task = Substitute.For<IAdaptivityTaskViewModel>();
            task.Name.Returns(y.ArgAt<string>(1));
            Tasks.Add(task);
        });
        PresentationLogic.When(x => x.DeleteAdaptivityTask(AdaptivityContent, Arg.Any<IAdaptivityTaskViewModel>())).Do(
            y => { Tasks.Remove(y.ArgAt<IAdaptivityTaskViewModel>(1)); });
        Context.ComponentFactories.AddStub<AdaptivityContentDialogRuleControl>();
    }

    [TearDown]
    public void Teardown()
    {
        DialogProvider.Dispose();
    }

    private IDialogReference Dialog { get; set; } = null!;
    private IAdaptivityContentViewModel AdaptivityContent { get; set; } = null!;
    private List<IAdaptivityTaskViewModel> Tasks { get; set; } = null!;
    private IPresentationLogic PresentationLogic { get; set; } = null!;
    private IMapper Mapper { get; set; } = null!;
    private IFormDataContainer<AdaptivityContentFormModel, AdaptivityContent> FormDataContainer { get; set; } = null!;

    private async Task GetDialogAsync()
    {
        var dialogParameters = new DialogParameters
        {
            { nameof(AdaptivityContentDialog.MyContent), AdaptivityContent },
            { nameof(AdaptivityContentDialog.DebounceInterval), 10 }
        };
        Dialog = await OpenDialogAndGetDialogReferenceAsync(options: new DialogOptions(),
            parameters: dialogParameters);
        Mapper.Received(1).Map(AdaptivityContent, FormDataContainer.FormModel);
        Mapper.ClearReceivedCalls();
    }

    [Test]
    public async Task AddTaskButtonClicked_CallsPresentationLogicAndMapper()
    {
        await GetDialogAsync();
        var button = DialogProvider.FindComponent<MudIconButton>().Find("button");
        await button.ClickAsync(new MouseEventArgs());
        PresentationLogic.Received(1).CreateAdaptivityTask(AdaptivityContent, "AdaptivityContentDialog.NewTask.Name1");
        Mapper.Received(1).Map(AdaptivityContent, FormDataContainer.FormModel);
    }

    [Test]
    public async Task DeleteTaskButtonClicked_CallsPresentationLogic()
    {
        await GetDialogAsync();
        var button = DialogProvider.FindComponent<MudIconButton>().Find("button");
        await button.ClickAsync(new MouseEventArgs());
        var buttons = DialogProvider.FindComponents<MudIconButton>();
        var deleteButton = buttons[0].Find("button");
        var task = AdaptivityContent.Tasks.Last();
        await deleteButton.ClickAsync(new MouseEventArgs());
        PresentationLogic.Received(1).DeleteAdaptivityTask(AdaptivityContent, task);
    }

    [Test]
    [Retry(3)]
    public async Task RenameTask_CallsPresentationLogic()
    {
        await GetDialogAsync();
        var button = DialogProvider.FindComponent<MudIconButton>().Find("button");
        await button.ClickAsync(new MouseEventArgs());
        var textFields = DialogProvider.FindComponents<MudTextField<string>>();
        var textField = textFields[0].Find("textarea");
        textField.Input("NewName");
        // Wait for debounce
        await Task.Delay(500);
        DialogProvider.WaitForAssertion(() => PresentationLogic.Received(1)
                .EditAdaptivityTask(Tasks.First(), "NewName", Tasks.First().MinimumRequiredDifficulty),
            TimeSpan.FromSeconds(2));
        PresentationLogic.ClearReceivedCalls();

        textField.Input("");
        // Wait for debounce
        await Task.Delay(500);
        PresentationLogic.DidNotReceiveWithAnyArgs()
            .EditAdaptivityTask(Arg.Any<IAdaptivityTaskViewModel>(), Arg.Any<string>(), Arg.Any<QuestionDifficulty?>());
    }

    [Test]
    public async Task ChangeRequiredDifficulty_CallsPresentationLogic([Values] bool wasSelectedAsRequired)
    {
        var task = Substitute.For<IAdaptivityTaskViewModel>();
        var question = Substitute.For<IAdaptivityQuestionViewModel>();
        question.Difficulty.Returns(QuestionDifficulty.Medium);
        task.Questions.Returns(new List<IAdaptivityQuestionViewModel> { question });
        task.MinimumRequiredDifficulty.Returns(wasSelectedAsRequired
            ? QuestionDifficulty.Medium
            : null);
        AdaptivityContent.Tasks.Add(task);
        await GetDialogAsync();
        var iconButtons = DialogProvider.FindComponents<MudIconButton>();
        var keyButton = iconButtons[3].Find("button");
        await keyButton.ClickAsync(new MouseEventArgs());
        PresentationLogic.Received(1)
            .EditAdaptivityTask(task, task.Name, wasSelectedAsRequired ? null : QuestionDifficulty.Medium);
    }

    [Test]
    public async Task DeleteQuestionButtonClicked_CallsPresentationLogic()
    {
        var task = Substitute.For<IAdaptivityTaskViewModel>();
        var question = Substitute.For<IAdaptivityQuestionViewModel>();
        question.Difficulty.Returns(QuestionDifficulty.Medium);
        task.Questions.Returns(new List<IAdaptivityQuestionViewModel> { question });
        AdaptivityContent.Tasks.Add(task);
        await GetDialogAsync();
        var iconButtons = DialogProvider.FindComponents<MudIconButton>();
        var deleteButton = iconButtons[2].Find("button");
        await deleteButton.ClickAsync(new MouseEventArgs());
        PresentationLogic.Received(1).DeleteAdaptivityQuestion(task, question);
    }
}