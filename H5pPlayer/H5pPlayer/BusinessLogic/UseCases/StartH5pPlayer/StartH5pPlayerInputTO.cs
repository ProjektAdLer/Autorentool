using H5pPlayer.BusinessLogic.Domain;

namespace H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;

public struct  StartH5pPlayerInputTO
{
    public StartH5pPlayerInputTO(
        H5pDisplayMode displayMode,
        string h5pJsonSourcePath)
    {
        DisplayMode = displayMode;
        H5pJsonSourcePath = h5pJsonSourcePath;
    }


    public H5pDisplayMode DisplayMode { get; }
    public string H5pJsonSourcePath { get; }
    
}