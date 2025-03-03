using System;
using System.Linq;
using System.Threading.Tasks;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Adaptivity.Dialogues;
using Presentation.Components.Forms;
using Presentation.Components.Forms.Element;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Action;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Trigger;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.SelectedViewModels;
using Presentation.View.LearningElement;
using Shared.Adaptivity;
using TestHelpers;

namespace IntegrationTest.Components.Adaptivity.Dialogues;

[TestFixture]
public class CreateEditReferenceActionDialogIt : MudDialogTestFixture<CreateEditReferenceActionDialog>
{
    [SetUp]
    public new async Task Setup()
    {
        ExistingAction = null;
        ExistingRule = Substitute.For<IAdaptivityRuleViewModel>();
        PresentationLogic = Substitute.For<IPresentationLogic>();
        LearningWorldPresenter = Substitute.For<ILearningWorldPresenter>();
        Contents = new ILearningContentViewModel[] { ViewModelProvider.GetFileContent(type: "png") };
        PresentationLogic.GetAllContent().Returns(Contents);
        World = Substitute.For<ILearningWorldViewModel>();
        var element = Substitute.For<ILearningElementViewModel>();
        element.Id.Returns(Guid.NewGuid());
        element.LearningContent.Returns(Contents.First());
        World.AllLearningElements.Returns(new[] { element });
        LearningWorldPresenter.LearningWorldVm.Returns(World);
        Context.Services.AddSingleton(PresentationLogic);
        Context.Services.AddSingleton(LearningWorldPresenter);
        Question = Substitute.For<IAdaptivityQuestionViewModel>();
        _selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        _elementModelHandler = Substitute.For<IElementModelHandler>();
        _stringLocalizer = Substitute.For<IStringLocalizer<DragDropLearningElement>>();
        _stringLocalizer[Arg.Any<string>()]
            .Returns(cinfo => new LocalizedString(cinfo.Arg<string>(), cinfo.Arg<string>()));
        Context.Services.AddSingleton(_stringLocalizer);
        Context.Services.AddSingleton(_selectedViewModelsProvider);
        Context.Services.AddSingleton(_elementModelHandler);
        Context.ComponentFactories.AddStub<TableSelect<ILearningContentViewModel>>();
        Context.RenderComponent<MudPopoverProvider>();
        await GetDialogAsync();
    }

    [TearDown]
    public void Teardown()
    {
        DialogProvider.Dispose();
    }

    private IDialogReference Dialog { get; set; }
    private IAdaptivityActionViewModel? ExistingAction { get; set; }
    private IAdaptivityRuleViewModel? ExistingRule { get; set; }
    private IAdaptivityQuestionViewModel Question { get; set; }
    private IPresentationLogic PresentationLogic { get; set; }
    private ILearningWorldPresenter LearningWorldPresenter { get; set; }
    private ILearningWorldViewModel World { get; set; }
    private ILearningContentViewModel[] Contents { get; set; }
    
    private ISelectedViewModelsProvider _selectedViewModelsProvider;
    private IElementModelHandler _elementModelHandler;
    private IStringLocalizer<DragDropLearningElement> _stringLocalizer;

    private async Task GetDialogAsync()
    {
        var dialogParameters = new DialogParameters
        {
            { nameof(CreateEditReferenceActionDialog.Question), Question },
        };
        if (ExistingAction != null)
        {
            dialogParameters.Add(nameof(CreateEditReferenceActionDialog.ExistingAction), ExistingAction);
            //dialogParameters.Add(nameof(CreateEditReferenceActionDialog.ExistingRule), ExistingRule);
        }
        Dialog = await OpenDialogAndGetDialogReferenceAsync("title", new DialogOptions(),
            dialogParameters);
    }

    [Test]
    // ANF-ID: [AWA0026]
    public async Task NoExistingAction_ContentSelected_CallsCreateAdaptivityRuleWithContentReferenceAction()
    {
        await DialogProvider.Find(".tab-panel-content").ClickAsync(new MouseEventArgs());
        
        var componentUnderTest = DialogProvider.FindComponent<CreateEditReferenceActionDialog>();
        componentUnderTest.Instance.LearningContent = Contents[0];
        componentUnderTest.Instance.Comment = "foo";


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
        ExistingRule = Substitute.For<IAdaptivityRuleViewModel>();
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
        ExistingRule = Substitute.For<IAdaptivityRuleViewModel>();
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
        ExistingRule = Substitute.For<IAdaptivityRuleViewModel>();
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
        ExistingRule = Substitute.For<IAdaptivityRuleViewModel>();
        await GetDialogAsync();

        await DialogProvider.FindComponent<MudButton>().Find("button").ClickAsync(new MouseEventArgs());

        PresentationLogic.DidNotReceiveWithAnyArgs().EditElementReferenceAction(eravm, eravm.ElementId, "foo");
    }
    
    [Test]
    // ANF-ID: [AWA0027]
    public async Task ExistingAction_ElementSelected_CallsReplaceElementReferenceActionByContentReferenceAction()
    {
        var eravm = ViewModelProvider.GetElementReferenceAction();
        eravm.ElementId = World.AllLearningElements.First().Id;
        ExistingAction = eravm;
        ExistingRule = Substitute.For<IAdaptivityRuleViewModel>();
        await GetDialogAsync();

        await DialogProvider.FindComponent<MudTextField<string>>().Find("textarea")
            .ChangeAsync(new ChangeEventArgs { Value = "foo" });
        
        await DialogProvider.Find(".tab-panel-content").ClickAsync(new MouseEventArgs());
        
        var componentUnderTest = DialogProvider.FindComponent<CreateEditReferenceActionDialog>();
        componentUnderTest.Instance.LearningContent = Contents[0];

        await DialogProvider.FindComponent<MudButton>().Find("button").ClickAsync(new MouseEventArgs());

        PresentationLogic.Received().ReplaceElementReferenceActionByContentReferenceAction(
            Arg.Any<IAdaptivityQuestionViewModel>(),
            Arg.Any<IAdaptivityRuleViewModel>(),
            Arg.Any<ContentReferenceActionViewModel>(),
            Arg.Any<CorrectnessTriggerViewModel>());
    }
    
    [Test]
    // ANF-ID: [AWA0027]
    public async Task ExistingAction_ContentSelected_CallsReplaceContentReferenceActionByElementReferenceAction()
    {
        var cravm = ViewModelProvider.GetContentReferenceAction();
        cravm.Content = Contents.First();
        ExistingAction = cravm;
        ExistingRule = Substitute.For<IAdaptivityRuleViewModel>();
        await GetDialogAsync();

        await DialogProvider.FindComponent<MudTextField<string>>().Find("textarea")
            .ChangeAsync(new ChangeEventArgs { Value = "foo" });
        
        await DialogProvider.Find(".tab-panel-element").ClickAsync(new MouseEventArgs());
        
        await DialogProvider.Find("div.mud-paper").ClickAsync(new MouseEventArgs());

        await DialogProvider.FindComponent<MudButton>().Find("button").ClickAsync(new MouseEventArgs());

        PresentationLogic.Received().ReplaceContentReferenceActionByElementReferenceAction(
            Arg.Any<IAdaptivityQuestionViewModel>(),
            Arg.Any<IAdaptivityRuleViewModel>(),
            Arg.Any<ElementReferenceActionViewModel>(),
            Arg.Any<CorrectnessTriggerViewModel>());
    }
}