using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.MyLearningWorlds;
using Presentation.PresentationLogic.SelectedViewModels;

namespace PresentationTest.PresentationLogic.MyLearningWorlds;

[TestFixture]
public class MyLearningWorldsProviderUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var logger = Substitute.For<ILogger<MyLearningWorldsProvider>>();
        var presentationLogic = Substitute.For<IPresentationLogic>();
        var workspaceViewModel = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        var workspacePresenter = Substitute.For<IAuthoringToolWorkspacePresenter>();
        workspacePresenter.AuthoringToolWorkspaceVm.Returns(workspaceViewModel);
        var selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();

        var systemUnderTest =
            CreateProviderForTesting(logger, presentationLogic, workspacePresenter, selectedViewModelsProvider);
        
        Assert.That(systemUnderTest.Logger, Is.EqualTo(logger));
        Assert.That(systemUnderTest.PresentationLogic, Is.EqualTo(presentationLogic));
        Assert.That(systemUnderTest.WorkspacePresenter, Is.EqualTo(workspacePresenter));
        Assert.That(systemUnderTest.SelectedViewModelsProvider, Is.EqualTo(selectedViewModelsProvider));
        Assert.That(systemUnderTest.WorkspaceVm, Is.EqualTo(workspaceViewModel));
    }

    private MyLearningWorldsProvider CreateProviderForTesting(ILogger<MyLearningWorldsProvider> logger,
        IPresentationLogic presentationLogic, IAuthoringToolWorkspacePresenter workspacePresenter,
        ISelectedViewModelsProvider selectedViewModelsProvider)
    {
        return new MyLearningWorldsProvider(logger, presentationLogic, workspacePresenter, selectedViewModelsProvider);
    }
}