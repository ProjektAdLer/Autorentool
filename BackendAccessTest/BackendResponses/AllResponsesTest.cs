using ApiAccess.BackendEntities;
using AutoBogus;
using FluentAssertions;
using Force.DeepCloner;
using NUnit.Framework;

namespace BackendAccessTest.WebApiResponses;

public class AllResponsesTest
{
    [TestCaseSource(nameof(GetTestCases))]
    public void ResponsesGetterAndSetter<T>(T _)
    {
        // Arrange
        var testClass = AutoFaker.Generate<T>();

        // Recursively clone the object
        var clone = testClass.DeepClone();

        // Assert
        clone.Should().BeEquivalentTo(testClass);
    }


    private static IEnumerable<TestCaseData> GetTestCases()
    {
        yield return new TestCaseData(new ErrorBE());
        yield return new TestCaseData(new UserTokenBE());
        yield return new TestCaseData(new UserInformationBE());
    }
}