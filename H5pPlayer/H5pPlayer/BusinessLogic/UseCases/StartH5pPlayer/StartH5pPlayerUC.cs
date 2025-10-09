using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;
using H5pPlayer.BusinessLogic.BusinessRules;
using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;

namespace H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;

/// <summary>
/// Documentation:
/// https://projektadler.github.io/Documentation/hse7.html
/// </summary>
public class StartH5pPlayerUC : IStartH5pPlayerUCInputPort
{

    internal StartH5pPlayerUC(
        IValidateH5pUc validateH5PUc, 
        IFileSystemDataAccess dataAccess,
        IDisplayH5pUc displayH5PUc,
        IStartH5pPlayerUCOutputPort startH5PPlayerUcOutputPort)
    {
        ValidateH5PUc = validateH5PUc;
        FileSystemDataAccess = dataAccess;
        DisplayH5pUC = displayH5PUc;
        StartH5pPlayerUcOutputPort = startH5PPlayerUcOutputPort;
        H5pEntity = null;
        TemporaryH5pManager = new TemporaryH5psManager(dataAccess);
    }

    /// <summary>
    /// the paths:
    /// ContentFiles
    ///     ZipSourcePath: C:\Users\%USERPROFILE%\AppData\Roaming\AdLerAuthoring\ContentFiles\Accordion_Test.h5p
    /// 
    /// UnzippedH5psPath: https://localhost:8001/H5pStandalone/h5p-folder
    ///
    /// </summary>
    public async Task StartH5pPlayer(StartH5pPlayerInputTO displayH5PInputTo)
    {
        TemporaryH5pManager.CleanDirectoryForTemporaryUnzippedH5ps();
        MapTOtoEntity(displayH5PInputTo);
        ExtractZippedSourceH5pToTemporaryFolder();
        await IfUserWantsToValidateH5pStartToValidateElseStartToDisplay();
    }

    private void MapTOtoEntity(StartH5pPlayerInputTO startH5pPlayerInputTo)
    {
        CreateH5pEntity();
        H5pEntity!.ActiveDisplayMode = startH5pPlayerInputTo.DisplayMode;
        MapPaths(startH5pPlayerInputTo);
    }

    private void MapPaths(StartH5pPlayerInputTO startH5pPlayerInputTo)
    {
        try
        {
            H5pEntity!.H5pZipSourcePath = startH5pPlayerInputTo.H5pZipSourcePath;
            H5pEntity.UnzippedH5psPath = startH5pPlayerInputTo.UnzippedH5psPath;
        }
        catch (ArgumentException e)
        {
            HandleErrorOutputForInvalidPath(startH5pPlayerInputTo, e.Message);
        }
    }

    private void HandleErrorOutputForInvalidPath(StartH5pPlayerInputTO startH5PPlayerInputT0, string errorMessage)
    {
        if (errorMessage.Contains(nameof(H5pEntity.H5pZipSourcePath)))
        {
            CreateErrorOutputForInvalidPath(startH5PPlayerInputT0.H5pZipSourcePath, errorMessage);
        }
        else
        {
            CreateErrorOutputForInvalidPath(startH5PPlayerInputT0.UnzippedH5psPath, errorMessage);
        }
    }

    private void CreateErrorOutputForInvalidPath(string invalidPath, string errorMessage)
    {
        var errorOutputTo = new StartH5pPlayerErrorOutputTO(
            invalidPath, errorMessage);
        StartH5pPlayerUcOutputPort.ErrorOutput(errorOutputTo);
    }
    
    /// <summary>
    /// Todo: error management
    /// for example: excess denied ...
    /// </summary>
    private void ExtractZippedSourceH5pToTemporaryFolder()
    {
        FileSystemDataAccess.ExtractZipFile(H5pEntity!.H5pZipSourcePath, BuildTemporaryDirectoryFullNameForOneH5p());
    }

    private async Task IfUserWantsToValidateH5pStartToValidateElseStartToDisplay()
    {
        if (H5pEntity!.ActiveDisplayMode == H5pDisplayMode.Validate)
        {
            // first build GUI because we need the <div id="h5p-container"></div> active in DOM
            StartH5pPlayerUcOutputPort.StartToValidateH5p();
            await ValidateH5PUc.StartToValidateH5p(H5pEntity);
        }
        else
        {
            // first build GUI because we need the <div id="h5p-container"></div> active in DOM
            StartH5pPlayerUcOutputPort.StartToDisplayH5p();
            await DisplayH5pUC.StartToDisplayH5p(H5pEntity);
        }
    }

    
    private string BuildTemporaryDirectoryFullNameForOneH5p()
    {
        return TemporaryH5pManager.BuildTemporaryDirectoryFullNameForCurrentH5p(
            Path.GetFileNameWithoutExtension(H5pEntity!.H5pZipSourcePath));
    }
    


    private void CreateH5pEntity()
    {
        H5pEntity = new H5pEntity();
    }


    private IValidateH5pUc ValidateH5PUc { get; }
    private IFileSystemDataAccess FileSystemDataAccess { get; }
    private IDisplayH5pUc DisplayH5pUC { get; }
    internal H5pEntity? H5pEntity { get; private set; }
    private IStartH5pPlayerUCOutputPort StartH5pPlayerUcOutputPort { get;  }
    private TemporaryH5psManager TemporaryH5pManager { get; set; }

}