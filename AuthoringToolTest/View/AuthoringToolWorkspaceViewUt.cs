using System;
using System.Collections.Generic;
using AuthoringTool.PresentationLogic.API;
using AuthoringTool.PresentationLogic.AuthoringToolWorkspace;
using AuthoringTool.PresentationLogic.LearningWorld;
using AuthoringTool.PresentationLogic.ModalDialog;
using AuthoringTool.View;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using TestContext = Bunit.TestContext;

namespace AuthoringToolTest.View;

[TestFixture]

public class AuthoringToolWorkspaceViewUt
{
#pragma warning disable CS8618 set in setup - m.ho
    private TestContext _ctx;
    private IAuthoringToolWorkspacePresenter _authoringToolWorkspacePresenter;
    private IAuthoringToolWorkspaceViewModel _authoringToolWorkspaceViewModel;
    private IAuthoringToolWorkspaceViewModalDialogFactory _modalDialogFactory;
    private IPresentationLogic _presentationLogic;
    private IMouseService _mouseService;
#pragma warning restore CS8618

    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _authoringToolWorkspacePresenter = Substitute.For<IAuthoringToolWorkspacePresenter>();
        _authoringToolWorkspaceViewModel = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        _modalDialogFactory = Substitute.For<IAuthoringToolWorkspaceViewModalDialogFactory>();
        _presentationLogic = Substitute.For<IPresentationLogic>();
        _mouseService = Substitute.For<IMouseService>();
        _ctx.Services.AddSingleton(_authoringToolWorkspacePresenter);
        _ctx.Services.AddSingleton(_authoringToolWorkspaceViewModel);
        _ctx.Services.AddSingleton(_modalDialogFactory);
        _ctx.Services.AddSingleton(_presentationLogic);
        _ctx.Services.AddSingleton(_mouseService);
        _ctx.Services.AddLogging();
    }
    
    [Test]
    public void Constructor_InjectsDependencies()
    {
        var systemUnderTest = GetWorkspaceViewForTesting();
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.MouseService, Is.EqualTo(_mouseService));
            Assert.That(systemUnderTest.Instance.AuthoringToolWorkspaceP, Is.EqualTo(_authoringToolWorkspacePresenter));
            Assert.That(systemUnderTest.Instance.AuthoringToolWorkspaceVm, Is.EqualTo(_authoringToolWorkspaceViewModel));
            Assert.That(systemUnderTest.Instance.ModalDialogFactory, Is.EqualTo(_modalDialogFactory));
            Assert.That(systemUnderTest.Instance.PresentationLogic, Is.EqualTo(_presentationLogic));
        });
    }
    
    [Test]
    public void Render_RendersTitleCountAndFilePath()
    {
        var world1 = new LearningWorldViewModel("a", "b", "c", "d", "e", "f");
        var world2 = new LearningWorldViewModel("a", "b", "c", "d", "e", "f");

        var learningWorlds = new List<LearningWorldViewModel> {world1, world2};

        _authoringToolWorkspaceViewModel.LearningWorlds.Returns(learningWorlds);
        
        var systemUnderTest = GetWorkspaceViewForTesting();

        var titleHeader = systemUnderTest.FindOrFail("h3");
        var worldCountFilePath = systemUnderTest.FindAll("p");

        titleHeader.MarkupMatches("<h3>AuthoringTool Workspace</h3>");
        worldCountFilePath.MarkupMatches("<p role=\"status\">Current count of learning worlds: 2</p> <p role=\"status\" id=\"filepath\">Filepath:</p>");
    }

    [Test]
    public void AddWorldButton_Clicked_CallsAddNewLearningWorld()
    {
        var systemUnderTest = GetWorkspaceViewForTesting();

        var addWorldButton = systemUnderTest.FindOrFail("button.btn.btn-primary.add-learning-world");
        addWorldButton.Click();
        _authoringToolWorkspacePresenter.Received().AddNewLearningWorld();
    }
    
    [Test]
    public void LoadWorldButton_Clicked_CallsLoadLearningWorldAsync()
    {
        var systemUnderTest = GetWorkspaceViewForTesting();

        var loadWorldButton = systemUnderTest.FindOrFail("button.btn.btn-primary.load-learning-world");
        loadWorldButton.Click();
        _authoringToolWorkspacePresenter.Received().LoadLearningWorldAsync();
    }
    
    [Test]
    public void LoadWorldButton_Clicked_OperationCancelledExceptionCaught()
    {
        _authoringToolWorkspacePresenter.LoadLearningWorldAsync().Throws<OperationCanceledException>();
        
        var systemUnderTest = GetWorkspaceViewForTesting();
        
        var loadWorldButton = systemUnderTest.FindOrFail("button.btn.btn-primary.load-learning-world");
        Assert.That(() => loadWorldButton.Click(), Throws.Nothing);
        _authoringToolWorkspacePresenter.Received().LoadLearningWorldAsync();
    }
    
    [Test]
    public void LoadWorldButton_Clicked_OtherExceptionsWrappedInErrorState()
    {
        _authoringToolWorkspacePresenter.LoadLearningWorldAsync().Throws(new Exception("lelele"));
        
        var systemUnderTest = GetWorkspaceViewForTesting();
        
        var loadWorldButton = systemUnderTest.FindOrFail("button.btn.btn-primary.load-learning-world");
        Assert.That(() => loadWorldButton.Click(), Throws.Nothing);
        _authoringToolWorkspacePresenter.Received().LoadLearningWorldAsync();
    }
    
    [Test]
    public void EditWorldButton_Clicked_CallsOpenEditSelectedLearningWorldDialog()
    {
        var world = new LearningWorldViewModel("a", "b", "c", "d", "e", "f");
        _authoringToolWorkspacePresenter.AuthoringToolWorkspaceVm.SelectedLearningWorld.Returns(world);

        var systemUnderTest = GetWorkspaceViewForTesting();

        var editWorldButton = systemUnderTest.FindOrFail("button.btn.btn-primary.edit-learning-world");
        editWorldButton.Click();
        _authoringToolWorkspacePresenter.Received().OpenEditSelectedLearningWorldDialog();
    }
    
    [Test]
    public void DeleteWorldButton_Clicked_CallsDeleteSelectedLearningWorld()
    {
        var world = new LearningWorldViewModel("a", "b", "c", "d", "e", "f");
        _authoringToolWorkspacePresenter.AuthoringToolWorkspaceVm.SelectedLearningWorld.Returns(world);
        
        var systemUnderTest = GetWorkspaceViewForTesting();

        var deleteWorldButton = systemUnderTest.FindOrFail("button.btn.btn-primary.delete-learning-world");
        deleteWorldButton.Click();
        _authoringToolWorkspacePresenter.Received().DeleteSelectedLearningWorld();
    }
    
    [Test]
    public void SaveWorldButton_Clicked_CallsSaveSelectedLearningWorldAsync()
    {
        var world = new LearningWorldViewModel("a", "b", "c", "d", "e", "f");
        _authoringToolWorkspacePresenter.AuthoringToolWorkspaceVm.SelectedLearningWorld.Returns(world);
        
        var systemUnderTest = GetWorkspaceViewForTesting();

        var saveWorldButton = systemUnderTest.FindOrFail("button.btn.btn-primary.save-learning-world");
        saveWorldButton.Click();
        _authoringToolWorkspacePresenter.Received().SaveSelectedLearningWorldAsync();
    }
    
    [Test]
    public void SaveWorldButton_Clicked_OperationCancelledExceptionCaught()
    {
        _authoringToolWorkspacePresenter.SaveSelectedLearningWorldAsync().Throws<OperationCanceledException>();
        
        var systemUnderTest = GetWorkspaceViewForTesting();
        
        var saveWorldButton = systemUnderTest.FindOrFail("button.btn.btn-primary.save-learning-world");
        Assert.That(() => saveWorldButton.Click(), Throws.Nothing);
        _authoringToolWorkspacePresenter.Received().SaveSelectedLearningWorldAsync();
    }
    
    [Test]
    public void SaveWorldButton_Clicked_OtherExceptionsWrappedInErrorState()
    {
        _authoringToolWorkspacePresenter.SaveSelectedLearningWorldAsync().Throws(new Exception("lelele"));
        
        var systemUnderTest = GetWorkspaceViewForTesting();
        
        var saveWorldButton = systemUnderTest.FindOrFail("button.btn.btn-primary.save-learning-world");
        Assert.That(() => saveWorldButton.Click(), Throws.Nothing);
        _authoringToolWorkspacePresenter.Received().SaveSelectedLearningWorldAsync();
    }


    private IRenderedComponent<AuthoringToolWorkspaceView> GetWorkspaceViewForTesting()
    {
        return _ctx.RenderComponent<AuthoringToolWorkspaceView>();
    }
}