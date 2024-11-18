namespace H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;

public interface IStartH5pPlayerUCInputPort
{
    Task StartH5pPlayer(StartH5pPlayerInputTO displayH5PInputTo);
}