using System;
using System.Threading.Tasks;
using AutoMapper;
using Bunit;
using BusinessLogic.Entities.LearningContent;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms.Content;
using Presentation.Components.Forms.Models;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningContent;
using TestHelpers;

namespace IntegrationTest.Forms.Content;

[TestFixture]
public class AddLinkFormIt : MudFormTestFixture<AddLinkForm, LinkContentFormModel, LinkContent>
{
    private IPresentationLogic PresentationLogic { get; set; }
    private IMapper Mapper { get; set; }

    [SetUp]
    public void SetUp()
    {
        PresentationLogic = Substitute.For<IPresentationLogic>();
        Mapper = Substitute.For<IMapper>();
        Context.Services.AddSingleton(PresentationLogic);
        Context.Services.AddSingleton(Mapper);
    }

    [Test]
    public void Render_SetsParametersAndInjectsDependencies()
    {
        var rerenderContentContainer = () => Task.CompletedTask;

        var systemUnderTest = GetRenderedComponent(rerenderContentContainer);

        Assert.That(systemUnderTest.Instance.FormDataContainer, Is.EqualTo(FormDataContainer));
        Assert.That(systemUnderTest.Instance.PresentationLogic, Is.EqualTo(PresentationLogic));
        Assert.That(systemUnderTest.Instance.Mapper, Is.EqualTo(Mapper));
        Assert.That(systemUnderTest.Instance.DebounceInterval, Is.EqualTo(0));
        Assert.That(systemUnderTest.Instance.RerenderContentContainer, Is.EqualTo(rerenderContentContainer));
    }

    [Test]
    public void SubmitButtonClicked_Submits()
    {
        var vm = ViewModelProvider.GetLinkContent();
        var systemUnderTest = GetRenderedComponent();
        Mapper.Map<LinkContentViewModel>(Arg.Any<object>()).Returns(vm);

        var textFields = systemUnderTest.FindComponents<MudTextField<string>>();
        textFields[0].Find("input").Change("name");
        textFields[1].Find("input").Change("url");
        
        var submitButton = systemUnderTest.FindComponent<MudButton>();
        submitButton.Find("button").Click();
        
        PresentationLogic.Received(1).SaveLink(vm);
    }

    private IRenderedComponent<AddLinkForm> GetRenderedComponent(Func<Task>? rerenderContentContainer = null)
    {
        rerenderContentContainer ??= () => Task.CompletedTask;
        return Context.RenderComponent<AddLinkForm>(p =>
        {
            p.Add(c => c.DebounceInterval, 0);
            p.AddCascadingValue("RerenderContentContainer", rerenderContentContainer);
        });
    }
}