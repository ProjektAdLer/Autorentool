using H5pPlayer.BusinessLogic.Entities;

namespace H5pPlayer.BusinessLogic.UseCases.DisplayH5p;

public interface IDisplayH5pUc
{
    Task StartToDisplayH5p(H5pEntity h5pEntity);
    void TerminateDisplayH5p();
    H5pEntity? H5pEntity { get; set; }
}