using System;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.Toolbox;
using Presentation.View.Toolbox;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.Toolbox;

[TestFixture]
public class LearningObjectRenderFragmentBaseUt
{
    private TestContext ctx = null!;
    private IToolboxController toolboxController = null!;
    
    [SetUp]
    public void Setup()
    {
        ctx = new TestContext();
        toolboxController = Substitute.For<IToolboxController>();
        ctx.Services.AddSingleton(toolboxController);
    }
    
    [Test]
    public void LearningObjectRenderFragmentBase_Constructor_AllParametersSet()
    {
        var obj = Substitute.For<IDisplayableLearningObject>();
        const string css = "foobar";
        var childContent = new RenderFragment(_ => { });
        
        var systemUnderTest = GetFragmentForTesting(obj, css, childContent);
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.Obj, Is.EqualTo(obj));
            Assert.That(systemUnderTest.Instance.CssClassSelector, Is.EqualTo(css));
            Assert.That(systemUnderTest.Instance.ChildContent, Is.EqualTo(childContent));
            Assert.That(systemUnderTest.Instance.ToolboxController, Is.Not.Null);
        });
    }

    [Test]
    public void LearningObjectRenderFragmentBase_SetParametersAsync_ThrowsWhenNoObjOrNull()
    {
        var ex = Assert.Throws<ArgumentNullException>(() =>
            ctx.RenderComponent<LearningObjectRenderFragmentBase>(builder =>
            {
                builder.Add(p => p.Obj, null);
            }));
        Assert.That(ex!.Message, Is.EqualTo("Obj cannot be null for LearingObjectRenderFragmentBase (Parameter 'Obj')"));
    }

    [Test]
    public void LearningObjectRenderFragmentBase_OnDoubleClick_CallsToolboxController()
    {
        var obj = Substitute.For<IDisplayableLearningObject>();
        const string css = "foobar";
        
        var systemUnderTest = GetFragmentForTesting(obj, css);
        
        systemUnderTest.Find("div").DoubleClick();
        toolboxController.Received().LoadObjectIntoWorkspace(obj);
    }

    private IRenderedComponent<LearningObjectRenderFragmentBase> GetFragmentForTesting(
        IDisplayableLearningObject? obj = null, 
        string? cssClassSelector = null,
        RenderFragment? childContent = null
    )
    {
        obj ??= Substitute.For<IDisplayableLearningObject>();
        cssClassSelector ??= "";
        return ctx.RenderComponent<LearningObjectRenderFragmentBase>(builder =>
        {
            builder.Add(p => p.Obj, obj);
            builder.Add(p => p.CssClassSelector, cssClassSelector);
            builder.Add(p => p.ChildContent, childContent);
        });
    }
}