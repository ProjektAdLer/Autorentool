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

namespace IntegrationTest.Components.Adaptivity.Dialogues;

[TestFixture]
public class AdaptivityContentDialogIt : MudDialogTestFixture<AdaptivityContentDialog>
{
    [SetUp]
    public void Setup()
    {
        PresentationLogic = Substitute.For<IPresentationLogic>();
        Context.Services.AddSingleton(PresentationLogic);
        Mapper = Substitute.For<IMapper>();
        Context.Services.AddSingleton(Mapper);
        FormDataContainer = Substitute.For<IFormDataContainer<AdaptivityContentFormModel, AdaptivityContent>>();
        Context.Services.AddSingleton(FormDataContainer);
        AdaptivityContent = Substitute.For<IAdaptivityContentViewModel>();
        var tasks = new List<IAdaptivityTaskViewModel>();
        AdaptivityContent.Tasks.Returns(tasks);
        PresentationLogic.When(x => x.CreateAdaptivityTask(AdaptivityContent, Arg.Any<string>())).Do(y =>
        {
            var task = Substitute.For<IAdaptivityTaskViewModel>();
            task.Name.Returns(y.ArgAt<string>(1));
            tasks.Add(task);
        });
        PresentationLogic.When(x => x.DeleteAdaptivityTask(AdaptivityContent, Arg.Any<IAdaptivityTaskViewModel>())).Do(
            y => { tasks.Remove(y.ArgAt<IAdaptivityTaskViewModel>(1)); });
    }

    [TearDown]
    public void Teardown()
    {
        DialogProvider.Dispose();
    }

    private IDialogReference Dialog { get; set; } = null!;
    private IAdaptivityContentViewModel AdaptivityContent { get; set; } = null!;
    private IPresentationLogic PresentationLogic { get; set; } = null!;
    private IMapper Mapper { get; set; } = null!;
    private IFormDataContainer<AdaptivityContentFormModel, AdaptivityContent> FormDataContainer { get; set; } = null!;

    private async Task GetDialogAsync()
    {
        var dialogParameters = new DialogParameters
        {
            {nameof(AdaptivityContentDialog.MyContent), AdaptivityContent}
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
}