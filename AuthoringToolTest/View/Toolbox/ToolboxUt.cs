using AngleSharp.Dom;
using AuthoringTool.PresentationLogic;
using AuthoringTool.PresentationLogic.Toolbox;
using AuthoringTool.View.Toolbox;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using TestContext = Bunit.TestContext;

namespace AuthoringToolTest.View.Toolbox;

[TestFixture]
public class ToolboxUt
{
    private TestContext ctx = null!;
    private IAbstractToolboxRenderFragmentFactory fragmentFactory = null!;
    private IToolboxEntriesProvider entriesProvider = null!;
    private IToolboxResultFilter resultFilter = null!;

    [SetUp]
    public void Setup()
    {
        ctx = new TestContext();
        fragmentFactory = Substitute.For<IAbstractToolboxRenderFragmentFactory>();
        entriesProvider = Substitute.For<IToolboxEntriesProvider>();
        resultFilter = Substitute.For<IToolboxResultFilter>();
        ctx.Services.AddLogging();
        ctx.Services.AddSingleton(fragmentFactory);
        ctx.Services.AddSingleton(entriesProvider);
        ctx.Services.AddSingleton(resultFilter);
    }
    
    [Test]
    public void Toolbox_Constructor_SetsParametersAndInjectsServices()
    {
        var systemUnderTest = GetToolboxForTest();
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.Logger, Is.Not.Null);
            Assert.That(systemUnderTest.Instance.RenderFragmentFactory, Is.EqualTo(fragmentFactory));
            Assert.That(systemUnderTest.Instance.EntriesProvider, Is.EqualTo(entriesProvider));
            Assert.That(systemUnderTest.Instance.ResultFilter, Is.EqualTo(resultFilter));
        });
    }

    [Test]
    public void Toolbox_SearchBox_SetsSearchTerm()
    {
        const string input = "foobar this is my search term";
        
        var systemUnderTest = GetToolboxForTest();
        
        Assert.That(string.IsNullOrEmpty(systemUnderTest.Instance.SearchTerm));
        
        systemUnderTest.Find("input").Input(input);
        
        Assert.That(systemUnderTest.Instance.SearchTerm, Is.EqualTo(input));
    }

    [Test]
    public void Toolbox_SearchBox_EntriesFilteredWithSearchTerm()
    {
        var systemUnderTest = GetToolboxForTest();

        entriesProvider.Entries.Received();

        var entries = new IDisplayableLearningObject[] { null!, null!, null! };
        entriesProvider.Entries.Returns(entries);
        
        const string input = "foobar this is my search term";
        systemUnderTest.Find("input").Input(input);

        resultFilter.Received().FilterCollection(entries, input);
        
        var filteredEntries = new IDisplayableLearningObject[] { null!, null! };
        resultFilter.FilterCollection(entries, input).Returns(filteredEntries);
        
        Assert.That(systemUnderTest.Instance.FilteredEntries, Is.EqualTo(filteredEntries));
    }
    
    [Test]
    public void Toolbox_Tooltip_DisplaysResultFilterUserExplanationText()
    {
        resultFilter.UserExplanationText.Returns("foobar");
        var systemUnderTest = GetToolboxForTest();

        IElement? toolboxP = null;
        Assert.That(() => toolboxP = systemUnderTest.Find("div.tooltip-wrapper span p"), Throws.Nothing);
        if (toolboxP == null)
            Assert.Fail("Could not find tooltip p element");
        Assert.That(toolboxP!.InnerHtml, Is.EqualTo("foobar"));
    }

    private IRenderedComponent<AuthoringTool.View.Toolbox.Toolbox> GetToolboxForTest()
    {
        return ctx.RenderComponent<AuthoringTool.View.Toolbox.Toolbox>();
    }
}