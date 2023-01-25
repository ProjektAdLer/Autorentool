using System;
using System.Collections.Generic;
using System.Linq;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.ModalDialog;
using Presentation.View.LearningElement;
using Presentation.View.LearningSpace;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.LearningSpace;

[TestFixture]
public class LearningSpaceViewUt
{
#pragma warning disable CS8618 // set in setup - n.stich
    private TestContext _ctx;
    private ILearningSpacePresenter _learningSpacePresenter;
    private IMouseService _mouseService;
    private ILearningSpaceViewModalDialogFactory _modalDialogFactory;
#pragma warning restore CS8618

    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _learningSpacePresenter = Substitute.For<ILearningSpacePresenter>();
        _mouseService = Substitute.For<IMouseService>();
        _modalDialogFactory = Substitute.For<ILearningSpaceViewModalDialogFactory>();
        _ctx.Services.AddSingleton(_learningSpacePresenter);
        _ctx.Services.AddSingleton(_mouseService);
        _ctx.Services.AddSingleton(_modalDialogFactory);
        _ctx.Services.AddLogging();
    }
    
    [Test]
    public void Constructor_InjectsDependencies()
    {
        var systemUnderTest = GetLearningSpaceViewForTesting();
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.MouseService, Is.EqualTo(_mouseService));
            Assert.That(systemUnderTest.Instance.LearningSpaceP, Is.EqualTo(_learningSpacePresenter));
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
        
        var systemUnderTest = GetLearningSpaceViewForTesting(childContent);

        Assert.That(systemUnderTest.FindOrFail("div.barbaz").TextContent, Is.EqualTo("foobar"));
    }

    [Test]
    public void Render_LearningSpaceSet_RendersNameAndWorkload()
    {
        var learningSpace = Substitute.For<ILearningSpaceViewModel>();
        learningSpace.Name.Returns("foobar");
        learningSpace.Workload.Returns(42);
        learningSpace.Points.Returns(8);
        _learningSpacePresenter.LearningSpaceVm.Returns(learningSpace);
        
        var systemUnderTest = GetLearningSpaceViewForTesting();

        var nameHeader = systemUnderTest.FindOrFail("h2");
        var workloadPointsHeader = systemUnderTest.FindAll("h5");
        
        nameHeader.MarkupMatches("<h2>LearningSpace foobar</h2>");
        workloadPointsHeader[0].MarkupMatches("<h5>Workload: 42 minutes</h5>");
        workloadPointsHeader[1].MarkupMatches("<h5>Points: 8</h5>");
    }

    [Test]
    public void Render_LearningObjectSelected_RendersLearningObjectSection()
    {
        var learningSpace = Substitute.For<ILearningSpaceViewModel>();
        var learningObject = Substitute.For<ILearningElementViewModel>();
        learningSpace.SelectedLearningElement.Returns(learningObject);
        learningObject.Name.Returns("my secret name");
        learningObject.Description.Returns("a super long description");
        _learningSpacePresenter.LearningSpaceVm.Returns(learningSpace);
        
        var systemUnderTest = GetLearningSpaceViewForTesting();

        var label = systemUnderTest.FindOrFail("label");
        var editButton = systemUnderTest.FindOrFail("button.btn.btn-primary.edit-learning-element");
        var deleteButton = systemUnderTest.FindOrFail("button.btn.btn-primary.delete-learning-element");
        var saveButton = systemUnderTest.FindOrFail("button.btn.btn-primary.save-learning-element");
        
        label.MarkupMatches(@"<label id=""learning-element-info"">Selected element: my secret name, Description: a super long description</label>");
        editButton.MarkupMatches(@"<button class=""btn btn-primary edit-learning-element"">Edit selected Learning Element</button>");
        deleteButton.MarkupMatches(@"<button class=""btn btn-primary delete-learning-element"">Delete Learning Element</button>");
        saveButton.MarkupMatches(@"<button class=""btn btn-primary save-learning-element"">Save selected Learning Element</button>");
    }

    [Test]
    public void Render_NoLearningObjectSelected_DoesNotRenderLearningObjectSection()
    {
        _learningSpacePresenter.LearningSpaceVm.Returns((LearningSpaceViewModel?)null);
        Assert.That(_learningSpacePresenter.LearningSpaceVm, Is.Null);
        
        var systemUnderTest = GetLearningSpaceViewForTesting();
        Assert.Multiple(() =>
        {
            Assert.That(() => systemUnderTest.Find("label.learning-object-info"), Throws.TypeOf<ElementNotFoundException>());
            Assert.That(() => systemUnderTest.Find("button.btn.btn-primary.edit-learning-object"),
                Throws.TypeOf<ElementNotFoundException>());
            Assert.That(() => systemUnderTest.Find("button.btn.btn-primary.delete-learning-object"),
                Throws.TypeOf<ElementNotFoundException>());
            Assert.That(() => systemUnderTest.Find("button.btn.btn-primary.save-learning-object"),
                Throws.TypeOf<ElementNotFoundException>());
        });
    }

    [Test]
    public void Svg_MouseMove_CallsMouseService()
    {
        var mouseEventArgs = new MouseEventArgs();
        var systemUnderTest = GetLearningSpaceViewForTesting();

        var svg = systemUnderTest.FindOrFail("svg");
        svg.MouseMove(mouseEventArgs);
        
        _mouseService.Received().FireMove(systemUnderTest.Instance, mouseEventArgs);
    }

    [Test]
    public void Svg_MouseUp_CallsMouseService()
    {
        var mouseEventArgs = new MouseEventArgs();
        var systemUnderTest = GetLearningSpaceViewForTesting();

        var svg = systemUnderTest.FindOrFail("svg");
        svg.MouseUp(mouseEventArgs);
        
        _mouseService.Received().FireUp(systemUnderTest.Instance, mouseEventArgs);
    }

    [Test]
    public void Svg_MouseLeave_CallsMouseService()
    {
        var mouseEventArgs = new MouseEventArgs();
        var systemUnderTest = GetLearningSpaceViewForTesting();

        var svg = systemUnderTest.FindOrFail("svg");
        svg.MouseLeave(mouseEventArgs);
        
        _mouseService.Received().FireOut(systemUnderTest.Instance, mouseEventArgs);
    }

    [Test]
    public void EditLearningSpaceDialog_FlagSet_CallsFactory_RendersRenderFragment_CallsPresenterOnDialogClose()
    {
        //prepare presenter return values
        var learningSpace = Substitute.For<ILearningSpaceViewModel>();
        _learningSpacePresenter.EditLearningSpaceDialogOpen.Returns(true);
        _learningSpacePresenter.LearningSpaceVm.Returns(learningSpace);
        var initialValues = new Dictionary<string, string>
        {
            { "foo", "bar" }
        };
        _learningSpacePresenter.EditLearningSpaceDialogInitialValues.Returns(initialValues);

        RenderFragment fragment = builder =>
        {
            builder.OpenElement(0, "p");
            builder.AddContent(1, "foobar");
            builder.CloseElement();
        };
        //prepare dialog factory return value and capture passed callback
        ModalDialogOnClose? callback = null;
        _modalDialogFactory
            .GetEditLearningSpaceFragment(initialValues, Arg.Any<ModalDialogOnClose>())
            .Returns(fragment)
            .AndDoes(ci =>
            {
                callback = ci.Arg<ModalDialogOnClose>();
            });

        //create system under test which will immediately render, therefore execute
        var systemUnderTest = GetLearningSpaceViewForTesting();
        
        //assert dialogFactory was called and that our dialog fragment was rendered
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
        _learningSpacePresenter.Received().OnEditSpaceDialogClose(returnValue);
    }
    
    [Test]
    public void CreateLearningElementDialogOpen_FlagSet_CallsFactory_RendersRenderFragment_CallsPresenterOnDialogClose()
    {
        _learningSpacePresenter.CreateLearningElementDialogOpen.Returns(true);
        var contentMock = new LearningContentViewModel("foo", "bla", "");
        _learningSpacePresenter.DragAndDropLearningContent.Returns(contentMock);
        var space = Substitute.For<ILearningSpaceViewModel>();
        space.Name = "spacename";
        _learningSpacePresenter.LearningSpaceVm.Returns(space);

        RenderFragment fragment = builder =>
        {
            builder.OpenElement(0, "p");
            builder.AddContent(1, "foobar");
            builder.CloseElement();
        };

        ModalDialogOnClose? callback = null;
        _modalDialogFactory
            .GetCreateLearningElementFragment(contentMock, Arg.Any<ModalDialogOnClose>())
            .Returns(fragment)
            .AndDoes(ci =>
            {
                callback = ci.Arg<ModalDialogOnClose>();
            });

        var systemUnderTest = GetLearningSpaceViewForTesting();
        
        _modalDialogFactory.Received().GetCreateLearningElementFragment(contentMock, Arg.Any<ModalDialogOnClose>());
        var p = systemUnderTest.FindOrFail("p");
        p.MarkupMatches("<p>foobar</p>");
        
        if (callback == null)
        {
            Assert.Fail("Didn't get a callback from call to modal dialog factory");
        }
        
        var returnValue = new ModalDialogOnCloseResult(ModalDialogReturnValue.Ok);
        
        callback!.Invoke(returnValue);
        
        _learningSpacePresenter.Received().OnCreateElementDialogClose(returnValue);
        
    }

    [Test]
    public void EditLearningElementDialogOpen_FlagSet_CallsFactory_RendersRenderFragment_CallsPresenterOnDialogClose()
    {
        //prepare presenter return values
        var learningSpace = Substitute.For<ILearningSpaceViewModel>();
        _learningSpacePresenter.EditLearningElementDialogOpen.Returns(true);
        _learningSpacePresenter.LearningSpaceVm.Returns(learningSpace);
        var initialValues = new Dictionary<string, string>
        {
            { "foo", "bar" }
        };
        _learningSpacePresenter.EditLearningElementDialogInitialValues.Returns(initialValues);

        RenderFragment fragment = builder =>
        {
            builder.OpenElement(0, "p");
            builder.AddContent(1, "foobar");
            builder.CloseElement();
        };
        //prepare dialog factory return value and capture passed callback
        ModalDialogOnClose? callback = null;
        _modalDialogFactory
            .GetEditLearningElementFragment(initialValues, Arg.Any<ModalDialogOnClose>())
            .Returns(fragment)
            .AndDoes(ci =>
            {
                callback = ci.Arg<ModalDialogOnClose>();
            });
        
        //create system under test which will immediately render, therefore execute
        var systemUnderTest = GetLearningSpaceViewForTesting();
        
        //assert dialogFactory was called and that our dialog fragment was rendered
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
        _learningSpacePresenter.Received().OnEditElementDialogClose(returnValue);
    }

    [Test]
    public void AddElementButton_Clicked_CallsAddNewLearningElement()
    {
        var systemUnderTest = GetLearningSpaceViewForTesting();
        
        var button = systemUnderTest.FindOrFail("button.btn.btn-primary.add-learning-element");
        button.Click();
        _learningSpacePresenter.Received().AddNewLearningElement();
    }

    [Test]
    public void EditObjectButton_Clicked_CallsEditSelectedLearningObject()
    {
        var space = Substitute.For<ILearningSpaceViewModel>();
        space.SelectedLearningElement.Returns(Substitute.For<ILearningElementViewModel>());
        _learningSpacePresenter.LearningSpaceVm.Returns(space);
        var systemUnderTest = GetLearningSpaceViewForTesting();
        
        var button = systemUnderTest.FindOrFail("button.btn.btn-primary.edit-learning-element");
        button.Click();
        _learningSpacePresenter.Received().EditSelectedLearningElement();
        
    }
    
    [Test]
    public void DeleteObjectButton_Clicked_CallsDeleteSelectedLearningObject()
    {
        var space = Substitute.For<ILearningSpaceViewModel>();
        space.SelectedLearningElement.Returns(Substitute.For<ILearningElementViewModel>());
        _learningSpacePresenter.LearningSpaceVm.Returns(space);
        var systemUnderTest = GetLearningSpaceViewForTesting();
        
        var button = systemUnderTest.FindOrFail("button.btn.btn-primary.delete-learning-element");
        button.Click();
        _learningSpacePresenter.Received().DeleteSelectedLearningElement();
        
    }

    [Test]
    public void LoadElementButton_Clicked_CallsLoadLearningElementAsync()
    {
        var systemUnderTest = GetLearningSpaceViewForTesting();
        
        var button = systemUnderTest.Find("button.btn.btn-primary.load-learning-element");
        button.Click();
        _learningSpacePresenter.Received().LoadLearningElementAsync(0);
    }
    
    [Test]
    public void LoadElementButton_Clicked_OperationCanceledExceptionCaught()
    {
        _learningSpacePresenter.LoadLearningElementAsync(0).Throws(new OperationCanceledException());
        
        var systemUnderTest = GetLearningSpaceViewForTesting();
        
        var button = systemUnderTest.Find("button.btn.btn-primary.load-learning-element");
        Assert.That(() => button.Click(), Throws.Nothing);
        _learningSpacePresenter.Received().LoadLearningElementAsync(0);
    }
    
    [Test]
    public void LoadElementButton_Clicked_OtherExceptionsWrappedInErrorState()
    {
        var ex = new Exception();
        _learningSpacePresenter.LoadLearningElementAsync(0).Throws(ex);
        
        var systemUnderTest = GetLearningSpaceViewForTesting();
        
        var button = systemUnderTest.Find("button.btn.btn-primary.load-learning-element");
        Assert.Multiple(() =>
        {
            Assert.That(() => button.Click(), Throws.Nothing);
            Assert.That(systemUnderTest.Instance.ErrorState, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.ErrorState!.CallSite, Is.EqualTo("Load learning element"));
            Assert.That(systemUnderTest.Instance.ErrorState!.Exception, Is.EqualTo(ex));
        });
        _learningSpacePresenter.Received().LoadLearningElementAsync(0);
    }

    [Test]
    public void SaveObjectButton_Clicked_CallsSaveSelectedLearningObjectAsync()
    {
        var systemUnderTest = GetLearningSpaceViewForTesting();
        
        var button = systemUnderTest.Find("button.btn.btn-primary.save-learning-element");
        button.Click();
        _learningSpacePresenter.Received().SaveSelectedLearningElementAsync();
    }
    
    [Test]
    public void SaveObjectButton_Clicked_OperationCanceledExceptionCaught()
    {
        _learningSpacePresenter.SaveSelectedLearningElementAsync().Throws(new OperationCanceledException());
        
        var systemUnderTest = GetLearningSpaceViewForTesting();
        
        var button = systemUnderTest.Find("button.btn.btn-primary.save-learning-element");
        Assert.That(() => button.Click(), Throws.Nothing);
        _learningSpacePresenter.Received().SaveSelectedLearningElementAsync();
    }
    
    [Test]
    public void SaveObjectButton_Clicked_OtherExceptionsWrappedInErrorState()
    {
        var ex = new Exception();
        _learningSpacePresenter.SaveSelectedLearningElementAsync().Throws(ex);
        
        var systemUnderTest = GetLearningSpaceViewForTesting();
        
        var button = systemUnderTest.Find("button.btn.btn-primary.save-learning-element");
        Assert.Multiple(() =>
        {
            Assert.That(() => button.Click(), Throws.Nothing);
            Assert.That(systemUnderTest.Instance.ErrorState, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.ErrorState!.CallSite, Is.EqualTo("Save learning element"));
            Assert.That(systemUnderTest.Instance.ErrorState!.Exception, Is.EqualTo(ex));
        });
        _learningSpacePresenter.Received().SaveSelectedLearningElementAsync();
    }
    
    [Test]
    public void ShowElementContentButton_Clicked_CallsShowSelectedElementContentAsync()
    {
        var systemUnderTest = GetLearningSpaceViewForTesting();
        
        var button = systemUnderTest.Find("button.btn.btn-primary.show-element-content");
        button.Click();
        _learningSpacePresenter.Received().ShowSelectedElementContentAsync();
    }
    
    [Test]
    public void ShowElementContentButton_Clicked_OperationCanceledExceptionCaught()
    {
        _learningSpacePresenter.ShowSelectedElementContentAsync().Throws(new OperationCanceledException());
        
        var systemUnderTest = GetLearningSpaceViewForTesting();
        
        var button = systemUnderTest.Find("button.btn.btn-primary.show-element-content");
        Assert.That(() => button.Click(), Throws.Nothing);
        _learningSpacePresenter.Received().ShowSelectedElementContentAsync();
    }
    
    [Test]
    public void ShowElementContentButton_Clicked_OtherExceptionsWrappedInErrorState()
    {
        var ex = new Exception();
        _learningSpacePresenter.ShowSelectedElementContentAsync().Throws(ex);
        
        var systemUnderTest = GetLearningSpaceViewForTesting();
        
        var button = systemUnderTest.Find("button.btn.btn-primary.show-element-content");
        Assert.Multiple(() =>
        {
            Assert.That(() => button.Click(), Throws.Nothing);
            Assert.That(systemUnderTest.Instance.ErrorState, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.ErrorState!.CallSite, Is.EqualTo("Show learning element content"));
            Assert.That(systemUnderTest.Instance.ErrorState!.Exception, Is.EqualTo(ex));
        });
        _learningSpacePresenter.Received().ShowSelectedElementContentAsync();
    }
    private IRenderedComponent<LearningSpaceView> GetLearningSpaceViewForTesting(RenderFragment? childContent = null)
    {
        childContent ??= delegate {  };
        return _ctx.RenderComponent<LearningSpaceView>(
            parameters => parameters
                .Add(p => p.ChildContent, childContent)
            );
    }
}