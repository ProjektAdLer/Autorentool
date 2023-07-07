using System.ComponentModel;
using AutoMapper;
using Bunit;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms;
using Presentation.Components.Forms.Models;
using Presentation.Components.Forms.World;
using Presentation.PresentationLogic.LearningWorld;
using PresentationTest;
using TestHelpers;

namespace IntegrationTest.Forms.World;

[TestFixture]
public class EditWorldFormIt : MudFormTestFixture<EditWorldForm, LearningWorldFormModel, LearningWorld>
{
    private ILearningWorldPresenter WorldPresenter { get; set; }
    private IMapper Mapper { get; set; }

    [SetUp]
    public void Setup()
    {
        WorldPresenter = Substitute.For<ILearningWorldPresenter>();
        Mapper = Substitute.For<IMapper>();
        Context.Services.AddSingleton(WorldPresenter);
        Context.Services.AddSingleton(Mapper);
    }


    [Test]
    public void Render_SetsParameters()
    {
        var vm = ViewModelProvider.GetLearningWorld();
        var onNewClicked = EventCallback.Factory.Create(this, () => { });

        var systemUnderTest = GetRenderedComponent(vm, onNewClicked);

        Assert.That(systemUnderTest.Instance.WorldToEdit, Is.EqualTo(vm));
        Assert.That(systemUnderTest.Instance.OnNewButtonClicked, Is.EqualTo(onNewClicked));
        Assert.That(systemUnderTest.Instance.DebounceInterval, Is.EqualTo(0));
    }

    [Test]
    public void OnParametersSet_CallsMapper()
    {
        var vm = ViewModelProvider.GetLearningWorld();
        
        var systemUnderTest = GetRenderedComponent(vm);
        
        Mapper.Received(1).Map(vm, FormDataContainer.FormModel);
    }

    [Test]
    public void WorldPresenterLearningWorld_PropertyChanged_RemapsIntoContainer()
    {
        var vm = ViewModelProvider.GetLearningWorld();
        WorldPresenter.LearningWorldVm.Returns(vm);
        
        var systemUnderTest = GetRenderedComponent(vm);
        
        Mapper.Received(1).Map(vm, FormDataContainer.FormModel);

        Assert.That(vm.Name, Is.Not.EqualTo("foobar"));
        vm.Name = "foobar";
        
        Mapper.Received(2).Map(vm, FormDataContainer.FormModel);
    }

    [Test]
    public void ResetButton_Clicked_RemapsIntoContainer()
    {
        var vm = ViewModelProvider.GetLearningWorld();
        WorldPresenter.LearningWorldVm.Returns(vm);
        
        var systemUnderTest = GetRenderedComponent(vm);
        
        Mapper.Received(1).Map(vm, FormDataContainer.FormModel);
        
        systemUnderTest.FindComponentWithMarkup<MudIconButton>("reset-form").Find("button").Click();
        
        Mapper.Received(2).Map(vm, FormDataContainer.FormModel);
    }

    private IRenderedComponent<EditWorldForm> GetRenderedComponent(ILearningWorldViewModel? worldToEdit = null,
        EventCallback? onNewClicked = null)
    {
        worldToEdit ??= ViewModelProvider.GetLearningWorld();
        onNewClicked ??= EventCallback.Empty;
        return Context.RenderComponent<EditWorldForm>(parameters =>
        {
            parameters.Add(c => c.WorldToEdit, worldToEdit);
            parameters.Add(c => c.OnNewButtonClicked, onNewClicked.Value);
            parameters.Add(c => c.DebounceInterval, 0);
        });
    }
}