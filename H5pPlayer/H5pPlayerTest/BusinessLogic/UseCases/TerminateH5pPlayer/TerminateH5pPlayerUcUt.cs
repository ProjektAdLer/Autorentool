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
    public void CleanH5pFolderInWwwroot()
    {
        var mockFileSystemDataAccess = Substitute.For<IFileSystemDataAccess>();
        var systemUnderTest = CreateSystemUnderTest(null,mockFileSystemDataAccess);
        var directoryForCleaning = @"wwwroot\H5pStandalone\h5p-folder";
        
        systemUnderTest.TerminateH5pPlayer();
        
        mockFileSystemDataAccess.Received().DeleteAllFilesInDirectory(
            Arg.Is<string>(path => path.Contains(directoryForCleaning)));
    }
    
    [Test]
    public void TerminateH5pJavaScriptPlayer()
    {
        var mockJavaScriptAdapter= Substitute.For<IJavaScriptAdapter>();
        var systemUnderTest = CreateSystemUnderTest(mockJavaScriptAdapter);

        systemUnderTest.TerminateH5pPlayer();
        
        mockJavaScriptAdapter.Received().TerminateH5pJavaScriptPlayer();
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