using Bunit;
using BusinessLogic.Entities;
using Microsoft.Extensions.DependencyInjection;
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

    [SetUp]
    public void Setup()
    {
        WorkspacePresenter = Substitute.For<IAuthoringToolWorkspacePresenter>();
        WorkspaceViewModel = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        FormDataContainer = Substitute.For<IFormDataContainer<LearningWorldFormModel, LearningWorld>>();
        var formModel = FormModelProvider.GetLearningWorld();
        var entity = EntityProvider.GetLearningWorld();
        FormDataContainer.FormModel.Returns(formModel);
        FormDataContainer.GetMappedEntity().Returns(entity);
        Context.Services.AddSingleton(WorkspacePresenter);
        Context.Services.AddSingleton(WorkspaceViewModel);
        Context.Services.AddSingleton(FormDataContainer);
    }
    
    [Test]
    public void Render_InjectsDependencies()
    {
        var systemUnderTest = GetRenderedComponent();
        
    }
    
    [Test]
    public void ChangeFieldValues_ChangesContainerValues()
    {
        
    }

    private IRenderedComponent<CreateWorldForm> GetRenderedComponent()
    {
        return Context.RenderComponent<CreateWorldForm>();
    }
}