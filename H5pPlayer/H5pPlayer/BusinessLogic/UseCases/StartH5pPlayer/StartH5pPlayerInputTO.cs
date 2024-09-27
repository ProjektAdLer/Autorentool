namespace H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;

public struct  StartH5pPlayerInputTO
{
    public StartH5pPlayerInputTO(
        string h5pJsonSourcePath)
    {
        H5pJsonSourcePath = h5pJsonSourcePath;
    }


    public string H5pJsonSourcePath { get; }
    
}