using System.Collections;
using System.Globalization;
using NUnit.Framework;
using Presentation.Components.LearningOutcomes;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components.LearningOutcomes;

[TestFixture]
public class GrammarOrderUt
{
    private TestContext _context;
    private const string Verb = "verb";
    private const string What = "what";

    [SetUp]
    public void Setup()
    {
        _context = new TestContext();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    [TestCaseSource(typeof(GrammarOrderTestCases), nameof(GrammarOrderTestCases.TestCases))]
    public string Render_Culture_ShowsVerbAndWhatContentInCorrectOrder(CultureInfo culture)
    {
        var sut = _context.RenderComponent<GrammarOrder>(pBuilder =>
            {
                pBuilder.Add(p => p.Culture, culture);
                pBuilder.Add(p => p.VerbContent, Verb);
                pBuilder.Add(p => p.WhatContent, What);
            }
        );
        
        return sut.Markup;
    }


    private class GrammarOrderTestCases
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(new CultureInfo("de-DE")).Returns($"{What}{Verb}");
                yield return new TestCaseData(new CultureInfo("en-DE")).Returns($"{Verb}{What}");
            }
        }
    }
}