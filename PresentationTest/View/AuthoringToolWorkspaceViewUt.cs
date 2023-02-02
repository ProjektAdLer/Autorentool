using System;
using System.Collections.Generic;
using System.Linq;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.ModalDialog;
using Presentation.PresentationLogic.Space;
using Presentation.PresentationLogic.World;
using Presentation.View;
using Presentation.View.World;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View;

[TestFixture]

public class AuthoringToolWorkspaceViewUt
{
#pragma warning disable CS8618 //set in setup - m.ho
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
        _ctx.ComponentFactories.AddStub<WorldView>();
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
        var world1 = new WorldViewModel("a", "b", "c", "d", "e", "f");
        var world2 = new WorldViewModel("a", "b", "c", "d", "e", "f");

        var worlds = new List<WorldViewModel> {world1, world2};

        _authoringToolWorkspaceViewModel.Worlds.Returns(worlds);
        
        var systemUnderTest = GetWorkspaceViewForTesting();

        var titleHeader = systemUnderTest.FindOrFail("h3");
        var worldCountFilePath = systemUnderTest.FindAll("p");

        titleHeader.MarkupMatches("<h3>AuthoringTool Workspace</h3>");
        worldCountFilePath.MarkupMatches("<p role=\"status\">Current count of worlds: 2</p> <p role=\"status\" id=\"filepath\">Filepath:</p>");
    }
    
    [Test]
    public void Render_RendersWorldSelection()
    {
        var world1 = new WorldViewModel("ab", "eb", "ic", "od", "ue", "af");
        var world2 = new WorldViewModel("aa", "bb", "cc", "dd", "ee", "ff");
        var world3 = new WorldViewModel("gg", "hh", "ii", "jj", "kk", "ll");
        var space = Substitute.For<ISpaceViewModel>();

        _authoringToolWorkspaceViewModel.Worlds.Returns(new List<WorldViewModel>()
        {
            world1, world2, world3
        });

        _authoringToolWorkspaceViewModel.SelectedWorld = world1;
        
        world1.Spaces.Add(space);
        
        Assert.That(_authoringToolWorkspaceViewModel.SelectedWorld, Is.Not.EqualTo(null));
        
        var systemUnderTest = GetWorkspaceViewForTesting();
        
        var worldData = systemUnderTest.FindOrFail("label.world-data");
        var worldSelection = systemUnderTest.FindOrFail("select");

        worldSelection.MarkupMatches("<select  value=\"ab\"><option value=\"ab\" selected=\"\">ab</option><option value=\"aa\">aa</option><option value=\"gg\">gg</option></select>");
        worldData.MarkupMatches("<label class=\"world-data\"> Selected world: ab, Description: ue, Spaces: 1</label>");
    }

    [Test]
    public void ShowInformationMessageDialog_FlagSet_CallsFactory_RendersRenderFragment_CallsOnDialogClose()
    {
        _authoringToolWorkspacePresenter.InformationMessageToShow = "info";

        RenderFragment fragment = builder =>
        {
            builder.OpenElement(0, "p");
            builder.AddContent(1, "bar");
            builder.CloseElement();
        };
        ModalDialogOnClose? callback = null;

        _modalDialogFactory.GetInformationMessageFragment(Arg.Any<ModalDialogOnClose>(), _authoringToolWorkspacePresenter.InformationMessageToShow)
            .Returns(fragment)
            .AndDoes(ci =>
            {
                callback = ci.Arg<ModalDialogOnClose>();
            });

        var systemUnderTest = GetWorkspaceViewForTesting();

        _modalDialogFactory.Received().GetInformationMessageFragment(Arg.Any<ModalDialogOnClose>(), _authoringToolWorkspacePresenter.InformationMessageToShow);
        var p = systemUnderTest.FindAllOrFail("p").ElementAt(2);
        p.MarkupMatches("<p>bar</p>");

        if (callback == null)
        {
            Assert.Fail("Didn't get a callback from call to modal dialog factory");
        }

        var returnValue = new ModalDialogOnCloseResult(ModalDialogReturnValue.Ok);
        
        callback!.Invoke(returnValue);
        
        Assert.That(_authoringToolWorkspacePresenter.InformationMessageToShow, Is.EqualTo(null));
    }
    
    [Test]
    public void ShowSaveUnsavedWorldsDialog_FlagSet_CallsFactory_RendersRenderFragment_CallsPresenterOnDialogClose()
    {
        var world = new WorldViewModel("a","b","c","d","e","f");
        var worlds = new List<WorldViewModel> {world};
        var unsavedWorldQueues = new Queue<WorldViewModel>(worlds);

        _authoringToolWorkspacePresenter.UnsavedWorldsQueue = unsavedWorldQueues;
        _authoringToolWorkspacePresenter.SaveUnsavedChangesDialogOpen = true;

        RenderFragment fragment = builder =>
        {
            builder.OpenElement(0, "p");
            builder.AddContent(1, "bar");
            builder.CloseElement();
        };
        ModalDialogOnClose? callback = null;

        _modalDialogFactory.GetSaveUnsavedWorldsFragment(Arg.Any<ModalDialogOnClose>(), _authoringToolWorkspacePresenter.UnsavedWorldsQueue.Peek().Name)
            .Returns(fragment)
            .AndDoes(ci =>
            {
                callback = ci.Arg<ModalDialogOnClose>();
            });

        var systemUnderTest = GetWorkspaceViewForTesting();

        _modalDialogFactory.Received().GetSaveUnsavedWorldsFragment(Arg.Any<ModalDialogOnClose>(), world.Name);
        var p = systemUnderTest.FindAllOrFail("p").ElementAt(2);
        p.MarkupMatches("<p>bar</p>");

        if (callback == null)
        {
            Assert.Fail("Didn't get a callback from call to modal dialog factory");
        }

        var returnValue = new ModalDialogOnCloseResult(ModalDialogReturnValue.Yes);
        
        callback!.Invoke(returnValue);
        
        _authoringToolWorkspacePresenter.Received().OnSaveWorldDialogClose(returnValue);
    }
    
    [Test]
    public void ShowDeleteUnsavedWorldDialog_FlagSet_CallsFactory_RendersRenderFragment_CallsPresenterOnDialogClose()
    {
        var world = new WorldViewModel("a","b","c","d","e","f");

        _authoringToolWorkspacePresenter.DeletedUnsavedWorld = world;

        RenderFragment fragment = builder =>
        {
            builder.OpenElement(0, "p");
            builder.AddContent(1, "bar");
            builder.CloseElement();
        };
        ModalDialogOnClose? callback = null;

        _modalDialogFactory.GetDeleteUnsavedWorldFragment(Arg.Any<ModalDialogOnClose>(), _authoringToolWorkspacePresenter.DeletedUnsavedWorld.Name)
            .Returns(fragment)
            .AndDoes(ci =>
            {
                callback = ci.Arg<ModalDialogOnClose>();
            });

        var systemUnderTest = GetWorkspaceViewForTesting();

        _modalDialogFactory.Received().GetDeleteUnsavedWorldFragment(Arg.Any<ModalDialogOnClose>(), world.Name);
        var p = systemUnderTest.FindAllOrFail("p").ElementAt(2);
        p.MarkupMatches("<p>bar</p>");

        if (callback == null)
        {
            Assert.Fail("Didn't get a callback from call to modal dialog factory");
        }

        var returnValue = new ModalDialogOnCloseResult(ModalDialogReturnValue.Yes);
        
        callback!.Invoke(returnValue);
        
        _authoringToolWorkspacePresenter.Received().OnSaveDeletedWorldDialogClose(returnValue);
    }
    
    [Test]
    public void ShowCreateWorldDialog_FlagSet_CallsFactory_RendersRenderFragment_CallsPresenterOnDialogClose()
    {
        _authoringToolWorkspacePresenter.CreateWorldDialogOpen.Returns(true);
        
        RenderFragment fragment = builder =>
        {
            builder.OpenElement(0, "p");
            builder.AddContent(1, "bar");
            builder.CloseElement();
        };
        ModalDialogOnClose? callback = null;

        _modalDialogFactory.GetCreateWorldFragment(Arg.Any<ModalDialogOnClose>())
            .Returns(fragment)
            .AndDoes(ci =>
            {
                callback = ci.Arg<ModalDialogOnClose>();
            });

        var systemUnderTest = GetWorkspaceViewForTesting();

        _modalDialogFactory.Received().GetCreateWorldFragment(Arg.Any<ModalDialogOnClose>());
        var p = systemUnderTest.FindAllOrFail("p").ElementAt(2);
        p.MarkupMatches("<p>bar</p>");

        if (callback == null)
        {
            Assert.Fail("Didn't get a callback from call to modal dialog factory");
        }
        
        var returnDictionary = new Dictionary<string, string>
        {
            { "foo", "baz" },
            { "bar", "baz" }
        };
        var returnValue = new ModalDialogOnCloseResult(ModalDialogReturnValue.Ok, returnDictionary);

        callback!.Invoke(returnValue);
        
        _authoringToolWorkspacePresenter.Received().OnCreateWorldDialogClose(returnValue);
    }
    
    [Test]
    public void ShowEditWorldDialog_FlagSet_CallsFactory_RendersRenderFragment_CallsPresenterOnDialogClose()
    {
        _authoringToolWorkspacePresenter.AuthoringToolWorkspaceVm.Returns(_authoringToolWorkspaceViewModel);
        _authoringToolWorkspacePresenter.EditWorldDialogOpen.Returns(true);
        var initialValues = new Dictionary<string, string>
        {
            {"baba", "bubu"}
        };
        
        _authoringToolWorkspaceViewModel.EditDialogInitialValues.Returns(initialValues);
        
        RenderFragment fragment = builder =>
        {
            builder.OpenElement(0, "p");
            builder.AddContent(1, "bar");
            builder.CloseElement();
        };
        ModalDialogOnClose? callback = null;

        _modalDialogFactory.
            GetEditWorldFragment(initialValues,Arg.Any<ModalDialogOnClose>())
            .Returns(fragment)
            .AndDoes(ci =>
            {
                callback = ci.Arg<ModalDialogOnClose>();
            });

        var systemUnderTest = GetWorkspaceViewForTesting();

        _modalDialogFactory.Received().GetEditWorldFragment(initialValues, Arg.Any<ModalDialogOnClose>());
        var p = systemUnderTest.FindAllOrFail("p").ElementAt(2);
        p.MarkupMatches("<p>bar</p>");

        if (callback == null)
        {
            Assert.Fail("Didn't get a callback from call to modal dialog factory");
        }
        
        var returnDictionary = new Dictionary<string, string>
        {
            { "foo", "baz" },
            { "bar", "baz" }
        };
        var returnValue = new ModalDialogOnCloseResult(ModalDialogReturnValue.Ok, returnDictionary);

        callback!.Invoke(returnValue);
        
        _authoringToolWorkspacePresenter.Received().OnEditWorldDialogClose(returnValue);
    }

    [Test]
    public void AddWorldButton_Clicked_CallsAddNewWorld()
    {
        var systemUnderTest = GetWorkspaceViewForTesting();

        var addWorldButton = systemUnderTest.FindOrFail("button.btn.btn-primary.add-world");
        addWorldButton.Click();
        _authoringToolWorkspacePresenter.Received().AddNewWorld();
    }
    
    [Test]
    public void LoadWorldButton_Clicked_CallsLoadWorldAsync()
    {
        var systemUnderTest = GetWorkspaceViewForTesting();

        var loadWorldButton = systemUnderTest.FindOrFail("button.btn.btn-primary.load-world");
        loadWorldButton.Click();
        _authoringToolWorkspacePresenter.Received().LoadWorldAsync();
    }
    
    [Test]
    public void LoadWorldButton_Clicked_OperationCancelledExceptionCaught()
    {
        _authoringToolWorkspacePresenter.LoadWorldAsync().Throws<OperationCanceledException>();
        
        var systemUnderTest = GetWorkspaceViewForTesting();
        
        var loadWorldButton = systemUnderTest.FindOrFail("button.btn.btn-primary.load-world");
        Assert.That(() => loadWorldButton.Click(), Throws.Nothing);
        _authoringToolWorkspacePresenter.Received().LoadWorldAsync();
    }
    
    [Test]
    public void LoadWorldButton_Clicked_OtherExceptionsWrappedInErrorState()
    {
        _authoringToolWorkspacePresenter.LoadWorldAsync().Throws(new Exception("lelele"));
        
        var systemUnderTest = GetWorkspaceViewForTesting();
        
        var loadWorldButton = systemUnderTest.FindOrFail("button.btn.btn-primary.load-world");
        Assert.That(() => loadWorldButton.Click(), Throws.Nothing);
        _authoringToolWorkspacePresenter.Received().LoadWorldAsync();
    }
    
    [Test]
    public void EditWorldButton_Clicked_CallsOpenEditSelectedWorldDialog()
    {
        var world = new WorldViewModel("a", "b", "c", "d", "e", "f");
        _authoringToolWorkspacePresenter.AuthoringToolWorkspaceVm.SelectedWorld.Returns(world);

        var systemUnderTest = GetWorkspaceViewForTesting();

        var editWorldButton = systemUnderTest.FindOrFail("button.btn.btn-primary.edit-world");
        editWorldButton.Click();
        _authoringToolWorkspacePresenter.Received().OpenEditSelectedWorldDialog();
    }
    
    [Test]
    public void DeleteWorldButton_Clicked_CallsDeleteSelectedWorld()
    {
        var world = new WorldViewModel("a", "b", "c", "d", "e", "f");
        _authoringToolWorkspacePresenter.AuthoringToolWorkspaceVm.SelectedWorld.Returns(world);
        
        var systemUnderTest = GetWorkspaceViewForTesting();

        var deleteWorldButton = systemUnderTest.FindOrFail("button.btn.btn-primary.delete-world");
        deleteWorldButton.Click();
        _authoringToolWorkspacePresenter.Received().DeleteSelectedWorld();
    }
    
    [Test]
    public void SaveWorldButton_Clicked_CallsSaveSelectedWorldAsync()
    {
        var world = new WorldViewModel("a", "b", "c", "d", "e", "f");
        _authoringToolWorkspacePresenter.AuthoringToolWorkspaceVm.SelectedWorld.Returns(world);
        
        var systemUnderTest = GetWorkspaceViewForTesting();

        var saveWorldButton = systemUnderTest.FindOrFail("button.btn.btn-primary.save-world");
        saveWorldButton.Click();
        _authoringToolWorkspacePresenter.Received().SaveSelectedWorldAsync();
    }
    
    [Test]
    public void SaveWorldButton_Clicked_OperationCancelledExceptionCaught()
    {
        _authoringToolWorkspacePresenter.SaveSelectedWorldAsync().Throws<OperationCanceledException>();
        
        var systemUnderTest = GetWorkspaceViewForTesting();
        
        var saveWorldButton = systemUnderTest.FindOrFail("button.btn.btn-primary.save-world");
        Assert.That(() => saveWorldButton.Click(), Throws.Nothing);
        _authoringToolWorkspacePresenter.Received().SaveSelectedWorldAsync();
    }
    
    [Test]
    public void SaveWorldButton_Clicked_OtherExceptionsWrappedInErrorState()
    {
        _authoringToolWorkspacePresenter.SaveSelectedWorldAsync().Throws(new Exception("lelele"));
        
        var systemUnderTest = GetWorkspaceViewForTesting();
        
        var saveWorldButton = systemUnderTest.FindOrFail("button.btn.btn-primary.save-world");
        Assert.That(() => saveWorldButton.Click(), Throws.Nothing);
        _authoringToolWorkspacePresenter.Received().SaveSelectedWorldAsync();
    }
    
    [Test]
    public void ExportWorldButton_Clicked_CallsExportWorld()
    {
        var world = new WorldViewModel("a", "b", "c", "d", "e", "f");
        var workSpaceVm = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        workSpaceVm.SelectedWorld.Returns(world);
        _authoringToolWorkspaceViewModel.SelectedWorld.Returns(world);

        var systemUnderTest = GetWorkspaceViewForTesting();
        
        Assert.That(_authoringToolWorkspaceViewModel.SelectedWorld, Is.Not.EqualTo(null));

        var saveWorldButton = systemUnderTest.FindOrFail("button.btn.btn-primary.export-world");
        saveWorldButton.Click();
        _presentationLogic.Received().ConstructBackupAsync(world);
    }


    private IRenderedComponent<AuthoringToolWorkspaceView> GetWorkspaceViewForTesting()
    {
        return _ctx.RenderComponent<AuthoringToolWorkspaceView>();
    }
}