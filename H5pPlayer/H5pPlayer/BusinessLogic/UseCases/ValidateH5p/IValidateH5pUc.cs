using H5pPlayer.BusinessLogic.Entities;

namespace H5pPlayer.BusinessLogic.UseCases.ValidateH5p;

public interface IValidateH5pUc
{
    Task StartToValidateH5p(H5pEntity h5pEntity);
    void ValidateH5p(ValidateH5pTO validateH5pTO);

    void SetActiveH5pStateToNotUsable();
    void SetActiveH5pStateToPrimitive();
    void SetActiveH5pStateToCompletable();
    
    H5pEntity H5pEntity { get; set; }
}