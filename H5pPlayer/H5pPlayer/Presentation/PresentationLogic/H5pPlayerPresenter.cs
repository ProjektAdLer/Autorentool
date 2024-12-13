using H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;
using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;

namespace H5pPlayer.Presentation.PresentationLogic;

public class H5pPlayerPresenter : IStartH5pPlayerUCOutputPort, IValidateH5pUcOutputPort
{
    public void ErrorOutput(StartH5pPlayerErrorOutputTO startH5PPlayerErrorOutputTo)
    {
        throw new NotImplementedException();
    }

    public void SetH5pIsCompletable()
    {
        throw new NotImplementedException();
    }
}