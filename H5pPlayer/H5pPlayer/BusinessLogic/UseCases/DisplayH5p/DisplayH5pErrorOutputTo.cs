namespace H5pPlayer.BusinessLogic.UseCases.DisplayH5p;

public struct  DisplayH5pErrorOutputTo
{
    public DisplayH5pErrorOutputTo(
        string invalidH5pJsonSourcePath,
        string h5PJsonSourcePathErrorText)
    {
        InvalidH5pJsonSourcePath = invalidH5pJsonSourcePath;
        H5pJsonSourcePathErrorText = h5PJsonSourcePathErrorText;
    }


    public string InvalidH5pJsonSourcePath { get; set; }
    
    public string H5pJsonSourcePathErrorText { get; set; }
}