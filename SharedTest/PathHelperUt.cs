using NUnit.Framework;
using Shared;

namespace SharedTest;

[TestFixture]
public class PathHelperUt
{
    [Test]
    [TestCase("test.txt", ExpectedResult = "test.txt")]
    [TestCase("test. txt", ExpectedResult = "test.txt")]
    [TestCase("test .txt", ExpectedResult = "test_.txt")]
    [TestCase("test  .txt", ExpectedResult = "test__.txt")]
    [TestCase("test . txt", ExpectedResult = "test_.txt")]
    [TestCase("test  .  txt", ExpectedResult = "test__.txt")]
    [TestCase("test .  txt", ExpectedResult = "test_.txt")]
    public string TrimFileName_WithFileNameWithSpaces_ReturnsFileNameWithoutSpaces(string input)
    {
        return PathHelper.ReplaceSpaceWithUnderscoreIn(input);
    }
}