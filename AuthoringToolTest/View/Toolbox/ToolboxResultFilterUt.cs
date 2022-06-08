using System.Collections;
using System.Collections.Generic;
using AuthoringTool.PresentationLogic;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;
using AuthoringTool.View.Toolbox;
using NUnit.Framework;

namespace AuthoringToolTest.View.Toolbox;

[TestFixture]
public class ToolboxResultFilterUt
{
    private static readonly LearningWorldViewModel World1, World2, World3, World4;
    private static readonly LearningSpaceViewModel Space1, Space2, Space3, Space4;
    private static readonly LearningElementViewModel Element1, Element2, Element3;
    private static readonly IEnumerable<IDisplayableLearningObject> Collection;

    static ToolboxResultFilterUt()
    {
        World1 = new LearningWorldViewModel("metrics", "f", "f", "f", "f", "f");
        World2 = new LearningWorldViewModel("quality", "f", "f", "f", "f", "f");
        World3 = new LearningWorldViewModel("testing", "f", "f", "f", "f", "f");
        World4 = new LearningWorldViewModel("space and time complexity", "f", "f", "f", "f", "f");
        Space1 = new LearningSpaceViewModel("lines of code metric", "foo", "fa", "ba", "b");
        Space2 = new LearningSpaceViewModel("unit testing", "f", "ba", "ba", "fa");
        Space3 = new LearningSpaceViewModel("measures of code quality", "fa", "fa", "ba", "ba");
        Space4 = new LearningSpaceViewModel("elements of code quality", "fa", "fa", "ba", "ba");
        Element1 = new LearningElementViewModel("principles of unit testing", "s", null, null, "s", 
            "s", "s");
        Element2 = new LearningElementViewModel("example calculation of lines of code metric", "s",
            null, null, "s", "s", "s");
        Element3 = new LearningElementViewModel("real world example of measurable code quality", "s", null, null, "s",
            "s", "s");
        
        Collection = new List<IDisplayableLearningObject>
        {
            World1, World2, World3, World4,
            Space1, Space2, Space3, Space4,
            Element1, Element2, Element3,
        };
        
    }

    private class FilterCollectionFiltersCorrectlyTestCases : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            //shortcuts
            var worlds = new IDisplayableLearningObject[] { World1, World2, World3, World4 };
            var spaces = new IDisplayableLearningObject[] { Space1, Space2, Space3, Space4 };
            var elements = new IDisplayableLearningObject[] { Element1, Element2, Element3 };
            var allObjects = Collection;
            
