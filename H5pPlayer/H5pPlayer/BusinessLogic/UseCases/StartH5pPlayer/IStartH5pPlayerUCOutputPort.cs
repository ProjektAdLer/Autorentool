﻿namespace H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;

public interface IStartH5pPlayerUCOutputPort
{
    void ErrorOutput(StartH5pPlayerErrorOutputTO startH5PPlayerErrorOutputTo);
}