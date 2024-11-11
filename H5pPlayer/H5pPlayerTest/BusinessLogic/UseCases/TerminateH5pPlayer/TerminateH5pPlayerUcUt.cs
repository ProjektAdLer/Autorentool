using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;
using H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;
using NSubstitute;

namespace H5pPlayerTest.BusinessLogic.UseCases.TerminateH5pPlayer;

[TestFixture]
public class TerminateH5pPlayerUcUt
{
    [Test]
    public void CleanH5pFolderInWwwroot()
    {
        var mockFileSystemDataAccess = Substitute.For<IFileSystemDataAccess>();
        var systemUnderTest = new TerminateH5pPlayerUc(mockFileSystemDataAccess);
        var directoryForCleaning = @"wwwroot\H5pStandalone\h5p-folder";
        
        systemUnderTest.TerminateH5pPlayer();
        
        mockFileSystemDataAccess.Received().DeleteAllFilesInDirectory(
            Arg.Is<string>(path => path.Contains(directoryForCleaning)));
    }
    
    
  

}