using System;
using System.Collections.Generic;
using System.Linq;
using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic;
using AuthoringTool.PresentationLogic.AuthoringToolWorkspace;
using AuthoringTool.PresentationLogic.LearningContent;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;
using AuthoringTool.PresentationLogic.ModalDialog;
using AuthoringTool.View.LearningElement;
using AuthoringTool.View.LearningSpace;
using AuthoringTool.View.LearningWorld;
using Bunit;
using Bunit.Rendering;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using TestContext = Bunit.TestContext;

namespace AuthoringToolTest.View.LearningWorld;

[TestFixture]
public class LearningWorldViewUt
{
#pragma warning disable CS8618 set in setup - n.stich
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
    public void Render_LearningWorldSet_RendersNameAndWorkload()
    {
        var learningWorld = Substitute.For<ILearningWorldViewModel>();
        learningWorld.Name.Returns("my insanely sophisticated name");
        learningWorld.Workload.Returns(42);
        _worldPresenter.LearningWorldVm.Returns(learningWorld);
        
        var systemUnderTest = GetLearningWorldViewForTesting();

        var h2 = systemUnderTest.FindOrFail("h2");
        h2.MarkupMatches(@"<h2>World: my insanely sophisticated name</h2>");
        var h5 = systemUnderTest.FindOrFail("h5");
        h5.MarkupMatches(@"<h5>Workload: 42 minutes</h5>");
    }
    
