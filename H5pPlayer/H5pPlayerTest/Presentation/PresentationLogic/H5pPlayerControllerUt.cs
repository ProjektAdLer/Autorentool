using H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;
using H5pPlayer.Presentation.PresentationLogic;
using NSubstitute;

namespace H5pPlayerTest.Presentation.PresentationLogic;

[TestFixture]
public class H5pPlayerControllerUt
{
    [Test]
    public async Task StartH5pPlayer()
    {
        var mockStartH5pPlayerUcInputPort = Substitute.For<IStartH5pPlayerUCInputPort>();
        var systemUnderTest = new H5pPlayerController(mockStartH5pPlayerUcInputPort);

        await systemUnderTest.StartH5pPlayer("testPath1", "testPath2");
        
        await mockStartH5pPlayerUcInputPort.Received().StartH5pPlayer(Arg.Any<StartH5pPlayerInputTO>());
    }
}