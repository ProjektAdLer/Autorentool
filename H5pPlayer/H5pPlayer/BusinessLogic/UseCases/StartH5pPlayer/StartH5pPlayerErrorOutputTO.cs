namespace H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;

public struct  StartH5pPlayerErrorOutputTO
{
    public StartH5pPlayerErrorOutputTO(
        string invalidH5pZipSourcePath,
        string h5PZipSourcePathErrorText)
    {
        InvalidH5pZipSourcePath = invalidH5pZipSourcePath;
        H5pZipSourcePathErrorText = h5PZipSourcePathErrorText;
    }


    public string InvalidH5pZipSourcePath { get; set; }
    
    public string H5pZipSourcePathErrorText { get; set; }
}