using H5pPlayer.BusinessLogic.Entities;

namespace H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;

public interface ITerminateH5pPlayerUcPort
{
    Task TerminateH5pPlayer(H5pState activeH5pState);
}