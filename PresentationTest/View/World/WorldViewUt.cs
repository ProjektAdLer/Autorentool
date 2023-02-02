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
using Presentation.PresentationLogic.ModalDialog;
using Presentation.PresentationLogic.PathWay;
using Presentation.PresentationLogic.Space;
using Presentation.PresentationLogic.World;
using Presentation.View.Space;
using Presentation.View.World;
using Presentation.View.PathWay;
using Shared;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.World;

[TestFixture]
public class WorldViewUt
{
#pragma warning disable CS8618 // set in setup - n.stich
    private TestContext _ctx;
    private IMouseService _mouseService;
    private IWorldPresenter _worldPresenter;
    private IWorldViewModalDialogFactory _modalDialogFactory;
#pragma warning restore CS8618
    
    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _mouseService = Substitute.For<IMouseService>();
        _worldPresenter = Substitute.For<IWorldPresenter>();
        _modalDialogFactory = Substitute.For<IWorldViewModalDialogFactory>();
        _ctx.ComponentFactories.AddStub<SpaceView>();
        _ctx.ComponentFactories.AddStub<DraggableObjectInPathWay>();
        _ctx.ComponentFactories.AddStub<Presentation.View.PathWay.PathWay>();
        _ctx.ComponentFactories.AddStub<ModalDialog>();
        _ctx.Services.AddSingleton(_mouseService);
        _ctx.Services.AddSingleton(_worldPresenter);
        _ctx.Services.AddSingleton(_modalDialogFactory);
    }

    [Test]
    public void Constructor_InjectsDependencies()
    {
        var systemUnderTest = GetWorldViewForTesting();
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.MouseService, Is.EqualTo(_mouseService));
            Assert.That(systemUnderTest.Instance.WorldP, Is.EqualTo(_worldPresenter));
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
        
        var systemUnderTest = GetWorldViewForTesting(childContent);

        Assert.That(systemUnderTest.FindOrFail("div.barbaz").TextContent, Is.EqualTo("foobar"));
    }

    [Test]
    public void Render_ShowingSpaceFalse_DoesNotRenderSpaceView()
    {
        _worldPresenter.ShowingSpaceView.Returns(false);
        
        var systemUnderTest = GetWorldViewForTesting();

        Assert.That(systemUnderTest.FindComponent<Stub<SpaceView>>,
            Throws.TypeOf<ComponentNotFoundException>());
    }

    [Test]
    public void Render_ShowingSpaceTrue_DoesRenderSpaceViewWithButton()
    {
        _worldPresenter.ShowingSpaceView.Returns(true);
        
        var systemUnderTest = GetWorldViewForTesting();

        var spaceView = systemUnderTest.FindComponentOrFail<Stub<SpaceView>>();
        var childContent = (RenderFragment)spaceView.Instance.Parameters[nameof(SpaceView.ChildContent)];
        Assert.That(childContent, Is.Not.Null);
        var childContentRendered = _ctx.Render(childContent);
        childContentRendered.MarkupMatches(
            @"<button class=""btn btn-primary"">Close Space View</button>");
    }

    [Test]
    public void Render_ShowSpaceTrue_ChildContentOnClickCallsCloseSpaceView()
    {
        _worldPresenter.ShowingSpaceView.Returns(true);
        
        var systemUnderTest = GetWorldViewForTesting();

        var spaceView = systemUnderTest.FindComponentOrFail<Stub<SpaceView>>();
        var childContent = (RenderFragment)spaceView.Instance.Parameters[nameof(SpaceView.ChildContent)];
        Assert.That(childContent, Is.Not.Null);
        var childContentRendered = _ctx.Render(childContent);
        childContentRendered.FindOrFail("button.btn.btn-primary").Click();
        _worldPresenter.Received(1).CloseSpaceView();
    }

    [Test]
    public void Render_WorldSet_RendersNameWorkloadAndPoints()
    {
        var world = Substitute.For<IWorldViewModel>();
        world.Name.Returns("my insanely sophisticated name");
        world.Workload.Returns(42);
        world.Points.Returns(9);
        _worldPresenter.WorldVm.Returns(world);
        
        var systemUnderTest = GetWorldViewForTesting();

        var h2 = systemUnderTest.FindOrFail("h2");
        h2.MarkupMatches(@"<h2>World: my insanely sophisticated name</h2>");
        var h5 = systemUnderTest.FindAll("h5");
        h5[0].MarkupMatches(@"<h5>Workload: 42 minutes</h5>");
        h5[1].MarkupMatches(@"<h5>Points: 9</h5>");
    }
    
    [Test]
    public void Render_SpaceInWorld_CorrectDraggableForEachSpace()
    {
        var space1 = Substitute.For<ISpaceViewModel>();
        var space2 = Substitute.For<ISpaceViewModel>();
        var pathWayCondition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        var pathWayConditions = new List<PathWayConditionViewModel> { pathWayCondition };
        var spaces = new List<ISpaceViewModel> { space1, space2 };
        var pathWay = new PathwayViewModel(space1, space2);
        var pathWayViewModels = new List<IPathWayViewModel> { pathWay };

        var world = Substitute.For<IWorldViewModel>();
        world.Spaces.Returns(spaces);
        world.ObjectsInPathWays.Returns(spaces);
        world.PathWays.Returns(pathWayViewModels);
        world.PathWayConditions.Returns(pathWayConditions);
        _worldPresenter.WorldVm.Returns(world);
        
        var systemUnderTest = GetWorldViewForTesting();

        var draggableSpaces = systemUnderTest.FindComponentsOrFail<Stub<DraggableSpace>>().ToList();
        var draggablePathWayConditions = systemUnderTest.FindComponentsOrFail<Stub<DraggablePathWayCondition>>().ToList();
        var pathWays = systemUnderTest.FindComponentsOrFail<Stub<Presentation.View.PathWay.PathWay>>().ToList();

        Assert.Multiple(() =>
        {
            Assert.That(draggableSpaces, Has.Count.EqualTo(spaces.Count));
            Assert.That(draggablePathWayConditions, Has.Count.EqualTo(pathWayConditions.Count));
            Assert.That(spaces.All(le =>
                draggableSpaces.Any(dle =>
                    dle.Instance.Parameters[nameof(DraggableObjectInPathWay.ObjectInPathWay)] == le)));
            Assert.That(pathWays, Has.Count.EqualTo(pathWayViewModels.Count));
        });
    }

    [Test]
    public void CreateSpaceDialog_FlagSet_CallsFactory_RendersRenderFragment_CallsPresenterOnDialogClose()
    {
        //prepare presenter
        var world = Substitute.For<IWorldViewModel>();
        _worldPresenter.WorldVm.Returns(world);
        _worldPresenter.CreateSpaceDialogOpen.Returns(true);
        
        RenderFragment fragment = builder =>
        {
            builder.OpenElement(0, "p");
            builder.AddContent(1, "foobar");
            builder.CloseElement();
        };
        ModalDialogOnClose? callback = null;
        _modalDialogFactory
            .GetCreateSpaceFragment(Arg.Any<ModalDialogOnClose>())
            .Returns(fragment)
            .AndDoes(ci =>
            {
                callback = ci.Arg<ModalDialogOnClose>();
            });
        
        var systemUnderTest = GetWorldViewForTesting();

        _modalDialogFactory.Received().GetCreateSpaceFragment(Arg.Any<ModalDialogOnClose>());
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
    public void CreatePathWayConditionDialog_FlagSet_CallsFactory_RendersRenderFragment_CallsPresenterOnDialogClose()
    {
        //prepare presenter
        var world = Substitute.For<IWorldViewModel>();
        _worldPresenter.WorldVm.Returns(world);
        _worldPresenter.CreatePathWayConditionDialogOpen.Returns(true);
        
        RenderFragment fragment = builder =>
        {
            builder.OpenElement(0, "p");
            builder.AddContent(1, "foobar");
            builder.CloseElement();
        };
        ModalDialogOnClose? callback = null;
        _modalDialogFactory
            .GetCreatePathWayConditionFragment(Arg.Any<ModalDialogOnClose>())
            .Returns(fragment)
            .AndDoes(ci =>
            {
                callback = ci.Arg<ModalDialogOnClose>();
            });
        
        var systemUnderTest = GetWorldViewForTesting();

        _modalDialogFactory.Received().GetCreatePathWayConditionFragment(Arg.Any<ModalDialogOnClose>());
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
        _worldPresenter.Received().OnCreatePathWayConditionDialogClose(returnValue);
    }

    [Test]
    public void EditPathWayConditionDialog_FlagSet_CallsFactory_RendersRenderFragment_CallsPresenterOnDialogClose()
    {
        //prepare presenter
        var world = Substitute.For<IWorldViewModel>();
        _worldPresenter.WorldVm.Returns(world);
        _worldPresenter.EditPathWayConditionDialogOpen.Returns(true);
        var initialValues = new Dictionary<string, string>
        {
            {"baba", "bubu"}
        };
        _worldPresenter.EditConditionDialogInitialValues.Returns(initialValues);
        
        RenderFragment fragment = builder =>
        {
            builder.OpenElement(0, "p");
            builder.AddContent(1, "foobar");
            builder.CloseElement();
        };
        ModalDialogOnClose? callback = null;
        _modalDialogFactory
            .GetEditPathWayConditionFragment(initialValues, Arg.Any<ModalDialogOnClose>())
            .Returns(fragment)
            .AndDoes(ci =>
            {
                callback = ci.Arg<ModalDialogOnClose>();
            });
        
        var systemUnderTest = GetWorldViewForTesting();

        _modalDialogFactory.Received().GetEditPathWayConditionFragment(initialValues, Arg.Any<ModalDialogOnClose>());
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
        _worldPresenter.Received().OnEditPathWayConditionDialogClose(returnValue);
    }
    
    [Test]
    public void EditSpaceDialog_FlagSet_CallsFactory_RendersRenderFragment_CallsPresenterOnDialogClose()
    {
        //prepare presenter
        var world = Substitute.For<IWorldViewModel>();
        _worldPresenter.WorldVm.Returns(world);
        _worldPresenter.EditSpaceDialogOpen.Returns(true);
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
            .GetEditSpaceFragment(initialValues, Arg.Any<ModalDialogOnClose>())
            .Returns(fragment)
            .AndDoes(ci =>
            {
                callback = ci.Arg<ModalDialogOnClose>();
            });
        
        var systemUnderTest = GetWorldViewForTesting();

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
        _worldPresenter.Received().OnEditSpaceDialogClose(returnValue);
    }


    [Test]
    public void Svg_MouseMove_CallsMouseService()
    {
        var mouseEventArgs = new MouseEventArgs();
        var systemUnderTest = GetWorldViewForTesting();

        var svg = systemUnderTest.FindOrFail("svg");
        svg.MouseMove(mouseEventArgs);
        
        _mouseService.Received().FireMove(systemUnderTest.Instance, mouseEventArgs);
    }

    [Test]
    public void Svg_MouseUp_CallsMouseService()
    {
        var mouseEventArgs = new MouseEventArgs();
        var systemUnderTest = GetWorldViewForTesting();

        var svg = systemUnderTest.FindOrFail("svg");
        svg.MouseUp(mouseEventArgs);
        
        _mouseService.Received().FireUp(systemUnderTest.Instance, mouseEventArgs);
    }

    [Test]
    public void Svg_MouseLeave_CallsMouseService()
    {
        var mouseEventArgs = new MouseEventArgs();
        var systemUnderTest = GetWorldViewForTesting();

        var svg = systemUnderTest.FindOrFail("svg");
        svg.MouseLeave(mouseEventArgs);
        
        _mouseService.Received().FireOut(systemUnderTest.Instance, mouseEventArgs);
    }

    [Test]
    public void AddSpaceButton_Clicked_CallsAddNewSpace()
    {
        var systemUnderTest = GetWorldViewForTesting();

        var addSpaceButton = systemUnderTest.FindOrFail("button.btn.btn-primary.add-space");
        addSpaceButton.Click();
        _worldPresenter.Received().AddNewSpace();
    }
    
    [Test]
    public void AddConditionButton_Clicked_CallsAddNewPathWayCondition()
    {
        var systemUnderTest = GetWorldViewForTesting();

        var addConditionButton = systemUnderTest.FindOrFail("button.btn.btn-primary.add-pathway-condition");
        addConditionButton.Click();
        _worldPresenter.Received().AddNewPathWayCondition();
    }

    [Test]
    public void LoadSpaceButton_Clicked_CallsLoadSpaceAsync()
    {
        var systemUnderTest = GetWorldViewForTesting();
        
        var loadSpaceButton = systemUnderTest.FindOrFail("button.btn.btn-primary.load-space");
        loadSpaceButton.Click();
        _worldPresenter.Received().LoadSpaceAsync();
    }

    [Test]
    public void LoadSpaceButton_Clicked_OperationCancelledExceptionCaught()
    {
        _worldPresenter.LoadSpaceAsync().Throws<OperationCanceledException>();
        
        var systemUnderTest = GetWorldViewForTesting();
        
        var loadSpaceButton = systemUnderTest.FindOrFail("button.btn.btn-primary.load-space");
        Assert.That(() => loadSpaceButton.Click(), Throws.Nothing);
        _worldPresenter.Received().LoadSpaceAsync();
    }

    [Test]
    public void LoadSpaceButton_Clicked_OtherExceptionsWrappedInErrorState()
    {
        _worldPresenter.LoadSpaceAsync().Throws(new Exception("saatana"));
        
        var systemUnderTest = GetWorldViewForTesting();
        
        var loadSpaceButton = systemUnderTest.FindOrFail("button.btn.btn-primary.load-space");
        Assert.That(() => loadSpaceButton.Click(), Throws.Nothing);
        _worldPresenter.Received().LoadSpaceAsync();
        
        var errorDialog = systemUnderTest.FindComponentOrFail<Stub<ModalDialog>>();
        Assert.Multiple(() =>
        {
            Assert.That(errorDialog.Instance.Parameters[nameof(ModalDialog.Title)], Is.EqualTo("Exception encountered"));
            Assert.That(errorDialog.Instance.Parameters[nameof(ModalDialog.Text)], Is.EqualTo(
                @"Exception encountered at Load space:
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
    public void EditSpaceButton_Clicked_CallsOpenEditSelectedSpaceDialog()
    {
        var space = Substitute.For<ISpaceViewModel>();
        var worldVm = Substitute.For<IWorldViewModel>();
        worldVm.Spaces.Returns(new List<ISpaceViewModel> { space });
        worldVm.SelectedObject.Returns(space);
        _worldPresenter.WorldVm.Returns(worldVm);

        var systemUnderTest = GetWorldViewForTesting();
        
        var editSpaceButton = systemUnderTest.FindOrFail("button.btn.btn-primary.edit-object");
        editSpaceButton.Click();
        _worldPresenter.Received().OpenEditSelectedObjectDialog();
    }

    [Test]
    public void DeleteSpaceButton_Clicked_CallsDeleteSelectedSpace()
    {
        var space = Substitute.For<ISpaceViewModel>();
        var worldVm = Substitute.For<IWorldViewModel>();
        worldVm.Spaces.Returns(new List<ISpaceViewModel> { space });
        worldVm.SelectedObject.Returns(space);
        _worldPresenter.WorldVm.Returns(worldVm);

        var systemUnderTest = GetWorldViewForTesting();
        
        var editSpaceButton = systemUnderTest.FindOrFail("button.btn.btn-primary.delete-object");
        editSpaceButton.Click();
        _worldPresenter.Received().DeleteSelectedObject();
    }
    
    [Test]
    public void SaveSpaceButton_Clicked_CallsSaveSelectedSpaceAsync()
    {
        var systemUnderTest = GetWorldViewForTesting();
        
        var loadElementButton = systemUnderTest.FindOrFail("button.btn.btn-primary.save-space");
        loadElementButton.Click();
        _worldPresenter.Received().SaveSelectedSpaceAsync();
    }
    
    [Test]
    public void SaveSpaceButton_Clicked_OperationCancelledExceptionCaught()
    {
        _worldPresenter.SaveSelectedSpaceAsync().Throws<OperationCanceledException>();
        
        var systemUnderTest = GetWorldViewForTesting();
        
        var loadElementButton = systemUnderTest.FindOrFail("button.btn.btn-primary.save-space");
        Assert.That(() => loadElementButton.Click(), Throws.Nothing);
        _worldPresenter.Received().SaveSelectedSpaceAsync();
    }
    
    [Test]
    public void SaveSpaceButton_Clicked_OtherExceptionsWrappedInErrorState()
    {
        _worldPresenter.SaveSelectedSpaceAsync().Throws(new Exception("saatana"));
        
        var systemUnderTest = GetWorldViewForTesting();
        
        var loadElementButton = systemUnderTest.FindOrFail("button.btn.btn-primary.save-space");
        Assert.That(() => loadElementButton.Click(), Throws.Nothing);
        _worldPresenter.Received().SaveSelectedSpaceAsync();
    }
    
    [Test]
    public void ShowSelectedSpace_Called_CallsShowSelectedSpaceView()
    {
        var worldVm = Substitute.For<IWorldViewModel>();
        _worldPresenter.WorldVm.Returns(worldVm);

        var systemUnderTest = GetWorldViewForTesting();
        
        var editSpaceButton = systemUnderTest.FindOrFail("button.btn.btn-primary.show-space");
        editSpaceButton.Click();
        _worldPresenter.Received().ShowSelectedSpaceView();
    }
    
    private IRenderedComponent<WorldView> GetWorldViewForTesting(RenderFragment? childContent = null)
    {
        return _ctx.RenderComponent<WorldView>(parameters => parameters
            .Add(p => p.ChildContent, childContent));
    }
}