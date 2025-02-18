namespace H5pPlayer.BusinessLogic.UseCases.DisplayH5p;

public struct CallJavaScriptAdapterTO
{
    public CallJavaScriptAdapterTO(
        string unzippedH5psPath,
        string zippedH5psPath)
    {
        UnzippedH5psPath = unzippedH5psPath;
        H5pZipSourcePath = zippedH5psPath;
    }
    public string UnzippedH5psPath { get;  }
    public string H5pZipSourcePath { get;  }
}