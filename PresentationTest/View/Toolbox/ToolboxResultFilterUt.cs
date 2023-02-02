using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.Element;
using Presentation.PresentationLogic.Space;
using Presentation.PresentationLogic.World;
using Presentation.View.Toolbox;
using Shared;

namespace PresentationTest.View.Toolbox;

[TestFixture]
public class ToolboxResultFilterUt
{
    private static readonly WorldViewModel World1, World2, World3, World4;
    private static readonly SpaceViewModel Space1, Space2, Space3, Space4;
    private static readonly ElementViewModel Element1, Element2, Element3;
    private static readonly IEnumerable<IDisplayableObject> Collection;

    static ToolboxResultFilterUt()
    {
        World1 = new WorldViewModel("metrics", "f", "f", "f", "f", "f");
        World2 = new WorldViewModel("quality", "f", "f", "f", "f", "f");
        World3 = new WorldViewModel("testing", "f", "f", "f", "f", "f");
        World4 = new WorldViewModel("space and time complexity", "f", "f", "f", "f", "f");
        Space1 = new SpaceViewModel("lines of code metric", "foo", "fa", "ba", "b");
        Space2 = new SpaceViewModel("unit testing", "f", "ba", "ba", "fa");
        Space3 = new SpaceViewModel("measures of code quality", "fa", "fa", "ba", "ba");
        Space4 = new SpaceViewModel("Elements of code quality", "fa", "fa", "ba", "ba");
        //nullability overrides because we don't care about content here
        Element1 = new ElementViewModel("principles of unit testing", "s", null!, "url","s", 
            "s", "s",ElementDifficultyEnum.Easy);
        Element2 = new ElementViewModel("example calculation of lines of code metric", "s", null!, "url","s", "s", "s",ElementDifficultyEnum.Easy);
        Element3 = new ElementViewModel("real world example of measurable code quality", "s", null!,"url", "s",
            "s", "s",ElementDifficultyEnum.Easy);
        
        Collection = new List<IDisplayableObject>
        {
            World1, World2, World3, World4,
            Space1, Space2, Space3, Space4,
            Element1, Element2, Element3,
        };
        
    }

    [Test]
    public void ToolboxResultFilter_UserExplanationText_ReturnsCorrectText()
    {
        var expected = 
@"Enter a search term to filter objects containing it in their name. Case is ignored.
A search term beginning with ""world"", ""space"", or ""element"" will only match those types of objects.
Example: ""world:basics"" will match all worlds containing ""basics"" in their name.
Search terms can be quoted to search them literally, ignoring the above rules.
";
        
        var systemUnderTest = new ToolboxResultFilter();
        
        Assert.That(systemUnderTest.UserExplanationText, Is.EqualTo(expected));
    }

    private class FilterCollectionFiltersCorrectlyTestCases : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            //shortcuts
            var worlds = new IDisplayableObject[] { World1, World2, World3, World4 };
            var spaces = new IDisplayableObject[] { Space1, Space2, Space3, Space4 };
            var elements = new IDisplayableObject[] { Element1, Element2, Element3 };
            
            //test cases
            //basic cases
            yield return new object[] { "metric", new IDisplayableObject[] { World1, Space1, Element2 } };
            yield return new object[] { " metric", new IDisplayableObject[] { World1, Space1, Element2 } };
            yield return new object[] { "metric ", new IDisplayableObject[] { World1, Space1, Element2 } };
            yield return new object[] { " metric ", new IDisplayableObject[] { World1, Space1, Element2 } };
            yield return new object[] { "e", new IDisplayableObject[] { World1, World3, World4, Space1, Space2, Space3, Space4, Element1, Element2, Element3} };
            yield return new object[] { " e", new IDisplayableObject[] { World1, World3, World4, Space1, Space2, Space3, Space4, Element1, Element2, Element3} };
            yield return new object[] { "e ", new IDisplayableObject[] { World1, World3, World4, Space1, Space2, Space3, Space4, Element1, Element2, Element3} };
            yield return new object[] { " e ", new IDisplayableObject[] { World1, World3, World4, Space1, Space2, Space3, Space4, Element1, Element2, Element3} };
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
            yield return new object[] { "world:metrics", new IDisplayableObject[] { World1 } };
            yield return new object[] { "world:  metrics", new IDisplayableObject[] { World1 } };
            yield return new object[] { "       world:metrics   ", new IDisplayableObject[] { World1 } };
            yield return new object[] { "       world:   metrics   ", new IDisplayableObject[] { World1 } };
            yield return new object[] { "space:unit", new IDisplayableObject[] { Space2 } };
            yield return new object[] { "space:  unit", new IDisplayableObject[] { Space2 } };
            yield return new object[] { "element:ulation", new IDisplayableObject[] { Element2 } };
            yield return new object[] { "element:   ulation", new IDisplayableObject[] { Element2 } };
            yield return new object[] { @"""element""", new IDisplayableObject[] { Space4 } };
            yield return new object[] { @"""space""", new IDisplayableObject[] { World4 } };
            yield return new object[] { @"""world""", new IDisplayableObject[] { Element3 } };
            yield return new object[] { @"""   element  """, new IDisplayableObject[] { Space4 } };
            yield return new object[] { @"""   space    """, new IDisplayableObject[] { World4 } };
            yield return new object[] { @"""   world         """, new IDisplayableObject[] { Element3 } };

        }
    }
    
    [Test]
    [TestCaseSource(typeof(FilterCollectionFiltersCorrectlyTestCases))]
    public void ToolboxResultFilter_FilterCollection_FiltersCorrectly(string inputString,
        IEnumerable<IDisplayableObject> expected)
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