﻿using H5pPlayer.BusinessLogic.Entities;

namespace H5pPlayer.BusinessLogic.UseCases.DisplayH5p;

public interface IDisplayH5pUC
{
    Task StartToDisplayH5pUC(H5pEntity h5pEntity);
}