using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;
using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;
using NSubstitute;

namespace H5pPlayerTest.BusinessLogic.UseCases.TerminateH5pPlayer;

[TestFixture]
public class TerminateH5pPlayerUcUt
{
    [Test]
    public async Task  CleanH5pFolderInWwwroot()
    {
        var mockFileSystemDataAccess = Substitute.For<IFileSystemDataAccess>();
        var systemUnderTest = CreateSystemUnderTest(null,mockFileSystemDataAccess);
        var directoryForCleaning = @"wwwroot\H5pStandalone\h5p-folder";
        
        await systemUnderTest.TerminateH5pPlayer();
        
        mockFileSystemDataAccess.Received().DeleteAllFilesInDirectory(
            Arg.Is<string>(path => path.Contains(directoryForCleaning)));
    }
    
    [Test]
    public async Task TerminateH5pJavaScriptPlayer()
    {
        var mockJavaScriptAdapter= Substitute.For<IJavaScriptAdapter>();
        var systemUnderTest = CreateSystemUnderTest(mockJavaScriptAdapter);

        await systemUnderTest.TerminateH5pPlayer();
        
        await mockJavaScriptAdapter.Received().TerminateH5pJavaScriptPlayer();
    }

    private static TerminateH5pPlayerUc CreateSystemUnderTest(
        IJavaScriptAdapter javaScriptAdapter = null,
        IFileSystemDataAccess fileSystemDataAccess = null)
    {
        javaScriptAdapter = javaScriptAdapter ?? Substitute.For<IJavaScriptAdapter>();
        fileSystemDataAccess ??= Substitute.For<IFileSystemDataAccess>();
        var systemUnderTest = new TerminateH5pPlayerUc(javaScriptAdapter, fileSystemDataAccess);
        return systemUnderTest;
    }
}