using H5pPlayer.BusinessLogic.BusinessRules;

namespace H5pPlayer.BusinessLogic.Api.CleanupH5pPlayer;

public class CleanupH5pPlayerPortFactory: ICleanupH5pPlayerPortFactory
{
    public ICleanupH5pPlayerPort CreateCleanupH5pPlayerPort()
    {
        return new TemporaryH5psManager(new DataAccess.FileSystem.FileSystemDataAccess());
    }
}