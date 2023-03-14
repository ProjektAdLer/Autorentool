using System;
using System.Collections.Generic;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.View.LearningSpace;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.LearningSpace;

[TestFixture]
public class LearningSpaceViewUt
{
#pragma warning disable CS8618 // set in setup - n.stich
    private TestContext _ctx;
    private ILearningSpacePresenter _learningSpacePresenter;
#pragma warning restore CS8618

    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _ctx.Services.AddMudServices();
        _ctx.JSInterop.SetupVoid("mudDragAndDrop.initDropZone", _ => true);
        _learningSpacePresenter = Substitute.For<ILearningSpacePresenter>();
        _ctx.Services.AddSingleton(_learningSpacePresenter);
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
        var deleteButton = systemUnderTest.FindOrFail("button.btn.btn-primary.delete-learning-element");
        var saveButton = systemUnderTest.FindOrFail("button.btn.btn-primary.save-learning-element");
        
        label.MarkupMatches(@"<label id=""learning-element-info"">Selected element: my secret name, Description: a super long description</label>");
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