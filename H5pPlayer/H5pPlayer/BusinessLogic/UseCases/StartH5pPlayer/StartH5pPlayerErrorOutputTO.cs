namespace H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;

public struct  StartH5pPlayerErrorOutputTO
{
    public StartH5pPlayerErrorOutputTO(
        string invalidH5pJsonSourcePath,
        string h5PJsonSourcePathErrorText)
    {
        InvalidH5pJsonSourcePath = invalidH5pJsonSourcePath;
        H5pJsonSourcePathErrorText = h5PJsonSourcePathErrorText;
    }


    public string InvalidH5pJsonSourcePath { get; set; }
    
    public string H5pJsonSourcePathErrorText { get; set; }
}