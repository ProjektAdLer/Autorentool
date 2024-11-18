using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;
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
    public void TerminateH5pStandalone()
    {
        var mockJsRuntime = Substitute.For<IJSRuntime>();
        var systemUnderTest = CreateSystemUnderTest(mockJsRuntime);

        systemUnderTest.TerminateH5pPlayer();
        
         mockJsRuntime.Received().InvokeAsync<IJSVoidResult>("terminateH5pPlayer");
    }
    
    private static TerminateH5pPlayerUc CreateSystemUnderTest(IJSRuntime jsRuntime = null,
        IFileSystemDataAccess fileSystemDataAccess = null)
    {
        jsRuntime = jsRuntime ?? Substitute.For<IJSRuntime>();
        fileSystemDataAccess ??= Substitute.For<IFileSystemDataAccess>();
        var systemUnderTest = new TerminateH5pPlayerUc(jsRuntime, fileSystemDataAccess);
        return systemUnderTest;
    }

    
    
    // mockJsRuntime.Received().InvokeAsync<IJSVoidResult>("terminateH5pPlayer");

}