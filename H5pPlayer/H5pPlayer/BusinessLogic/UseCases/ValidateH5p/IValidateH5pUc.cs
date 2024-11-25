using H5pPlayer.BusinessLogic.Entities;

namespace H5pPlayer.BusinessLogic.UseCases.ValidateH5p;

public interface IValidateH5pUc
{
    Task StartToValidateH5p(H5pEntity h5pEntity);
}