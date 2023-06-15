using System;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using MudBlazor;
using MudBlazor.Services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.SelectedViewModels;
using Presentation.View.LearningSpace;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.LearningSpace;

[TestFixture]
public class LearningSpaceViewUt
{
#pragma warning disable CS8618 // set in setup - n.stich
    private TestContext _ctx;
    private ILearningSpacePresenter _learningSpacePresenter;
    private ISelectedViewModelsProvider _mediator;
    private IStringLocalizer<LearningSpaceView> _localizer;
#pragma warning restore CS8618

    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _ctx.Services.AddMudServices();
        _ctx.JSInterop.SetupVoid("mudDragAndDrop.initDropZone", _ => true);
        _ctx.ComponentFactories.AddStub<MudText>();
        _learningSpacePresenter = Substitute.For<ILearningSpacePresenter>();
        _mediator = Substitute.For<ISelectedViewModelsProvider>();
        _localizer = Substitute.For<IStringLocalizer<LearningSpaceView>>();
        _localizer[Arg.Any<string>()].Returns(ci => new LocalizedString(ci.Arg<string>(), ci.Arg<string>()));
        _ctx.Services.AddSingleton(_learningSpacePresenter);
        _ctx.Services.AddSingleton(_mediator);
        _ctx.Services.AddSingleton(_localizer);
        _ctx.Services.AddLogging();
    }

    [Test]
    public void Constructor_InjectsDependencies()
    {
        var systemUnderTest = GetLearningSpaceViewForTesting();

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.LearningSpaceP, Is.EqualTo(_learningSpacePresenter));
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

        //TODO Use this for LmsLoginDialogUt
        var spaceWorkload = systemUnderTest.Find("h3.space-workload");
        spaceWorkload.MarkupMatches(
            @"<h3 class=""text-base text-adlerblue-600 space-workload""><span class=""text-adlergrey-600"">LearningSpace.SpaceWorkload.Text</span> 42<span class=""text-adlergrey-600""> min.</span></h3>");
        var spacePoints = systemUnderTest.Find("h3.space-points");
        spacePoints.MarkupMatches(
            @"<h3 class=""text-base text-adlerblue-600 space-points""><span class=""text-adlergrey-600"">LearningSpace.SpacePoints.Text</span> 8</h3>");
    }

    [Test]
    public void Render_LearningObjectSelected_RendersLearningObjectSection()
    {
        var learningSpace = Substitute.For<ILearningSpaceViewModel>();
        var learningObject = Substitute.For<ILearningElementViewModel>();
        _learningSpacePresenter.LearningSpaceVm.Returns(learningSpace);
        _mediator.LearningElement.Returns(learningObject);

        var systemUnderTest = GetLearningSpaceViewForTesting();

        var elementName = systemUnderTest.Find("h3.space-theme");
        elementName.MarkupMatches(@"<h3 class=""text-base text-adlerblue-600 space-theme"" ><span class=""text-adlergrey-600"" >LearningSpace.SpaceTheme.Text</span>Campus</h3>");
        var elementDescription = systemUnderTest.Find("h3.space-goals");
        elementDescription.MarkupMatches(
            @"<h3 class=""text-base text-adlerblue-600 flex-initial break-all space-goals"" ><span class=""text-adlergrey-600"" >LearningSpace.SpaceGoals.Text</span></h3>");
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
    public void DeleteObjectButton_Clicked_CallsDeleteSelectedLearningObject()
    {
        var space = Substitute.For<ILearningSpaceViewModel>();
        _learningSpacePresenter.LearningSpaceVm.Returns(space);
        var systemUnderTest = GetLearningSpaceViewForTesting();

        var button = systemUnderTest.FindOrFail("button.delete-learning-element");
        button.Click();
        _learningSpacePresenter.Received().DeleteSelectedLearningElement();
    }


    [Test]
    public void SaveObjectButton_Clicked_CallsSaveSelectedLearningObjectAsync()
    {
        var systemUnderTest = GetLearningSpaceViewForTesting();

        var button = systemUnderTest.Find("button.save-learning-element");
        button.Click();
        _learningSpacePresenter.Received().SaveSelectedLearningElementAsync();
    }

    [Test]
    public void SaveObjectButton_Clicked_OperationCanceledExceptionCaught()
    {
        _learningSpacePresenter.SaveSelectedLearningElementAsync().Throws(new OperationCanceledException());

        var systemUnderTest = GetLearningSpaceViewForTesting();

        var button = systemUnderTest.Find("button.save-learning-element");
        Assert.That(() => button.Click(), Throws.Nothing);
        _learningSpacePresenter.Received().SaveSelectedLearningElementAsync();
    }

    [Test]
    public void SaveObjectButton_Clicked_OtherExceptionsWrappedInErrorState()
    {
        var ex = new Exception();
        _learningSpacePresenter.SaveSelectedLearningElementAsync().Throws(ex);

        var systemUnderTest = GetLearningSpaceViewForTesting();

        var button = systemUnderTest.Find("button.save-learning-element");
        Assert.Multiple(() =>
        {
            Assert.That(() => button.Click(), Throws.Nothing);
            Assert.That(systemUnderTest.Instance.ErrorState, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.ErrorState!.CallSite, Is.EqualTo("Error.ExceptionWrapper.SaveLearningElement.Message"));
            Assert.That(systemUnderTest.Instance.ErrorState!.Exception, Is.EqualTo(ex));
        });
        _learningSpacePresenter.Received().SaveSelectedLearningElementAsync();
    }

    [Test]
    public void ShowElementContentButton_Clicked_CallsShowSelectedElementContentAsync()
    {
        var systemUnderTest = GetLearningSpaceViewForTesting();

        var button = systemUnderTest.Find("button.show-learning-element-content");
        button.Click();
        _learningSpacePresenter.Received().ShowSelectedElementContentAsync();
    }

    [Test]
    public void ShowElementContentButton_Clicked_OperationCanceledExceptionCaught()
    {
        _learningSpacePresenter.ShowSelectedElementContentAsync().Throws(new OperationCanceledException());

        var systemUnderTest = GetLearningSpaceViewForTesting();

        var button = systemUnderTest.Find("button.show-learning-element-content");
        Assert.That(() => button.Click(), Throws.Nothing);
        _learningSpacePresenter.Received().ShowSelectedElementContentAsync();
    }

    [Test]
    public void ShowElementContentButton_Clicked_OtherExceptionsWrappedInErrorState()
    {
        var ex = new Exception();
        _learningSpacePresenter.ShowSelectedElementContentAsync().Throws(ex);

        var systemUnderTest = GetLearningSpaceViewForTesting();

        var button = systemUnderTest.Find("button.show-learning-element-content");
        Assert.Multiple(() =>
        {
            Assert.That(() => button.Click(), Throws.Nothing);
            Assert.That(systemUnderTest.Instance.ErrorState, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.ErrorState!.CallSite,
                Is.EqualTo("Error.ExceptionWrapper.ShowLearningElementContent.Message"));
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