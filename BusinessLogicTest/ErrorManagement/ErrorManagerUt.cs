using BusinessLogic.ErrorManagement;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace BusinessLogicTest.ErrorManagement;

[TestFixture]
public class ErrorManagerUt
{
    [Test]
    public void LogAndRethrowError_LogsErrorAndRethrows()
    {
        var mockLogger = Substitute.For<ILogger<ErrorManager>>();
        var systemUnderTest = new ErrorManager(mockLogger);

        var dummyException = new Exception("Dummy Exception");

        var ex = Assert.Throws<Exception>(() => systemUnderTest.LogAndRethrowError(dummyException));

        Assert.That(ex.Message, Is.EqualTo(dummyException.Message));
    }
}