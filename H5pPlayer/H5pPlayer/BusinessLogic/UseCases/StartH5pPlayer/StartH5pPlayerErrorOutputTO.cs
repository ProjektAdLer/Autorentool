namespace H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;

public struct  StartH5pPlayerErrorOutputTO
{
    public StartH5pPlayerErrorOutputTO(
        string invalidPath,
        string errorTextForInvalidPath)
    {
        InvalidPath = invalidPath;
        ErrorTextForInvalidPath = errorTextForInvalidPath;
    }


    public string InvalidPath { get; set; }
    
    public string ErrorTextForInvalidPath { get; set; }
}