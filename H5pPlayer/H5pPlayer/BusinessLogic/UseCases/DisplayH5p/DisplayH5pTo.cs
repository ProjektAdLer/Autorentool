using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;

namespace H5pPlayer.BusinessLogic.UseCases.DisplayH5p;

public struct  DisplayH5pInputTo
{
    public DisplayH5pInputTo(
        string h5pJsonSourcePath)
    {
        H5pJsonSourcePath = h5pJsonSourcePath;
    }


    public string H5pJsonSourcePath { get; }
    
}