    [Test]
    public void Render_LearningSpacesAndElementsInWorld_CorrectDraggableForEachObject()
    {
        var space1 = Substitute.For<ILearningSpaceViewModel>();
        var space2 = Substitute.For<ILearningSpaceViewModel>();
        var learningSpaces = new List<ILearningSpaceViewModel> { space1, space2 };
        var element1 = Substitute.For<ILearningElementViewModel>();
        var element2 = Substitute.For<ILearningElementViewModel>();
        var element3 = Substitute.For<ILearningElementViewModel>();
        var learningElements = new List<ILearningElementViewModel> { element1, element2, element3 };
        var objects = new List<ILearningObjectViewModel> { space1, element1, element2, element3, space2 };
        
        var learningWorld = Substitute.For<ILearningWorldViewModel>();
        learningWorld.LearningObjects.Returns(objects);
        _worldPresenter.LearningWorldVm.Returns(learningWorld);
        
        var systemUnderTest = GetLearningWorldViewForTesting();

        var draggableLearningSpaces = systemUnderTest.FindComponentsOrFail<Stub<DraggableLearningSpace>>().ToList();
        var draggableLearningElements = systemUnderTest.FindComponentsOrFail<Stub<DraggableLearningElement>>().ToList();
        
        Assert.Multiple(() =>
        {
            Assert.That(draggableLearningElements, Has.Count.EqualTo(learningElements.Count));
            Assert.That(learningElements.All(le =>
                draggableLearningElements.Any(dle =>
                    dle.Instance.Parameters[nameof(DraggableLearningElement.LearningElement)] == le)));
            Assert.That(draggableLearningSpaces, Has.Count.EqualTo(learningSpaces.Count));
            Assert.That(learningSpaces.All(le =>
                draggableLearningSpaces.Any(dle =>
                    dle.Instance.Parameters[nameof(DraggableLearningSpace.LearningSpace)] == le)));
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
    public void CreateLearningElementDialog_FlagSet_CallsFactory_RendersRenderFragment_CallsPresenterOnDialogClose()
    {
        //prepare presenter
        var learningWorld = Substitute.For<ILearningWorldViewModel>();
        var learningSpaces = Array.Empty<ILearningSpaceViewModel>();
        learningWorld.Name.Returns("my learning world");
        learningWorld.LearningSpaces.Returns(learningSpaces);
        _worldPresenter.LearningWorldVm.Returns(learningWorld);
        _worldPresenter.CreateLearningElementDialogOpen.Returns(true);
        var content = new LearningContentViewModel("foo", "bar", Array.Empty<byte>());
        _worldPresenter.DragAndDropLearningContent.Returns(content);
        
        RenderFragment fragment = builder =>
        {
            builder.OpenElement(0, "p");
            builder.AddContent(1, "foobar");
            builder.CloseElement();
        };
        ModalDialogOnClose? callback = null;
        _modalDialogFactory
            .GetCreateLearningElementFragment(content, learningWorld.LearningSpaces,
                learningWorld.Name, Arg.Any<ModalDialogOnClose>())
            .Returns(fragment)
            .AndDoes(ci =>
            {
                callback = ci.Arg<ModalDialogOnClose>();
            });
        
        var systemUnderTest = GetLearningWorldViewForTesting();

        _modalDialogFactory.Received().GetCreateLearningElementFragment(content, learningWorld.LearningSpaces,
            learningWorld.Name, Arg.Any<ModalDialogOnClose>());
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
        _worldPresenter.Received().OnCreateElementDialogClose(returnValue);
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
    public void EditLearningElementDialog_FlagSet_CallsFactory_RendersRenderFragment_CallsPresenterOnDialogClose()
    {
        //prepare presenter
        var learningWorld = Substitute.For<ILearningWorldViewModel>();
        _worldPresenter.LearningWorldVm.Returns(learningWorld);
        _worldPresenter.EditLearningElementDialogOpen.Returns(true);
        var initialValues = new Dictionary<string, string>
        {
            {"baba", "bubu"}
        };
        _worldPresenter.EditElementDialogInitialValues.Returns(initialValues);
        
        RenderFragment fragment = builder =>
        {
            builder.OpenElement(0, "p");
            builder.AddContent(1, "foobar");
            builder.CloseElement();
        };
        ModalDialogOnClose? callback = null;
        _modalDialogFactory
            .GetEditLearningElementFragment(initialValues, Arg.Any<ModalDialogOnClose>())
            .Returns(fragment)
            .AndDoes(ci =>
            {
                callback = ci.Arg<ModalDialogOnClose>();
            });
        
        var systemUnderTest = GetLearningWorldViewForTesting();

        _modalDialogFactory.Received().GetEditLearningElementFragment(initialValues, Arg.Any<ModalDialogOnClose>());
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
        _worldPresenter.Received().OnEditElementDialogClose(returnValue);
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
        
        _mouseService.Received().FireOut(systemUnderTest.Instance, null);
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
    public void AddElementButton_Clicked_CallsAddNewLearningElement()
    {
        var systemUnderTest = GetLearningWorldViewForTesting();

        var addElementButton = systemUnderTest.FindOrFail("button.btn.btn-primary.add-learning-element");
        addElementButton.Click();
        _worldPresenter.Received().AddNewLearningElement();
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
    }
    
    [Test]
    public void LoadElementButton_Clicked_CallsLoadLearningElementAsync()
    {
        var systemUnderTest = GetLearningWorldViewForTesting();
        
        var loadElementButton = systemUnderTest.FindOrFail("button.btn.btn-primary.load-learning-element");
        loadElementButton.Click();
        _worldPresenter.Received().LoadLearningElementAsync();
    }
    
    [Test]
    public void LoadElementButton_Clicked_OperationCancelledExceptionCaught()
    {
        _worldPresenter.LoadLearningElementAsync().Throws<OperationCanceledException>();
        
        var systemUnderTest = GetLearningWorldViewForTesting();
        
        var loadElementButton = systemUnderTest.FindOrFail("button.btn.btn-primary.load-learning-element");
        Assert.That(() => loadElementButton.Click(), Throws.Nothing);
        _worldPresenter.Received().LoadLearningElementAsync();
    }
    
    [Test]
    public void LoadElementButton_Clicked_OtherExceptionsWrappedInErrorState()
    {
        _worldPresenter.LoadLearningSpaceAsync().Throws(new Exception("saatana"));
        
        var systemUnderTest = GetLearningWorldViewForTesting();
        
        var loadElementButton = systemUnderTest.FindOrFail("button.btn.btn-primary.load-learning-element");
        Assert.That(() => loadElementButton.Click(), Throws.Nothing);
        _worldPresenter.Received().LoadLearningElementAsync();
    }
    
    [Test]
    public void EditObjectButton_Clicked_CallsOpenEditSelectedLearningObjectDialog()
    {
        var element = Substitute.For<ILearningElementViewModel>();
        var worldVm = Substitute.For<ILearningWorldViewModel>();
        worldVm.LearningElements.Returns(new List<ILearningElementViewModel> { element });
        worldVm.SelectedLearningObject.Returns(element);
        _worldPresenter.LearningWorldVm.Returns(worldVm);

        var systemUnderTest = GetLearningWorldViewForTesting();
        
        var editObjectButton = systemUnderTest.FindOrFail("button.btn.btn-primary.edit-learning-object");
        editObjectButton.Click();
        _worldPresenter.Received().OpenEditSelectedLearningObjectDialog();
    }

    [Test]
    public void DeleteObjectButton_Clicked_CallsDeleteSelectedLearningObject()
    {
        var element = Substitute.For<ILearningElementViewModel>();
        var worldVm = Substitute.For<ILearningWorldViewModel>();
        worldVm.LearningElements.Returns(new List<ILearningElementViewModel> { element });
        worldVm.SelectedLearningObject.Returns(element);
        _worldPresenter.LearningWorldVm.Returns(worldVm);

        var systemUnderTest = GetLearningWorldViewForTesting();
        
        var editObjectButton = systemUnderTest.FindOrFail("button.btn.btn-primary.delete-learning-object");
        editObjectButton.Click();
        _worldPresenter.Received().DeleteSelectedLearningObject();
    }
    
    [Test]
    public void SaveObjectButton_Clicked_CallsSaveSelectedLearningObjectAsync()
    {
        var systemUnderTest = GetLearningWorldViewForTesting();
        
        var loadElementButton = systemUnderTest.FindOrFail("button.btn.btn-primary.save-learning-object");
        loadElementButton.Click();
        _worldPresenter.Received().SaveSelectedLearningObjectAsync();
    }
    
    [Test]
    public void SaveObjectButton_Clicked_OperationCancelledExceptionCaught()
    {
        _worldPresenter.SaveSelectedLearningObjectAsync().Throws<OperationCanceledException>();
        
        var systemUnderTest = GetLearningWorldViewForTesting();
        
        var loadElementButton = systemUnderTest.FindOrFail("button.btn.btn-primary.save-learning-object");
        Assert.That(() => loadElementButton.Click(), Throws.Nothing);
        _worldPresenter.Received().SaveSelectedLearningObjectAsync();
    }
    
    [Test]
    public void SaveObjectButton_Clicked_OtherExceptionsWrappedInErrorState()
    {
        _worldPresenter.SaveSelectedLearningObjectAsync().Throws(new Exception("saatana"));
        
        var systemUnderTest = GetLearningWorldViewForTesting();
        
        var loadElementButton = systemUnderTest.FindOrFail("button.btn.btn-primary.save-learning-object");
        Assert.That(() => loadElementButton.Click(), Throws.Nothing);
        _worldPresenter.Received().SaveSelectedLearningObjectAsync();
    }
    
    [Test]
    public void ShowSelectedSpace_Called_CallsShowSelectedLearningSpaceView()
    {
        var element = Substitute.For<ILearningElementViewModel>();
        var worldVm = Substitute.For<ILearningWorldViewModel>();
        worldVm.LearningElements.Returns(new List<ILearningElementViewModel> { element });
        worldVm.SelectedLearningObject.Returns(element);
        _worldPresenter.LearningWorldVm.Returns(worldVm);

        var systemUnderTest = GetLearningWorldViewForTesting();
        
        var editObjectButton = systemUnderTest.FindOrFail("button.btn.btn-primary.show-learning-space");
        editObjectButton.Click();
        _worldPresenter.Received().ShowSelectedLearningSpaceView();
    }
    
    private IRenderedComponent<LearningWorldView> GetLearningWorldViewForTesting(RenderFragment? childContent = null)
    {
        return _ctx.RenderComponent<LearningWorldView>(parameters => parameters
            .Add(p => p.ChildContent, childContent));
    }
}