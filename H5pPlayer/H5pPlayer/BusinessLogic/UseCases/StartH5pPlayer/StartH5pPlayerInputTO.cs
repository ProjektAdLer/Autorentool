using H5pPlayer.BusinessLogic.Entities;

namespace H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;

public struct  StartH5pPlayerInputTO
{
    public StartH5pPlayerInputTO(
        H5pDisplayMode displayMode,
        string h5pZipSourcePath,
        string unzippedH5psPath   )
    {
        DisplayMode = displayMode;
        H5pZipSourcePath = h5pZipSourcePath;
        UnzippedH5psPath = unzippedH5psPath;
    }


    public H5pDisplayMode DisplayMode { get; }
    public string H5pZipSourcePath { get; }
    public string UnzippedH5psPath { get; }
}