using H5pPlayer.BusinessLogic.Api.CleanupH5pPlayer;
using H5pPlayer.BusinessLogic.BusinessRules;

namespace H5pPlayerTest.BusinessLogic.Api.CleanupH5pPlayer;

[TestFixture]
public class CleanupH5pPlayerPortUt
{

    [Test]
    public void CreateCleanupH5pPlayerPort()
    {
        var systemUnderTest = new CleanupH5pPlayerPortFactory();

        var cleanupH5pPlayer =  systemUnderTest.CreateCleanupH5pPlayerPort();
        
        Assert.That(cleanupH5pPlayer, Is.TypeOf<TemporaryH5PsInWwwrootManager>());
    }
}