using BusinessLogic.ErrorManagement;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Shared.Exceptions;

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

    [Test]
    public void LogAndRethrowUndoError_LogsErrorAndRethrows()
    {
        var mockLogger = Substitute.For<ILogger<ErrorManager>>();
        var systemUnderTest = new ErrorManager(mockLogger);

        var dummyException = new Exception("Dummy Exception");

        var ex = Assert.Throws<UndoException>(() => systemUnderTest.LogAndRethrowUndoError(dummyException));

        Assert.That(ex?.Message, Is.EqualTo(dummyException.Message));
        Assert.That(ex?.InnerException, Is.EqualTo(dummyException));
    }

    [Test]
    public void LogAndRethrowRedoError_LogsErrorAndRethrows()
    {
        var mockLogger = Substitute.For<ILogger<ErrorManager>>();
        var systemUnderTest = new ErrorManager(mockLogger);

        var dummyException = new Exception("Dummy Exception");

        var ex = Assert.Throws<RedoException>(() => systemUnderTest.LogAndRethrowRedoError(dummyException));

        Assert.That(ex?.Message, Is.EqualTo(dummyException.Message));
        Assert.That(ex?.InnerException, Is.EqualTo(dummyException));
    }

    [Test]
    public void LogAndRethrowGeneratorError_LogsErrorAndRethrows()
    {
        var mockLogger = Substitute.For<ILogger<ErrorManager>>();
        var systemUnderTest = new ErrorManager(mockLogger);

        var dummyException = new Exception("Dummy Exception");

        var ex = Assert.Throws<GeneratorException>(() => systemUnderTest.LogAndRethrowGeneratorError(dummyException));

        Assert.That(ex?.Message, Is.EqualTo(dummyException.Message));
        Assert.That(ex?.InnerException, Is.EqualTo(dummyException));
    }

    [Test]
    public void LogAndRethrowBackendAccessError_LogsErrorAndRethrows()
    {
        var mockLogger = Substitute.For<ILogger<ErrorManager>>();
        var systemUnderTest = new ErrorManager(mockLogger);

        var dummyException = new Exception("Dummy Exception");

        var ex = Assert.Throws<BackendException>(() => systemUnderTest.LogAndRethrowBackendAccessError(dummyException));

        Assert.That(ex?.Message, Is.EqualTo(dummyException.Message));
        Assert.That(ex?.InnerException, Is.EqualTo(dummyException));
    }
}