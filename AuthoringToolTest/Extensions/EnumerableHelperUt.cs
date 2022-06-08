using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AuthoringTool.Extensions;
using NUnit.Framework;

namespace AuthoringToolTest.Extensions;

[TestFixture]
public class EnumerableHelperUt
{
    [Test]
    [TestCaseSource(typeof(EnumerableHelperSplitCases))]
    public void EnumerableHelper_Split_SplitsCorrectly(IEnumerable<int> collection, uint length, IEnumerable<IEnumerable<int>> expected)
    {
        //no systemUnderTest because its a static method
        var actual = collection.Split(length);
        
        CollectionAssert.AreEqual(expected, actual);
    }

    [Test]
    public void EnumerableHelper_Split_ThrowsForLengthZero()
    {
        var input = new[] { 1, 2, 3 };

        var ex = Assert.Throws<ArgumentException>(() => _ = input.Split(0).ToArray());
        Assert.Multiple(() =>
        {
            Assert.That(ex!.Message, Is.EqualTo("Size cannot be 0 (Parameter 'size')"));
            Assert.That(ex.ParamName, Is.EqualTo("size"));
        });
    }
}

internal class EnumerableHelperSplitCases : IEnumerable
{
    public IEnumerator GetEnumerator()
    {
        yield return new object[] //normal case
        {
            new[] { 1, 2, 3, 1, 2, 3 },
            (uint)3,
            new[] { new[] { 1, 2, 3 }, new[] { 1, 2, 3 } }
        };
        yield return new object[] //empty input
        {
            Array.Empty<int>(),
            (uint)4,
            Array.Empty<int[]>()
        };
        yield return new object[] //size 1
        {
            new[] { 1, 2, 3, 4, 5 },
            (uint)1,
            new[] { new[] { 1 }, new[] { 2 }, new[] { 3 }, new[] { 4 }, new[] { 5 } }
        };
        yield return new object[] //size exactly size of input
        {
            new[] { 1, 2, 3 },
            (uint)3,
            new[] { new[] { 1, 2, 3 } }
        };
        yield return new object[] //size larger than size of input
        {
            new[] { 1, 2, 3 },
            (uint)4,
            new[] { new[] { 1, 2, 3 } }
        };
    }
}