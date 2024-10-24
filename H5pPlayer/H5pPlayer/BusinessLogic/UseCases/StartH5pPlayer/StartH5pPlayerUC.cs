using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;
using H5pPlayer.BusinessLogic.Domain;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using Shared.Configuration;

namespace H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;

/// <summary>
/// Documentation:
/// https://projektadler.github.io/Documentation/hse7.html
/// </summary>
public class StartH5pPlayerUC : IStartH5pPlayerUCInputPort
{

    public StartH5pPlayerUC(
        IFileSystemDataAccess dataAccess,
        IDisplayH5pUC displayH5PUc,
        IStartH5pPlayerUCOutputPort startH5PPlayerUcOutputPort)
    {
        FileSystemDataAccess = dataAccess;
        DisplayH5pUC = displayH5PUc;
        StartH5pPlayerUcOutputPort = startH5PPlayerUcOutputPort;
        H5pEntity = null;
    }


    /// <summary>
    /// Was für pfade kommen an:
    /// ContentFIles
    ///     ZipSourcePath: C:\Users\%USERPROFILE%\AppData\Roaming\AdLerAuthoring\ContentFiles\Accordion_Test.h5p
    /// 
    /// UnzippedH5psPath: https://localhost:8001/H5pStandalone/h5p-folder
    ///
    ///
    /// H5pJsonSourcePath rein in
    ///     Aus dem ZipSourcePath extrahieren
    ///     das könnte man z.b. in der Entity machen somit bräuchte die Entity nur einen getter
    ///     Aber achtung erst muss die Zip Datei in den H5pOrdner der H5pStandalone in wwwroot des
    ///     Autorentools implementiert werden
    ///
    ///     
    /// 
    ///  JavascriptAdapter aufräumen, -> json path in jsonpath inentitiy
    ///
    ///
    /// Vgl Datei ContentFilesAdd in Presentation.Componentes.ContentFiles
    ///  
    ///
    /// 1. 
    /// 
    /// </summary>
    public void StartH5pPlayer(StartH5pPlayerInputTO displayH5PInputTo)
    {
        MapTOtoEntity(displayH5PInputTo);
        ExtractZippedSourceH5pToTemporaryFolder();
        DisplayH5pUC.StartToDisplayH5pUC(H5pEntity);
    }
    
    
    private void MapTOtoEntity(StartH5pPlayerInputTO startH5pPlayerInputTo)
    {
        CreateH5pEntity();
        H5pEntity.ActiveDisplayMode = startH5pPlayerInputTo.DisplayMode;
        MapPaths(startH5pPlayerInputTo);
    }

    private void MapPaths(StartH5pPlayerInputTO startH5pPlayerInputTo)
    {
        try
        {
            H5pEntity.H5pZipSourcePath = startH5pPlayerInputTo.H5pZipSourcePath;
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
        var errorOutputTo = new StartH5pPlayerErrorOutputTO();
        errorOutputTo.InvalidPath = invalidPath;
        errorOutputTo.ErrorTextForInvalidPath = errorMessage;
        StartH5pPlayerUcOutputPort.ErrorOutput(errorOutputTo);
    }
    
    
    private void ExtractZippedSourceH5pToTemporaryFolder()
    {
        var destinationDirectoryName = BuildDestinationDirectoryName();
        FileSystemDataAccess.ExtractZipFile(H5pEntity.H5pZipSourcePath, destinationDirectoryName);
    }
    /// <summary>
    /// to reach the wwwroot-directory we need a path like that:
    /// C:\Users\%USERPROFILE%\Documents\GitHub\Autorentool\AuthoringTool\wwwroot\H5pStandalone\h5p-folder
    /// We get this from: <see cref="Environment.CurrentDirectory"/>
    /// </summary>
    private string BuildDestinationDirectoryName()
    {
        string[] paths = new string[]
        {
            Environment.CurrentDirectory,
            "wwwroot",
            "H5pStandalone",
            "h5p-folder",
            Path.GetFileNameWithoutExtension(H5pEntity.H5pZipSourcePath),
        };
        var destinationDirectoryName = Path.Combine(paths);
        return destinationDirectoryName;
    }
    


    private void CreateH5pEntity()
    {
        H5pEntity = new H5pEntity();
    }

    
   
    internal IFileSystemDataAccess FileSystemDataAccess { get; }
    internal IDisplayH5pUC DisplayH5pUC { get; }
    internal H5pEntity H5pEntity { get; set; }
    internal IStartH5pPlayerUCOutputPort StartH5pPlayerUcOutputPort { get;  }
}