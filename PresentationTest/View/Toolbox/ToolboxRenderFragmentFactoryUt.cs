using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Toolbox;
using Presentation.View.Toolbox;
using Shared;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.Toolbox;

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
        _testContext.Services.AddSingleton(_toolboxController);
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
        var cache = new ToolboxMemoryCacheMock();
        
        var systemUnderTest = GetTestableToolboxRenderFragmentFactory(cache:cache);

        var firstPass = systemUnderTest.GetRenderFragment(obj);
        var firstPassRendered = _testContext.Render(firstPass);
        firstPassRendered.MarkupMatches(expectedMarkup);
        
        //assert that entry was saved (in other words, Set was called)
        Assert.That(cache.Entries.Any(entry => entry.Key == obj && (RenderFragment)entry.Value == firstPass));

        var secondPass = systemUnderTest.GetRenderFragment(obj);
        var secondPassRendered = _testContext.Render(secondPass);
        secondPassRendered.MarkupMatches(expectedMarkup);
        
        //assert that entry was retrieved
        Assert.That(secondPass, Is.SameAs(firstPass));
        Assert.That(secondPass, Is.SameAs(cache.Entries.First(entry => entry.Key == obj).Value));
    }

    [Test]
    public void ToolboxRenderFragmentFactory_GetRenderFragment_ThrowsExceptionOnInvalidObject()
    {
        var systemUnderTest = GetTestableToolboxRenderFragmentFactory();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            systemUnderTest.GetRenderFragment(Substitute.For<IDisplayableLearningObject>()));
    }


    private IAbstractToolboxRenderFragmentFactory GetTestableToolboxRenderFragmentFactory(
        ILogger<ToolboxRenderFragmentFactory>? logger = null, IMemoryCache? cache = null)
    {
        logger ??= Substitute.For<ILogger<ToolboxRenderFragmentFactory>>();
        cache ??= new MemoryCache(new MemoryCacheOptions());
        return new ToolboxRenderFragmentFactory(logger, cache);
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
                new LearningElementViewModel("another name", "an", null!, "authors", "description", "goals",LearningElementDifficultyEnum.Easy),
                @"<div class=""col-3 element text-center text-wrap learning-element"">
another name
<br/>
    <p>I am an element</p>
</div>"
            };
        }
    }

    private class ToolboxMemoryCacheMock : IMemoryCache
    {
        internal ToolboxMemoryCacheMock()
        {
            Entries = new List<ToolboxCacheEntryMock>();
        }
        
        internal IList<ToolboxCacheEntryMock> Entries { get; }
        public void Dispose()
        {
        }

        public ICacheEntry CreateEntry(object key)
        {
            var entry = new ToolboxCacheEntryMock { Key = key };
            Entries.Add(entry);
            return entry;
        }

        public void Remove(object key)
        {
        }

        public bool TryGetValue(object key, out object value)
        {
            var entry = Entries.FirstOrDefault(entry => entry.Key == key);
            if (entry?.Value is null)
            {
                value = null!;
                return false;
            }

            value = entry.Value;
            return true;
        }
    }
    
    private class ToolboxCacheEntryMock : ICacheEntry
    {
        public void Dispose()
        {
        }

        public DateTimeOffset? AbsoluteExpiration { get; set; }
        public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public IList<IChangeToken>? ExpirationTokens { get; }
        public object? Key { get; set; }
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public IList<PostEvictionCallbackRegistration>? PostEvictionCallbacks { get; }
        public CacheItemPriority Priority { get; set; }
        public long? Size { get; set; }
        public TimeSpan? SlidingExpiration { get; set; }
        public object? Value { get; set; }
    }
}