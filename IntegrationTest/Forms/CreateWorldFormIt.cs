using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bunit;
using BusinessLogic.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms;
using Presentation.Components.Forms.Models;
using Presentation.Components.Forms.World;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using TestHelpers;

namespace IntegrationTest.Forms;

[TestFixture]
public sealed class CreateWorldFormIt : MudFormTestFixture<CreateWorldForm, LearningWorldFormModel, LearningWorld>
{
    private IAuthoringToolWorkspacePresenter WorkspacePresenter { get; set; }
    private IAuthoringToolWorkspaceViewModel WorkspaceViewModel { get; set; }
    private IFormDataContainer<LearningWorldFormModel, LearningWorld> FormDataContainer { get; set; }
    private LearningWorldFormModel FormModel { get; set; }
    private LearningWorld Entity { get; set; }

    [SetUp]
    public void Setup()
    {
        WorkspacePresenter = Substitute.For<IAuthoringToolWorkspacePresenter>();
        WorkspaceViewModel = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        FormModel = FormModelProvider.GetLearningWorld();
        Entity = EntityProvider.GetLearningWorld();
        FormDataContainer = Substitute.For<IFormDataContainer<LearningWorldFormModel, LearningWorld>>();
        FormDataContainer.FormModel.Returns(FormModel);
        FormDataContainer.GetMappedEntity().Returns(Entity);
        Context.Services.AddSingleton(WorkspacePresenter);
        Context.Services.AddSingleton(WorkspaceViewModel);
        Context.Services.AddSingleton(FormDataContainer);
    }


    [Test]
    public void Render_InjectsDependencies()
    {
        var systemUnderTest = GetRenderedComponent();

        Assert.That(systemUnderTest.Instance.Localizer, Is.Not.Null);
        Assert.That(systemUnderTest.Instance.AuthoringToolWorkspacePresenter, Is.EqualTo(WorkspacePresenter));
        Assert.That(systemUnderTest.Instance.AuthoringToolWorkspaceViewModel, Is.EqualTo(WorkspaceViewModel));
        Assert.That(systemUnderTest.Instance.FormDataContainer, Is.EqualTo(FormDataContainer));
    }

    [Test]
    public async Task ChangeFieldValues_ChangesContainerValues()
    {
        var systemUnderTest = GetRenderedComponent();
        var mudForm = systemUnderTest.FindComponent<MudForm>();
        
        systemUnderTest.FindComponents<Collapsable>()[1].Find("div.toggler").Click();
        await systemUnderTest.InvokeAsync(() => systemUnderTest.Render());
        
        const string expected = "test";
        // Validator.ValidateAsync(Entity, Arg.Any<string>()).Returns(ci =>
        //     (string)FormModel.GetType().GetProperty(ci.Arg<string>()).GetValue(FormModel) == expected
        //         ? Enumerable.Empty<string>()
        //         : new[] { "Must be test" });

        Assert.That(FormModel.Name, Is.EqualTo(""));
        Assert.That(FormModel.Shortname, Is.EqualTo(""));
        Assert.That(FormModel.Authors, Is.EqualTo(""));
        Assert.That(FormModel.Language, Is.EqualTo(""));
        Assert.That(FormModel.Description, Is.EqualTo(""));
        Assert.That(FormModel.Goals, Is.EqualTo(""));


        var mudInputs = systemUnderTest.FindComponents<MudTextField<string>>();
        foreach (var mudInput in mudInputs.Take(4))
        {
            var input = mudInput.Find("input");
            input.Change(expected);
        }
        foreach (var mudInput in mudInputs.Skip(4))
        {
            var input = mudInput.Find("textarea");
            input.Change(expected);
        }

        Assert.That(FormModel.Name, Is.EqualTo(expected));
        Assert.That(FormModel.Shortname, Is.EqualTo(expected));
        Assert.That(FormModel.Authors, Is.EqualTo(expected));
        Assert.That(FormModel.Language, Is.EqualTo(expected));
        Assert.That(FormModel.Description, Is.EqualTo(expected));
        Assert.That(FormModel.Goals, Is.EqualTo(expected));
    }

    private IRenderedComponent<CreateWorldForm> GetRenderedComponent(EventCallback? onSubmitted = null)
    {
        onSubmitted ??= EventCallback.Empty;
        return Context.RenderComponent<CreateWorldForm>(parameters =>
        {
            parameters.Add(c => c.OnSubmitted, onSubmitted.Value);
            parameters.Add(c => c.DebounceInterval, 0);
        });
    }
}