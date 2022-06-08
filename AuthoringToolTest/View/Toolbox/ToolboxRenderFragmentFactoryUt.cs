using System;
using System.Collections;
using AuthoringTool.Entities;
using AuthoringTool.PresentationLogic;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;
using AuthoringTool.PresentationLogic.Toolbox;
using AuthoringTool.View.Toolbox;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using TestContext = Bunit.TestContext;

namespace AuthoringToolTest.View.Toolbox;

[TestFixture]
public class ToolboxRenderFragmentFactoryUt
{
#pragma warning disable CS8618
    private TestContext _testContext;
    private IToolboxController _toolboxController;
#pragma warning restore CS8618
    
    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
        _toolboxController = Substitute.For<IToolboxController>();
        _testContext.Services.Add(new ServiceDescriptor(typeof(IToolboxController), _toolboxController));
    }
    
    [Test]
    [TestCaseSource(typeof(ToolboxRenderFragmentFactoryTestCases))]
    public void ToolboxRenderFragmentFactory_GetRenderFragment_ReturnsCorrectFragment(IDisplayableLearningObject obj, string expectedMarkup)
    {
        var systemUnderTest = GetTestableToolboxRenderFragmentFactory();

        var renderFragment = systemUnderTest.GetRenderFragment(obj);
        var rendered = _testContext.Render(renderFragment);
        
        //see https://bunit.dev/docs/verification/verify-markup.html
        rendered.MarkupMatches(expectedMarkup);
    }
    
    [Test]
    [TestCaseSource(typeof(ToolboxRenderFragmentFactoryTestCases))]
    public void ToolboxRenderFragmentFactory_GetRenderFragment_CallingTwiceReturnsSameFragment(IDisplayableLearningObject obj, string expectedMarkup)
    {
        var systemUnderTest = GetTestableToolboxRenderFragmentFactory();

        var firstPass = systemUnderTest.GetRenderFragment(obj);
        var firstPassRendered = _testContext.Render(firstPass);
        firstPassRendered.MarkupMatches(expectedMarkup);

        var secondPass = systemUnderTest.GetRenderFragment(obj);
        var secondPassRendered = _testContext.Render(secondPass);
        secondPassRendered.MarkupMatches(expectedMarkup);
        
        Assert.That(secondPass, Is.SameAs(firstPass));
    }

    [Test]
    public void ToolboxRenderFragmentFactory_GetRenderFragment_ThrowsExceptionOnInvalidObject()
    {
        var systemUnderTest = GetTestableToolboxRenderFragmentFactory();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            systemUnderTest.GetRenderFragment(Substitute.For<IDisplayableLearningObject>()));
    }


    private IAbstractToolboxRenderFragmentFactory GetTestableToolboxRenderFragmentFactory(ILogger<ToolboxRenderFragmentFactory>? logger = null)
    {
        logger ??= Substitute.For<ILogger<ToolboxRenderFragmentFactory>>();
        return new ToolboxRenderFragmentFactory(logger);
    }

    private class ToolboxRenderFragmentFactoryTestCases : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return new object[]
            {
                new LearningWorldViewModel("myname", "fa", "fa", "fa", "fa", "fa"),
                @"<div class=""col-3 element text-center text-wrap learning-world"">
myname
<br/>
    <i class=""bi bi-globe""></i>
</div>"
            };
            
            yield return new object[]
            {
                new LearningSpaceViewModel("a name", "sn", "authors", "a description", "goals"),
                @"<div class=""col-3 element text-center text-wrap learning-space"">
a name
<br/>
    <p>a description</p>
</div>"
            };

            yield return new object[]
            {
                new LearningElementViewModel("another name", "an", null, null, "authors", "description", "goals"),
                @"<div class=""col-3 element text-center text-wrap learning-element"">
another name
<br/>
    <p>I am an element</p>
</div>"
            };
        }
    }
}