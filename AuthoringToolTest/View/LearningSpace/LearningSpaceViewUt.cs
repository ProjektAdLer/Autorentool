using System;
using System.Collections.Generic;
using AngleSharp.Dom;
using AuthoringTool.Components.ModalDialog;
using AuthoringTool.PresentationLogic;
using AuthoringTool.PresentationLogic.AuthoringToolWorkspace;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.ModalDialog;
using AuthoringTool.View.LearningSpace;
using AuthoringTool.View.Shared;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using TestContext = Bunit.TestContext;

namespace AuthoringToolTest.View.LearningSpace;

[TestFixture]
public class LearningSpaceViewUt
{
#pragma warning disable CS8618 set in setup - n.stich
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
        var systemUnderTest = GetFragmentForTesting();
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.MouseService, Is.EqualTo(_mouseService));
            Assert.That(systemUnderTest.Instance.LearningSpaceP, Is.EqualTo(_learningSpacePresenter));
            Assert.That(systemUnderTest.Instance.ModalDialogFactory, Is.EqualTo(_modalDialogFactory));
        });
    }

    [Test]
    public void Constructor_RendersChildContent()
    {
        RenderFragment childContent = builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "barbaz");
            builder.AddContent(2, "foobar");
            builder.CloseElement();
        };
        
        var systemUnderTest = GetFragmentForTesting(childContent);

        Assert.That(systemUnderTest.FindOrFail("div.barbaz").TextContent, Is.EqualTo("foobar"));
    }

    [Test]
    public void Render_LearningSpaceSet_RendersNameAndWorkload()
    {
        var learningSpace = Substitute.For<ILearningSpaceViewModel>();
        learningSpace.Name.Returns("foobar");
        learningSpace.Workload.Returns(42);
        _learningSpacePresenter.LearningSpaceVm.Returns(learningSpace);
        
        var systemUnderTest = GetFragmentForTesting();

        var nameHeader = systemUnderTest.FindOrFail("h2");
        var workloadHeader = systemUnderTest.FindOrFail("h5");
        
        nameHeader.MarkupMatches("<h2>LearningSpace foobar</h2>");
        workloadHeader.MarkupMatches("<h5>Workload: 42 minutes</h5>");
    }

    [Test]
    public void Render_LearningObjectSelected_RendersLearningObjectSection()
    {
        var learningSpace = Substitute.For<ILearningSpaceViewModel>();
        var learningObject = Substitute.For<ILearningObjectViewModel>();
        learningSpace.SelectedLearningObject.Returns(learningObject);
        learningObject.Name.Returns("my secret name");
        learningObject.Description.Returns("a super long description");
        _learningSpacePresenter.LearningSpaceVm.Returns(learningSpace);
        
        var systemUnderTest = GetFragmentForTesting();

        var label = systemUnderTest.FindOrFail("label");
        var editButton = systemUnderTest.FindOrFail("button.btn.btn-primary.edit-learning-object");
        var deleteButton = systemUnderTest.FindOrFail("button.btn.btn-primary.delete-learning-object");
        var saveButton = systemUnderTest.FindOrFail("button.btn.btn-primary.save-learning-object");
        
        label.MarkupMatches("<label>Selected element: my secret name, Description: a super long description</label>");
        editButton.MarkupMatches(@"<button class=""btn btn-primary edit-learning-object"">Edit selected Learning Object</button>");
        deleteButton.MarkupMatches(@"<button class=""btn btn-primary delete-learning-object"">Delete Learning Object</button>");
        saveButton.MarkupMatches(@"<button class=""btn btn-primary save-learning-object"">Save selected Learning Object</button>");
    }

    [Test]
    public void Render_LearningElementsInSpace_RendersDraggableWithInnerComponent()
    {
        //TODO: implement with ComponentFactories replaced https://bunit.dev/docs/providing-input/controlling-component-instantiation
        
    }

    [Test]
    public void Svg_MouseMove_CallsMouseService()
    {
        var mouseEventArgs = new MouseEventArgs();
        var systemUnderTest = GetFragmentForTesting();

        var svg = systemUnderTest.FindOrFail("svg");
        svg.MouseMove(mouseEventArgs);
        
        _mouseService.Received().FireMove(systemUnderTest.Instance, mouseEventArgs);
    }

    [Test]
    public void Svg_MouseUp_CallsMouseService()
    {
        var mouseEventArgs = new MouseEventArgs();
        var systemUnderTest = GetFragmentForTesting();

        var svg = systemUnderTest.FindOrFail("svg");
        svg.MouseUp(mouseEventArgs);
        
        _mouseService.Received().FireUp(systemUnderTest.Instance, mouseEventArgs);
    }

    [Test]
    public void Svg_MouseLeave_CallsMouseService()
    {
        var mouseEventArgs = new MouseEventArgs();
        var systemUnderTest = GetFragmentForTesting();

        var svg = systemUnderTest.FindOrFail("svg");
        svg.MouseLeave(mouseEventArgs);
        
        _mouseService.Received().FireOut(systemUnderTest.Instance, null);
    }

    [Test]
    public void EditLearningSpaceDialog_FlagSet_CallsFactory_RendersRenderFragment_CallsPresenterOnDialogClose()
    {
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

        ModalDialogOnClose? callback = null;
        _modalDialogFactory
            .GetEditLearningSpaceFragment(initialValues, Arg.Any<ModalDialogOnClose>())
            .Returns(fragment)
            .AndDoes(ci =>
            {
                callback = ci.Arg<ModalDialogOnClose>();
            });

        var systemUnderTest = GetFragmentForTesting();
        
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
        callback!.Invoke(returnValue);
        
        _learningSpacePresenter.Received().OnEditSpaceDialogClose(returnValue);
    }
    

    private IRenderedComponent<LearningSpaceView> GetFragmentForTesting(RenderFragment? childContent = null)
    {
        childContent ??= delegate {  };
        return _ctx.RenderComponent<LearningSpaceView>(
            parameters => parameters
                .Add(p => p.ChildContent, childContent)
            );
    }
}