namespace H5pPlayer.BusinessLogic.UseCases.ValidateH5p;

public interface IValidateH5pUcOutputPort
{
    void SetH5pIsCompletable();
    void SetH5pActiveStateToNotUsable();
    void SetH5pActiveStateToPrimitive();
    void SetH5pActiveStateToCompletable();
}