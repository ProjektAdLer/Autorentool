using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;
using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;
using NSubstitute;

namespace H5pPlayerTest.BusinessLogic.UseCases.TerminateH5pPlayer;

[TestFixture]
public class TerminateH5pPlayerUcUt
{
    [Test]
    public async Task  CleanH5pFolderInWwwroot()
    {
        var directoryForCleaning = Path.Combine("wwwroot", "H5pStandalone", "h5p-folder");
        var mockFileSystemDataAccess = Substitute.For<IFileSystemDataAccess>();
        mockFileSystemDataAccess.DirectoryExists(
            Arg.Is<string>(path => path.Contains(directoryForCleaning))).Returns(true);
        var systemUnderTest = CreateSystemUnderTest(null!,mockFileSystemDataAccess);
        
        await systemUnderTest.TerminateH5pPlayer();
        
        mockFileSystemDataAccess.Received().DeleteAllFilesAndDirectoriesIn(
            Arg.Is<string>(path => path.Contains(directoryForCleaning)));
    }
    
    [Test]
    public async Task TerminateH5pJavaScriptPlayer()
    {
        var mockJavaScriptAdapter= Substitute.For<ICallJavaScriptAdapter>();
        var systemUnderTest = CreateSystemUnderTest(mockJavaScriptAdapter);

        await systemUnderTest.TerminateH5pPlayer();
        
        await mockJavaScriptAdapter.Received().TerminateH5pJavaScriptPlayer();
    }

    private static TerminateH5pPlayerUc CreateSystemUnderTest(
        ICallJavaScriptAdapter? callJavaScriptAdapter = null!,
        IFileSystemDataAccess? fileSystemDataAccess = null!)
    {
        callJavaScriptAdapter  ??= Substitute.For<ICallJavaScriptAdapter>();
        fileSystemDataAccess ??= Substitute.For<IFileSystemDataAccess>();
        var systemUnderTest = new TerminateH5pPlayerUc(callJavaScriptAdapter, fileSystemDataAccess);
        return systemUnderTest;
    }
}