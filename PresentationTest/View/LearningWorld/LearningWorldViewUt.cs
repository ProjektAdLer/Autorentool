using System;
using System.Collections.Generic;
using System.Linq;
using Bunit;
using Bunit.Rendering;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.ModalDialog;
using Presentation.View.LearningElement;
using Presentation.View.LearningSpace;
using Presentation.View.LearningWorld;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.LearningWorld;

[TestFixture]
public class LearningWorldViewUt
{
#pragma warning disable CS8618 // set in setup - n.stich
    private TestContext _ctx;
    private IMouseService _mouseService;
    private ILearningWorldPresenter _worldPresenter;
    private ILearningWorldViewModalDialogFactory _modalDialogFactory;
#pragma warning restore CS8618
    
    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _mouseService = Substitute.For<IMouseService>();
        _worldPresenter = Substitute.For<ILearningWorldPresenter>();
        _modalDialogFactory = Substitute.For<ILearningWorldViewModalDialogFactory>();
        _ctx.ComponentFactories.AddStub<LearningSpaceView>();
        _ctx.ComponentFactories.AddStub<DraggableLearningSpace>();
        _ctx.ComponentFactories.AddStub<DraggableLearningElement>();
        _ctx.ComponentFactories.AddStub<PathWay>();
        _ctx.ComponentFactories.AddStub<ModalDialog>();
        _ctx.Services.AddSingleton(_mouseService);
        _ctx.Services.AddSingleton(_worldPresenter);
        _ctx.Services.AddSingleton(_modalDialogFactory);
    }

    [Test]
    public void Constructor_InjectsDependencies()
    {
        var systemUnderTest = GetLearningWorldViewForTesting();
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.MouseService, Is.EqualTo(_mouseService));
            Assert.That(systemUnderTest.Instance.LearningWorldP, Is.EqualTo(_worldPresenter));
            Assert.That(systemUnderTest.Instance.ModalDialogFactory, Is.EqualTo(_modalDialogFactory));
        });
    }

    [Test]
    public void Render_ChildContentSet_RendersChildContent()
    {
        RenderFragment childContent = builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "barbaz");
            builder.AddContent(2, "foobar");
            builder.CloseElement();
        };
        
        var systemUnderTest = GetLearningWorldViewForTesting(childContent);

        Assert.That(systemUnderTest.FindOrFail("div.barbaz").TextContent, Is.EqualTo("foobar"));
    }

    [Test]
    public void Render_ShowingLearningSpaceFalse_DoesNotRenderLearningSpaceView()
    {
        _worldPresenter.ShowingLearningSpaceView.Returns(false);
        
        var systemUnderTest = GetLearningWorldViewForTesting();

        Assert.That(systemUnderTest.FindComponent<Stub<LearningSpaceView>>,
            Throws.TypeOf<ComponentNotFoundException>());
    }

    [Test]
    public void Render_ShowingLearningSpaceTrue_DoesRenderLearningSpaceViewWithButton()
    {
        _worldPresenter.ShowingLearningSpaceView.Returns(true);
        
        var systemUnderTest = GetLearningWorldViewForTesting();

        var spaceView = systemUnderTest.FindComponentOrFail<Stub<LearningSpaceView>>();
        var childContent = (RenderFragment)spaceView.Instance.Parameters[nameof(LearningSpaceView.ChildContent)];
        Assert.That(childContent, Is.Not.Null);
        var childContentRendered = _ctx.Render(childContent);
        childContentRendered.MarkupMatches(
            @"<button class=""btn btn-primary"">Close Learning Space View</button>");
    }

    [Test]
    public void Render_ShowLearningSpaceTrue_ChildContentOnClickCallsCloseLearningSpaceView()
    {
        _worldPresenter.ShowingLearningSpaceView.Returns(true);
        
        var systemUnderTest = GetLearningWorldViewForTesting();

        var spaceView = systemUnderTest.FindComponentOrFail<Stub<LearningSpaceView>>();
        var childContent = (RenderFragment)spaceView.Instance.Parameters[nameof(LearningSpaceView.ChildContent)];
        Assert.That(childContent, Is.Not.Null);
        var childContentRendered = _ctx.Render(childContent);
        childContentRendered.FindOrFail("button.btn.btn-primary").Click();
        _worldPresenter.Received(1).CloseLearningSpaceView();
    }

    [Test]
    public void Render_LearningWorldSet_RendersNameWorkloadAndPoints()
    {
        var learningWorld = Substitute.For<ILearningWorldViewModel>();
        learningWorld.Name.Returns("my insanely sophisticated name");
        learningWorld.Workload.Returns(42);
        learningWorld.Points.Returns(9);
        _worldPresenter.LearningWorldVm.Returns(learningWorld);
        
        var systemUnderTest = GetLearningWorldViewForTesting();

        var h2 = systemUnderTest.FindOrFail("h2");
        h2.MarkupMatches(@"<h2>World: my insanely sophisticated name</h2>");
        var h5 = systemUnderTest.FindAll("h5");
        h5[0].MarkupMatches(@"<h5>Workload: 42 minutes</h5>");
        h5[1].MarkupMatches(@"<h5>Points: 9</h5>");
    }
    
    [Test]
    public void Render_LearningSpaceInWorld_CorrectDraggableForEachSpace()
    {
        var space1 = Substitute.For<ILearningSpaceViewModel>();
        var space2 = Substitute.For<ILearningSpaceViewModel>();
        var learningSpaces = new List<ILearningSpaceViewModel> { space1, space2 };
        var learningPathWay = new LearningPathwayViewModel(space1, space2);
        var learningPathWays = new List<ILearningPathWayViewModel> { learningPathWay };

        var learningWorld = Substitute.For<ILearningWorldViewModel>();
        learningWorld.LearningSpaces.Returns(learningSpaces);
        learningWorld.LearningPathWays.Returns(learningPathWays);
        _worldPresenter.LearningWorldVm.Returns(learningWorld);
        
        var systemUnderTest = GetLearningWorldViewForTesting();

        var draggableLearningSpaces = systemUnderTest.FindComponentsOrFail<Stub<DraggableLearningSpace>>().ToList();
        var pathWays = systemUnderTest.FindComponentsOrFail<Stub<PathWay>>().ToList();

        Assert.Multiple(() =>
        {
            Assert.That(draggableLearningSpaces, Has.Count.EqualTo(learningSpaces.Count));
            Assert.That(learningSpaces.All(le =>
                draggableLearningSpaces.Any(dle =>
                    dle.Instance.Parameters[nameof(DraggableLearningSpace.LearningSpace)] == le)));
            Assert.That(pathWays, Has.Count.EqualTo(learningPathWays.Count));
        });
    }

    [Test]
    public void CreateLearningSpaceDialog_FlagSet_CallsFactory_RendersRenderFragment_CallsPresenterOnDialogClose()
    {
        //prepare presenter
        var learningWorld = Substitute.For<ILearningWorldViewModel>();
        _worldPresenter.LearningWorldVm.Returns(learningWorld);
        _worldPresenter.CreateLearningSpaceDialogOpen.Returns(true);
        
        RenderFragment fragment = builder =>
        {
            builder.OpenElement(0, "p");
            builder.AddContent(1, "foobar");
            builder.CloseElement();
        };
        ModalDialogOnClose? callback = null;
        _modalDialogFactory
            .GetCreateLearningSpaceFragment(Arg.Any<ModalDialogOnClose>())
            .Returns(fragment)
            .AndDoes(ci =>
            {
                callback = ci.Arg<ModalDialogOnClose>();
            });
        
        var systemUnderTest = GetLearningWorldViewForTesting();

        _modalDialogFactory.Received().GetCreateLearningSpaceFragment(Arg.Any<ModalDialogOnClose>());
        var p = systemUnderTest.FindOrFail("p");
        p.MarkupMatches("<p>foobar</p>");

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
        
        //call the callback with the return value
        callback!.Invoke(returnValue);
        
        //assert that the delegate we called executed presenter
        _worldPresenter.Received().OnCreateSpaceDialogClose(returnValue);
    }

    [Test]
    public void EditLearningSpaceDialog_FlagSet_CallsFactory_RendersRenderFragment_CallsPresenterOnDialogClose()
    {
        //prepare presenter
        var learningWorld = Substitute.For<ILearningWorldViewModel>();
        _worldPresenter.LearningWorldVm.Returns(learningWorld);
        _worldPresenter.EditLearningSpaceDialogOpen.Returns(true);
        var initialValues = new Dictionary<string, string>
        {
            {"baba", "bubu"}
        };
        _worldPresenter.EditSpaceDialogInitialValues.Returns(initialValues);
        
        RenderFragment fragment = builder =>
        {
            builder.OpenElement(0, "p");
            builder.AddContent(1, "foobar");
            builder.CloseElement();
        };
        ModalDialogOnClose? callback = null;
        _modalDialogFactory
            .GetEditLearningSpaceFragment(initialValues, Arg.Any<ModalDialogOnClose>())
            .Returns(fragment)
            .AndDoes(ci =>
            {
                callback = ci.Arg<ModalDialogOnClose>();
            });
        
        var systemUnderTest = GetLearningWorldViewForTesting();

        _modalDialogFactory.Received().GetEditLearningSpaceFragment(initialValues, Arg.Any<ModalDialogOnClose>());
        var p = systemUnderTest.FindOrFail("p");
        p.MarkupMatches("<p>foobar</p>");

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
        
        //call the callback with the return value
        callback!.Invoke(returnValue);
        
        //assert that the delegate we called executed presenter
        _worldPresenter.Received().OnEditSpaceDialogClose(returnValue);
    }


    [Test]
    public void Svg_MouseMove_CallsMouseService()
    {
        var mouseEventArgs = new MouseEventArgs();
        var systemUnderTest = GetLearningWorldViewForTesting();

        var svg = systemUnderTest.FindOrFail("svg");
        svg.MouseMove(mouseEventArgs);
        
        _mouseService.Received().FireMove(systemUnderTest.Instance, mouseEventArgs);
    }

    [Test]
    public void Svg_MouseUp_CallsMouseService()
    {
        var mouseEventArgs = new MouseEventArgs();
        var systemUnderTest = GetLearningWorldViewForTesting();

        var svg = systemUnderTest.FindOrFail("svg");
        svg.MouseUp(mouseEventArgs);
        
        _mouseService.Received().FireUp(systemUnderTest.Instance, mouseEventArgs);
    }

    [Test]
    public void Svg_MouseLeave_CallsMouseService()
    {
        var mouseEventArgs = new MouseEventArgs();
        var systemUnderTest = GetLearningWorldViewForTesting();

        var svg = systemUnderTest.FindOrFail("svg");
        svg.MouseLeave(mouseEventArgs);
        
        _mouseService.Received().FireOut(systemUnderTest.Instance, mouseEventArgs);
    }

    [Test]
    public void AddSpaceButton_Clicked_CallsAddNewLearningSpace()
    {
        var systemUnderTest = GetLearningWorldViewForTesting();

        var addSpaceButton = systemUnderTest.FindOrFail("button.btn.btn-primary.add-learning-space");
        addSpaceButton.Click();
        _worldPresenter.Received().AddNewLearningSpace();
    }

    [Test]
    public void LoadSpaceButton_Clicked_CallsLoadLearningSpaceAsync()
    {
        var systemUnderTest = GetLearningWorldViewForTesting();
        
        var loadSpaceButton = systemUnderTest.FindOrFail("button.btn.btn-primary.load-learning-space");
        loadSpaceButton.Click();
        _worldPresenter.Received().LoadLearningSpaceAsync();
    }

    [Test]
    public void LoadSpaceButton_Clicked_OperationCancelledExceptionCaught()
    {
        _worldPresenter.LoadLearningSpaceAsync().Throws<OperationCanceledException>();
        
        var systemUnderTest = GetLearningWorldViewForTesting();
        
        var loadSpaceButton = systemUnderTest.FindOrFail("button.btn.btn-primary.load-learning-space");
        Assert.That(() => loadSpaceButton.Click(), Throws.Nothing);
        _worldPresenter.Received().LoadLearningSpaceAsync();
    }

    [Test]
    public void LoadSpaceButton_Clicked_OtherExceptionsWrappedInErrorState()
    {
        _worldPresenter.LoadLearningSpaceAsync().Throws(new Exception("saatana"));
        
        var systemUnderTest = GetLearningWorldViewForTesting();
        
        var loadSpaceButton = systemUnderTest.FindOrFail("button.btn.btn-primary.load-learning-space");
        Assert.That(() => loadSpaceButton.Click(), Throws.Nothing);
        _worldPresenter.Received().LoadLearningSpaceAsync();
        
        var errorDialog = systemUnderTest.FindComponentOrFail<Stub<ModalDialog>>();
        Assert.Multiple(() =>
        {
            Assert.That(errorDialog.Instance.Parameters[nameof(ModalDialog.Title)], Is.EqualTo("Exception encountered"));
            Assert.That(errorDialog.Instance.Parameters[nameof(ModalDialog.Text)], Is.EqualTo(
                @"Exception encountered at Load learning space:
Exception:
saatana"));
            Assert.That(errorDialog.Instance.Parameters[nameof(ModalDialog.DialogType)], Is.EqualTo(ModalDialogType.Ok));
        });
        
        var onclose = errorDialog.Instance.Parameters[nameof(ModalDialog.OnClose)] as ModalDialogOnClose;
        Assert.That(onclose, Is.Not.Null);
        //nullability overridden because of above assert - n.stich
        onclose!.Invoke(new ModalDialogOnCloseResult(ModalDialogReturnValue.Ok));   
        Assert.That(() => systemUnderTest.FindComponent<Stub<ModalDialog>>(), Throws.Exception);
    }
    
    [Test]
    public void EditSpaceButton_Clicked_CallsOpenEditSelectedLearningSpaceDialog()
    {
        var space = Substitute.For<ILearningSpaceViewModel>();
        var worldVm = Substitute.For<ILearningWorldViewModel>();
        worldVm.LearningSpaces.Returns(new List<ILearningSpaceViewModel> { space });
        worldVm.SelectedLearningSpace.Returns(space);
        _worldPresenter.LearningWorldVm.Returns(worldVm);

        var systemUnderTest = GetLearningWorldViewForTesting();
        
        var editSpaceButton = systemUnderTest.FindOrFail("button.btn.btn-primary.edit-learning-space");
        editSpaceButton.Click();
        _worldPresenter.Received().OpenEditSelectedLearningSpaceDialog();
    }

    [Test]
    public void DeleteSpaceButton_Clicked_CallsDeleteSelectedLearningSpace()
    {
        var space = Substitute.For<ILearningSpaceViewModel>();
        var worldVm = Substitute.For<ILearningWorldViewModel>();
        worldVm.LearningSpaces.Returns(new List<ILearningSpaceViewModel> { space });
        worldVm.SelectedLearningSpace.Returns(space);
        _worldPresenter.LearningWorldVm.Returns(worldVm);

        var systemUnderTest = GetLearningWorldViewForTesting();
        
        var editSpaceButton = systemUnderTest.FindOrFail("button.btn.btn-primary.delete-learning-space");
        editSpaceButton.Click();
        _worldPresenter.Received().DeleteSelectedLearningSpace();
    }
    
    [Test]
    public void SaveSpaceButton_Clicked_CallsSaveSelectedLearningSpaceAsync()
    {
        var systemUnderTest = GetLearningWorldViewForTesting();
        
        var loadElementButton = systemUnderTest.FindOrFail("button.btn.btn-primary.save-learning-space");
        loadElementButton.Click();
        _worldPresenter.Received().SaveSelectedLearningSpaceAsync();
    }
    
    [Test]
    public void SaveSpaceButton_Clicked_OperationCancelledExceptionCaught()
    {
        _worldPresenter.SaveSelectedLearningSpaceAsync().Throws<OperationCanceledException>();
        
        var systemUnderTest = GetLearningWorldViewForTesting();
        
        var loadElementButton = systemUnderTest.FindOrFail("button.btn.btn-primary.save-learning-space");
        Assert.That(() => loadElementButton.Click(), Throws.Nothing);
        _worldPresenter.Received().SaveSelectedLearningSpaceAsync();
    }
    
    [Test]
    public void SaveSpaceButton_Clicked_OtherExceptionsWrappedInErrorState()
    {
        _worldPresenter.SaveSelectedLearningSpaceAsync().Throws(new Exception("saatana"));
        
        var systemUnderTest = GetLearningWorldViewForTesting();
        
        var loadElementButton = systemUnderTest.FindOrFail("button.btn.btn-primary.save-learning-space");
        Assert.That(() => loadElementButton.Click(), Throws.Nothing);
        _worldPresenter.Received().SaveSelectedLearningSpaceAsync();
    }
    
    [Test]
    public void ShowSelectedSpace_Called_CallsShowSelectedLearningSpaceView()
    {
        var worldVm = Substitute.For<ILearningWorldViewModel>();
        _worldPresenter.LearningWorldVm.Returns(worldVm);

        var systemUnderTest = GetLearningWorldViewForTesting();
        
        var editSpaceButton = systemUnderTest.FindOrFail("button.btn.btn-primary.show-learning-space");
        editSpaceButton.Click();
        _worldPresenter.Received().ShowSelectedLearningSpaceView();
    }
    
    private IRenderedComponent<LearningWorldView> GetLearningWorldViewForTesting(RenderFragment? childContent = null)
    {
        return _ctx.RenderComponent<LearningWorldView>(parameters => parameters
            .Add(p => p.ChildContent, childContent));
    }
}