            //test cases
            //basic cases
            yield return new object[] { "metric", new IDisplayableLearningObject[] { World1, Space1, Element2 } };
            yield return new object[] { " metric", new IDisplayableLearningObject[] { World1, Space1, Element2 } };
            yield return new object[] { "metric ", new IDisplayableLearningObject[] { World1, Space1, Element2 } };
            yield return new object[] { " metric ", new IDisplayableLearningObject[] { World1, Space1, Element2 } };
            yield return new object[] { "e", new IDisplayableLearningObject[] { World1, World3, World4, Space1, Space2, Space3, Space4, Element1, Element2, Element3} };
            yield return new object[] { " e", new IDisplayableLearningObject[] { World1, World3, World4, Space1, Space2, Space3, Space4, Element1, Element2, Element3} };
            yield return new object[] { "e ", new IDisplayableLearningObject[] { World1, World3, World4, Space1, Space2, Space3, Space4, Element1, Element2, Element3} };
            yield return new object[] { " e ", new IDisplayableLearningObject[] { World1, World3, World4, Space1, Space2, Space3, Space4, Element1, Element2, Element3} };
            //regex tests
            yield return new object[] { "world", worlds };
            yield return new object[] { "  world     ", worlds };
            yield return new object[] { "  world   foo  ", worlds };
            yield return new object[] { "world:", worlds };
            yield return new object[] { "space", spaces };
            yield return new object[] { "  space     ", spaces };
            yield return new object[] { "  space   foo  ", spaces };
            yield return new object[] { "space:", spaces };
            yield return new object[] { "element", elements };
            yield return new object[] { "  element     ", elements };
            yield return new object[] { "  element   foo  ", elements };
            yield return new object[] { "element:", elements };
            yield return new object[] { "world:metrics", new IDisplayableLearningObject[] { World1 } };
            yield return new object[] { "world:  metrics", new IDisplayableLearningObject[] { World1 } };
            yield return new object[] { "       world:metrics   ", new IDisplayableLearningObject[] { World1 } };
            yield return new object[] { "       world:   metrics   ", new IDisplayableLearningObject[] { World1 } };
            yield return new object[] { "space:unit", new IDisplayableLearningObject[] { Space2 } };
            yield return new object[] { "space:  unit", new IDisplayableLearningObject[] { Space2 } };
            yield return new object[] { "element:ulation", new IDisplayableLearningObject[] { Element2 } };
            yield return new object[] { "element:   ulation", new IDisplayableLearningObject[] { Element2 } };
            yield return new object[] { @"""element""", new IDisplayableLearningObject[] { Space4 } };
            yield return new object[] { @"""space""", new IDisplayableLearningObject[] { World4 } };
            yield return new object[] { @"""world""", new IDisplayableLearningObject[] { Element3 } };
            yield return new object[] { @"""   element  """, new IDisplayableLearningObject[] { Space4 } };
            yield return new object[] { @"""   space    """, new IDisplayableLearningObject[] { World4 } };
            yield return new object[] { @"""   world         """, new IDisplayableLearningObject[] { Element3 } };

        }
    }
    
    [Test]
    [TestCaseSource(typeof(FilterCollectionFiltersCorrectlyTestCases))]
    public void ToolboxResultFilter_FilterCollection_FiltersCorrectly(string inputString,
        IEnumerable<IDisplayableLearningObject> expected)
    {
        var systemUnderTest = GetToolboxResultFilterForTest();
        
        var actual = systemUnderTest.FilterCollection(Collection, inputString);
        
        Assert.That(actual, Is.EquivalentTo(expected));
    }
    
    [Test]
    [TestCase(@"""world""", ExpectedResult = true)]
    [TestCase(@"""space""", ExpectedResult = true)]
    [TestCase(@"""element""", ExpectedResult = true)]
    [TestCase(@"""element: foo""", ExpectedResult = true)]
    [TestCase(@"""element of surprise""", ExpectedResult = true)]
    [TestCase(@"""   world: foo bar""", ExpectedResult = true)]
    [TestCase(@"""world: foo bar""""", ExpectedResult = true)]
    [TestCase(@"""""", ExpectedResult = true)]
    [TestCase(@"   world: foo bar""", ExpectedResult = false)]
    [TestCase(@"""   world: foo bar", ExpectedResult = false)]
    [TestCase(@"   world: foo bar", ExpectedResult = false)]
    [TestCase(@"world: foo bar", ExpectedResult = false)]
    [TestCase(@"", ExpectedResult = false)]
    public bool ToolboxResultFilter_MatchesQuote_MatchesCorrectly(string inputString)
    {
        return GetToolboxResultFilterForTest().MatchesQuoteRule(inputString);
    }

    [Test]
    [TestCase(@"world", ExpectedResult = true)]
    [TestCase(@"world:", ExpectedResult = true)]
    [TestCase(@"world:foo", ExpectedResult = true)]
    [TestCase(@"world:   foo", ExpectedResult = true)]
    [TestCase(@"world:   foo  ", ExpectedResult = true)]
    [TestCase(@"  world:   foo", ExpectedResult = true)]
    [TestCase(@"  world:   foo  ", ExpectedResult = true)]
    [TestCase(@"world: foo:asdf:asdf", ExpectedResult = true)]
    [TestCase(@"world: foo:asdf:asdf""", ExpectedResult = true)]
    [TestCase(@"world""", ExpectedResult = true)]
    [TestCase(@"woirld", ExpectedResult = false)]
    [TestCase(@":world", ExpectedResult = false)]
    [TestCase(@"""world: foo:asdf:asdf""", ExpectedResult = false)]
    [TestCase(@"""world: foo:asdf:asdf", ExpectedResult = false)]
    [TestCase(@"space", ExpectedResult = false)]
    [TestCase(@"space:", ExpectedResult = false)]
    [TestCase(@"space:foobar", ExpectedResult = false)]
    [TestCase(@"element", ExpectedResult = false)]
    [TestCase(@"element:", ExpectedResult = false)]
    [TestCase(@"element:foobar", ExpectedResult = false)]
    public bool ToolboxResultFilter_MatchesWorld_MatchesCorrectly(string inputString)
    {
        return GetToolboxResultFilterForTest().MatchesWorldRule(inputString);
    }
    
    [Test]
    [TestCase(@"space", ExpectedResult = true)]
    [TestCase(@"space:", ExpectedResult = true)]
    [TestCase(@"space:foo", ExpectedResult = true)]
    [TestCase(@"space:   foo", ExpectedResult = true)]
    [TestCase(@"space:   foo  ", ExpectedResult = true)]
    [TestCase(@"  space:   foo", ExpectedResult = true)]
    [TestCase(@"  space:   foo  ", ExpectedResult = true)]
    [TestCase(@"space: foo:asdf:asdf", ExpectedResult = true)]
    [TestCase(@"space: foo:asdf:asdf""", ExpectedResult = true)]
    [TestCase(@"space""", ExpectedResult = true)]
    [TestCase(@"spoice", ExpectedResult = false)]
    [TestCase(@":space", ExpectedResult = false)]
    [TestCase(@"""space: foo:asdf:asdf""", ExpectedResult = false)]
    [TestCase(@"""space: foo:asdf:asdf", ExpectedResult = false)]
    [TestCase(@"world", ExpectedResult = false)]
    [TestCase(@"world:", ExpectedResult = false)]
    [TestCase(@"world:foobar", ExpectedResult = false)]
    [TestCase(@"element", ExpectedResult = false)]
    [TestCase(@"element:", ExpectedResult = false)]
    [TestCase(@"element:foobar", ExpectedResult = false)]
    public bool ToolboxResultFilter_MatchesSpace_MatchesCorrectly(string inputString)
    {
        return GetToolboxResultFilterForTest().MatchesSpaceRule(inputString);
    }

    [Test]
    [TestCase(@"element", ExpectedResult = true)]
    [TestCase(@"element:", ExpectedResult = true)]
    [TestCase(@"element:foo", ExpectedResult = true)]
    [TestCase(@"element:   foo", ExpectedResult = true)]
    [TestCase(@"element:   foo  ", ExpectedResult = true)]
    [TestCase(@"  element:   foo", ExpectedResult = true)]
    [TestCase(@"  element:   foo  ", ExpectedResult = true)]
    [TestCase(@"element: foo:asdf:asdf", ExpectedResult = true)]
    [TestCase(@"element: foo:asdf:asdf""", ExpectedResult = true)]
    [TestCase(@"element""", ExpectedResult = true)]
    [TestCase(@"elment", ExpectedResult = false)]
    [TestCase(@":element", ExpectedResult = false)]
    [TestCase(@"""element: foo:asdf:asdf""", ExpectedResult = false)]
    [TestCase(@"""element: foo:asdf:asdf", ExpectedResult = false)]
    [TestCase(@"world", ExpectedResult = false)]
    [TestCase(@"world:", ExpectedResult = false)]
    [TestCase(@"world:foobar", ExpectedResult = false)]
    [TestCase(@"space", ExpectedResult = false)]
    [TestCase(@"space:", ExpectedResult = false)]
    [TestCase(@"space:foobar", ExpectedResult = false)]
    public bool ToolboxResultFilter_MatchesElement_MatchesCorrectly(string inputString)
    {
        return GetToolboxResultFilterForTest().MatchesElementRule(inputString);
    }

    private IToolboxResultFilter GetToolboxResultFilterForTest()
    {
        return new ToolboxResultFilter();
    }
}