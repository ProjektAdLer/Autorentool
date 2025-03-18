using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;
using H5pPlayer.Presentation.PresentationLogic;
using NSubstitute;

namespace H5pPlayerTest.Presentation.PresentationLogic;

[TestFixture]
public class H5pPlayerControllerUt
{
    [TestCase(H5pDisplayMode.Display)]
    [TestCase(H5pDisplayMode.Validate)] 
    // ANF-ID: [HSE7]
    public async Task StartH5pPlayerToValidate(H5pDisplayMode displayMode)
    {
        var mockStartH5pPlayerUcInputPort = Substitute.For<IStartH5pPlayerUCInputPort>();
        var systemUnderTest = new H5pPlayerController(mockStartH5pPlayerUcInputPort);
        var h5pSourcePath = "h5pSourcePath";
        var unzippedH5psPath = "unzippedH5psPath";
        
        await systemUnderTest.StartH5pPlayer(displayMode, h5pSourcePath, unzippedH5psPath);
        
        var displayH5pTo = new StartH5pPlayerInputTO(displayMode, h5pSourcePath, unzippedH5psPath);
        await mockStartH5pPlayerUcInputPort.Received().StartH5pPlayer(displayH5pTo);
    }
}