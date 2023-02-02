using System;
using System.Collections.Generic;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.Content;
using Presentation.PresentationLogic.Element;
using Presentation.PresentationLogic.ModalDialog;
using Presentation.PresentationLogic.Space;
using Presentation.View.Space;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.Space;

[TestFixture]
public class SpaceViewUt
{
#pragma warning disable CS8618 // set in setup - n.stich
    private TestContext _ctx;
    private ISpacePresenter _spacePresenter;
    private ISpaceViewModalDialogFactory _modalDialogFactory;
#pragma warning restore CS8618

    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _ctx.Services.AddMudServices();
        _ctx.JSInterop.SetupVoid("mudDragAndDrop.initDropZone", _ => true);
        _spacePresenter = Substitute.For<ISpacePresenter>();
        _modalDialogFactory = Substitute.For<ISpaceViewModalDialogFactory>();
        _ctx.Services.AddSingleton(_spacePresenter);
        _ctx.Services.AddSingleton(_modalDialogFactory);
        _ctx.Services.AddLogging();
    }

    [Test]
    public void Constructor_InjectsDependencies()
    {
        var systemUnderTest = GetSpaceViewForTesting();

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.SpaceP, Is.EqualTo(_spacePresenter));
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

        var systemUnderTest = GetSpaceViewForTesting(childContent);

        Assert.That(systemUnderTest.FindOrFail("div.barbaz").TextContent, Is.EqualTo("foobar"));
    }

    [Test]
    public void Render_SpaceSet_RendersNameAndWorkload()
    {
        var space = Substitute.For<ISpaceViewModel>();
        space.Name.Returns("foobar");
        space.Workload.Returns(42);
        space.Points.Returns(8);
        _spacePresenter.SpaceVm.Returns(space);

        var systemUnderTest = GetSpaceViewForTesting();

        var nameHeader = systemUnderTest.FindOrFail("h2");
        var workloadPointsHeader = systemUnderTest.FindAll("h5");

        nameHeader.MarkupMatches("<h2>Space foobar</h2>");
        workloadPointsHeader[0].MarkupMatches("<h5>Workload: 42 minutes</h5>");
        workloadPointsHeader[1].MarkupMatches("<h5>Points: 8</h5>");
    }

    [Test]
    public void Render_ObjectSelected_RendersObjectSection()
    {
        var space = Substitute.For<ISpaceViewModel>();
        var elementViewModel = Substitute.For<IElementViewModel>();
        space.SelectedElement.Returns(elementViewModel);
        elementViewModel.Name.Returns("my secret name");
        elementViewModel.Description.Returns("a super long description");
        _spacePresenter.SpaceVm.Returns(space);

        var systemUnderTest = GetSpaceViewForTesting();

        var label = systemUnderTest.FindOrFail("label");
        var editButton = systemUnderTest.FindOrFail("button.btn.btn-primary.edit-element");
        var deleteButton = systemUnderTest.FindOrFail("button.btn.btn-primary.delete-element");
        var saveButton = systemUnderTest.FindOrFail("button.btn.btn-primary.save-element");
        
        label.MarkupMatches(@"<label id=""element-info"">Selected element: my secret name, Description: a super long description</label>");
        editButton.MarkupMatches(@"<button class=""btn btn-primary edit-element"">Edit selected Element</button>");
        deleteButton.MarkupMatches(@"<button class=""btn btn-primary delete-element"">Delete Element</button>");
        saveButton.MarkupMatches(@"<button class=""btn btn-primary save-element"">Save selected Element</button>");
    }

    [Test]
    public void Render_NoObjectSelected_DoesNotRenderObjectSection()
    {
        _spacePresenter.SpaceVm.Returns((SpaceViewModel?)null);
        Assert.That(_spacePresenter.SpaceVm, Is.Null);

        var systemUnderTest = GetSpaceViewForTesting();
        Assert.Multiple(() =>
        {
            Assert.That(() => systemUnderTest.Find("label.object-info"), Throws.TypeOf<ElementNotFoundException>());
            Assert.That(() => systemUnderTest.Find("button.btn.btn-primary.edit-object"),
                Throws.TypeOf<ElementNotFoundException>());
            Assert.That(() => systemUnderTest.Find("button.btn.btn-primary.delete-object"),
                Throws.TypeOf<ElementNotFoundException>());
            Assert.That(() => systemUnderTest.Find("button.btn.btn-primary.save-object"),
                Throws.TypeOf<ElementNotFoundException>());
        });
    }

    [Test]
    public void EditSpaceDialog_FlagSet_CallsFactory_RendersRenderFragment_CallsPresenterOnDialogClose()
    {
        //prepare presenter return values
        var space = Substitute.For<ISpaceViewModel>();
        _spacePresenter.EditSpaceDialogOpen.Returns(true);
        _spacePresenter.SpaceVm.Returns(space);
        var initialValues = new Dictionary<string, string>
        {
            { "foo", "bar" }
        };
        _spacePresenter.EditSpaceDialogInitialValues.Returns(initialValues);

        RenderFragment fragment = builder =>
        {
            builder.OpenElement(0, "p");
            builder.AddContent(1, "foobar");
            builder.CloseElement();
        };
        //prepare dialog factory return value and capture passed callback
        ModalDialogOnClose? callback = null;
        _modalDialogFactory
            .GetEditSpaceFragment(initialValues, Arg.Any<ModalDialogOnClose>())
            .Returns(fragment)
            .AndDoes(ci =>
            {
                callback = ci.Arg<ModalDialogOnClose>();
            });

        //create system under test which will immediately render, therefore execute
        var systemUnderTest = GetSpaceViewForTesting();

        //assert dialogFactory was called and that our dialog fragment was rendered
        _modalDialogFactory.Received().GetEditSpaceFragment(initialValues, Arg.Any<ModalDialogOnClose>());
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
        _spacePresenter.Received().OnEditSpaceDialogClose(returnValue);
    }

    [Test]
    public void CreateElementDialogOpen_FlagSet_CallsFactory_RendersRenderFragment_CallsPresenterOnDialogClose()
    {
        _spacePresenter.CreateElementDialogOpen.Returns(true);
        var contentMock = new ContentViewModel("foo", "bla", "");
        _spacePresenter.DragAndDropContent.Returns(contentMock);
        var space = Substitute.For<ISpaceViewModel>();
        space.Name = "spacename";
        _spacePresenter.SpaceVm.Returns(space);

        RenderFragment fragment = builder =>
        {
            builder.OpenElement(0, "p");
            builder.AddContent(1, "foobar");
            builder.CloseElement();
        };

        ModalDialogOnClose? callback = null;
        _modalDialogFactory
            .GetCreateElementFragment(contentMock, Arg.Any<ModalDialogOnClose>())
            .Returns(fragment)
            .AndDoes(ci =>
            {
                callback = ci.Arg<ModalDialogOnClose>();
            });

        var systemUnderTest = GetSpaceViewForTesting();

        _modalDialogFactory.Received().GetCreateElementFragment(contentMock, Arg.Any<ModalDialogOnClose>());
        var p = systemUnderTest.FindOrFail("p");
        p.MarkupMatches("<p>foobar</p>");

        if (callback == null)
        {
            Assert.Fail("Didn't get a callback from call to modal dialog factory");
        }

        var returnValue = new ModalDialogOnCloseResult(ModalDialogReturnValue.Ok);

        callback!.Invoke(returnValue);

        _spacePresenter.Received().OnCreateElementDialogClose(returnValue);
    }

    [Test]
    public void EditElementDialogOpen_FlagSet_CallsFactory_RendersRenderFragment_CallsPresenterOnDialogClose()
    {
        //prepare presenter return values
        var space = Substitute.For<ISpaceViewModel>();
        _spacePresenter.EditElementDialogOpen.Returns(true);
        _spacePresenter.SpaceVm.Returns(space);
        var initialValues = new Dictionary<string, string>
        {
            { "foo", "bar" }
        };
        _spacePresenter.EditElementDialogInitialValues.Returns(initialValues);

        RenderFragment fragment = builder =>
        {
            builder.OpenElement(0, "p");
            builder.AddContent(1, "foobar");
            builder.CloseElement();
        };
        //prepare dialog factory return value and capture passed callback
        ModalDialogOnClose? callback = null;
        _modalDialogFactory
            .GetEditElementFragment(initialValues, Arg.Any<ModalDialogOnClose>())
            .Returns(fragment)
            .AndDoes(ci =>
            {
                callback = ci.Arg<ModalDialogOnClose>();
            });
        
        //create system under test which will immediately render, therefore execute
        var systemUnderTest = GetSpaceViewForTesting();

        //assert dialogFactory was called and that our dialog fragment was rendered
        _modalDialogFactory.Received().GetEditElementFragment(initialValues, Arg.Any<ModalDialogOnClose>());
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
        _spacePresenter.Received().OnEditElementDialogClose(returnValue);
    }

    [Test]
    public void AddElementButton_Clicked_CallsAddNewElement()
    {
        var systemUnderTest = GetSpaceViewForTesting();

        var button = systemUnderTest.FindOrFail("button.btn.btn-primary.add-element");
        button.Click();
        _spacePresenter.Received().AddNewElement();
    }

    [Test]
    public void EditObjectButton_Clicked_CallsEditSelectedObject()
    {
        var space = Substitute.For<ISpaceViewModel>();
        space.SelectedElement.Returns(Substitute.For<IElementViewModel>());
        _spacePresenter.SpaceVm.Returns(space);
        var systemUnderTest = GetSpaceViewForTesting();

        var button = systemUnderTest.FindOrFail("button.btn.btn-primary.edit-element");
        button.Click();
        _spacePresenter.Received().EditSelectedElement();
    }

    [Test]
    public void DeleteObjectButton_Clicked_CallsDeleteSelectedObject()
    {
        var space = Substitute.For<ISpaceViewModel>();
        space.SelectedElement.Returns(Substitute.For<IElementViewModel>());
        _spacePresenter.SpaceVm.Returns(space);
        var systemUnderTest = GetSpaceViewForTesting();

        var button = systemUnderTest.FindOrFail("button.btn.btn-primary.delete-element");
        button.Click();
        _spacePresenter.Received().DeleteSelectedElement();
    }

    [Test]
    public void LoadElementButton_Clicked_CallsLoadElementAsync()
    {
        var systemUnderTest = GetSpaceViewForTesting();

        var button = systemUnderTest.Find("button.btn.btn-primary.load-element");
        button.Click();
        _spacePresenter.Received().LoadElementAsync(0);
    }

    [Test]
    public void LoadElementButton_Clicked_OperationCanceledExceptionCaught()
    {
        _spacePresenter.LoadElementAsync(0).Throws(new OperationCanceledException());

        var systemUnderTest = GetSpaceViewForTesting();

        var button = systemUnderTest.Find("button.btn.btn-primary.load-element");
        Assert.That(() => button.Click(), Throws.Nothing);
        _spacePresenter.Received().LoadElementAsync(0);
    }

    [Test]
    public void LoadElementButton_Clicked_OtherExceptionsWrappedInErrorState()
    {
        var ex = new Exception();
        _spacePresenter.LoadElementAsync(0).Throws(ex);

        var systemUnderTest = GetSpaceViewForTesting();

        var button = systemUnderTest.Find("button.btn.btn-primary.load-element");
        Assert.Multiple(() =>
        {
            Assert.That(() => button.Click(), Throws.Nothing);
            Assert.That(systemUnderTest.Instance.ErrorState, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.ErrorState!.CallSite, Is.EqualTo("Load element"));
            Assert.That(systemUnderTest.Instance.ErrorState!.Exception, Is.EqualTo(ex));
        });
        _spacePresenter.Received().LoadElementAsync(0);
    }

    [Test]
    public void SaveObjectButton_Clicked_CallsSaveSelectedObjectAsync()
    {
        var systemUnderTest = GetSpaceViewForTesting();

        var button = systemUnderTest.Find("button.btn.btn-primary.save-element");
        button.Click();
        _spacePresenter.Received().SaveSelectedElementAsync();
    }

    [Test]
    public void SaveObjectButton_Clicked_OperationCanceledExceptionCaught()
    {
        _spacePresenter.SaveSelectedElementAsync().Throws(new OperationCanceledException());

        var systemUnderTest = GetSpaceViewForTesting();

        var button = systemUnderTest.Find("button.btn.btn-primary.save-element");
        Assert.That(() => button.Click(), Throws.Nothing);
        _spacePresenter.Received().SaveSelectedElementAsync();
    }

    [Test]
    public void SaveObjectButton_Clicked_OtherExceptionsWrappedInErrorState()
    {
        var ex = new Exception();
        _spacePresenter.SaveSelectedElementAsync().Throws(ex);

        var systemUnderTest = GetSpaceViewForTesting();

        var button = systemUnderTest.Find("button.btn.btn-primary.save-element");
        Assert.Multiple(() =>
        {
            Assert.That(() => button.Click(), Throws.Nothing);
            Assert.That(systemUnderTest.Instance.ErrorState, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.ErrorState!.CallSite, Is.EqualTo("Save element"));
            Assert.That(systemUnderTest.Instance.ErrorState!.Exception, Is.EqualTo(ex));
        });
        _spacePresenter.Received().SaveSelectedElementAsync();
    }

    [Test]
    public void ShowElementContentButton_Clicked_CallsShowSelectedElementContentAsync()
    {
        var systemUnderTest = GetSpaceViewForTesting();

        var button = systemUnderTest.Find("button.btn.btn-primary.show-content");
        button.Click();
        _spacePresenter.Received().ShowSelectedElementContentAsync();
    }

    [Test]
    public void ShowElementContentButton_Clicked_OperationCanceledExceptionCaught()
    {
        _spacePresenter.ShowSelectedElementContentAsync().Throws(new OperationCanceledException());

        var systemUnderTest = GetSpaceViewForTesting();

        var button = systemUnderTest.Find("button.btn.btn-primary.show-content");
        Assert.That(() => button.Click(), Throws.Nothing);
        _spacePresenter.Received().ShowSelectedElementContentAsync();
    }

    [Test]
    public void ShowElementContentButton_Clicked_OtherExceptionsWrappedInErrorState()
    {
        var ex = new Exception();
        _spacePresenter.ShowSelectedElementContentAsync().Throws(ex);

        var systemUnderTest = GetSpaceViewForTesting();

        var button = systemUnderTest.Find("button.btn.btn-primary.show-content");
        Assert.Multiple(() =>
        {
            Assert.That(() => button.Click(), Throws.Nothing);
            Assert.That(systemUnderTest.Instance.ErrorState, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.ErrorState!.CallSite, Is.EqualTo("Show element content"));
            Assert.That(systemUnderTest.Instance.ErrorState!.Exception, Is.EqualTo(ex));
        });
        _spacePresenter.Received().ShowSelectedElementContentAsync();
    }

    private IRenderedComponent<SpaceView> GetSpaceViewForTesting(RenderFragment? childContent = null)
    {
        childContent ??= delegate {  };
        return _ctx.RenderComponent<SpaceView>(
            parameters => parameters
                .Add(p => p.ChildContent, childContent)
        );
    }
}