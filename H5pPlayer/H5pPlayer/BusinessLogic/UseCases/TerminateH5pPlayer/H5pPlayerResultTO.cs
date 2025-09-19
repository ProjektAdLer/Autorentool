namespace H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;

public struct H5pPlayerResultTO
{
    public H5pPlayerResultTO(string activeH5pState)
    {
        ActiveH5pState = activeH5pState;
    }

    public string ActiveH5pState { get; }
}


