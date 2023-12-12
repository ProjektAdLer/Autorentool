using System.Collections.Generic;
using System.IO.Abstractions;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.MyLearningWorlds;

namespace PresentationTest.PresentationLogic.MyLearningWorlds;

[TestFixture]
public class FileInfoFullNameComparerUt
{
    private IEqualityComparer<IFileInfo> _systemUnderTest;

    [SetUp]
    public void Setup()
    {
        _systemUnderTest = new FileInfoFullNameComparer();
    }

    [Test]
    [TestCaseSource(nameof(FileInfoFullNameComparerTestCaseSource))]
    public bool Equals_ReturnsCorrectResult(IFileInfo? a, IFileInfo? b)
    {
        return _systemUnderTest.Equals(a, b);
        
    }

    private static IEnumerable<TestCaseData> FileInfoFullNameComparerTestCaseSource() 
    {
        yield return new TestCaseData(null,null).Returns(true);
        yield return new TestCaseData(null,Substitute.For<IFileInfo>()).Returns(false);
        yield return new TestCaseData(Substitute.For<IFileInfo>(),null).Returns(false);
        var sub1 = Substitute.For<IFileInfo>();
        sub1.FullName.Returns("sub1");
        yield return new TestCaseData(sub1, sub1).Returns(true);
        var sub2 = Substitute.For<IFileInfo>();
        sub2.FullName.Returns("sub2");
        yield return new TestCaseData(sub1, sub2).Returns(false);
    }
}