using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;
using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;
using NSubstitute;

namespace H5pPlayerTest.BusinessLogic.UseCases.TerminateH5pPlayer;

[TestFixture]
public class TerminateH5pPlayerUcUt
{
    /// <summary>
    /// Documentation for UnzippedH5psPath:
    /// <see cref="H5pEntity.UnzippedH5psPath"/>
    /// </summary>
    [Test]
    // ANF-ID: [HSE9]
    public async Task  CleanDirectoryForTemporaryUnzippedH5ps()
    {
        var directoryForCleaning = Path.Combine( "h5p-folder");
        var mockFileSystemDataAccess = Substitute.For<IFileSystemDataAccess>();
        mockFileSystemDataAccess.DirectoryExists(
            Arg.Is<string>(path => path.Contains(directoryForCleaning))).Returns(true);
        var systemUnderTest = CreateSystemUnderTest(null!,mockFileSystemDataAccess);
        
        await systemUnderTest.TerminateH5pPlayer(H5pState.Completable);
        
        mockFileSystemDataAccess.Received().DeleteAllFilesAndDirectoriesIn(
            Arg.Is<string>(path => path.Contains(directoryForCleaning)));
    }
    
    [Test]
    // ANF-ID: [HSE8]
    public async Task TerminateH5pJavaScriptPlayer()
    {
        var mockJavaScriptAdapter= Substitute.For<ICallJavaScriptAdapter>();
        var systemUnderTest = CreateSystemUnderTest(mockJavaScriptAdapter);

        await systemUnderTest.TerminateH5pPlayer(H5pState.Completable);
        
        await mockJavaScriptAdapter.Received().TerminateH5pJavaScriptPlayer();
    }
    
    [Test]
    // ANF-ID: [HSE8]
    public async Task SendResultToCallerSystem()
    {
        bool wasCalled = false;
        H5pPlayerResultTO? receivedResult = null;

        Action<H5pPlayerResultTO> onH5pPlayerFinished = result =>
        {
            wasCalled = true;
            receivedResult = result;
        };

        var mockJavaScriptAdapter = Substitute.For<ICallJavaScriptAdapter>();
        var systemUnderTest = CreateSystemUnderTest(
            mockJavaScriptAdapter,
            null!,
            onH5pPlayerFinished
        );

        await systemUnderTest.TerminateH5pPlayer(H5pState.Completable);

        Assert.That(wasCalled, Is.True);
        Assert.That(receivedResult, Is.Not.Null);
    }

    private static TerminateH5pPlayerUc CreateSystemUnderTest(
        ICallJavaScriptAdapter? callJavaScriptAdapter = null,
        IFileSystemDataAccess? fileSystemDataAccess = null,
        Action<H5pPlayerResultTO>? onH5pPlayerFinished = null)
    {
        callJavaScriptAdapter ??= Substitute.For<ICallJavaScriptAdapter>();
        fileSystemDataAccess ??= Substitute.For<IFileSystemDataAccess>();
        onH5pPlayerFinished ??= _ => { }; // Fake Lambda

        return new TerminateH5pPlayerUc(
            callJavaScriptAdapter,
            fileSystemDataAccess,
            onH5pPlayerFinished
        );
    }
}