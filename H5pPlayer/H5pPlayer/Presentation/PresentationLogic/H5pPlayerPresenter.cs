using H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;
using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;

namespace H5pPlayer.Presentation.PresentationLogic;

public class H5pPlayerPresenter : IStartH5pPlayerUCOutputPort, IValidateH5pUcOutputPort
{
    public H5pPlayerViewModel? H5pPlayerVm { get; }

    public H5pPlayerPresenter(H5pPlayerViewModel? h5PPlayerVm)
    {
        H5pPlayerVm = h5PPlayerVm;
    }

    public void ErrorOutput(StartH5pPlayerErrorOutputTO startH5PPlayerErrorOutputTo)
    {
        H5pPlayerVm!.InvalidPathErrorVm.ErrorTextForInvalidPath = startH5PPlayerErrorOutputTo.ErrorTextForInvalidPath;
        H5pPlayerVm!.InvalidPathErrorVm.InvalidPath = startH5PPlayerErrorOutputTo.InvalidPath;
        H5pPlayerVm!.InvalidPathErrorVm.InvalidPathErrorIsActive = true;
    }

    public void SetH5pIsCompletable()
    {
        H5pPlayerVm!.IsCompletable = true;
    }
}