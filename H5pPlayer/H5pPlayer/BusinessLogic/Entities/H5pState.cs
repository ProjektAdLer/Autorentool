namespace H5pPlayer.BusinessLogic.Entities;

/// <summary>
/// Documentation: (and why we dont use State-Pattern)
/// https://projektadler.github.io/Documentation/h5p-zust%C3%A4nde.html
/// </summary>
public enum H5pState
{
    NotValidated,
    NotUsable,
    Primitive,
    Completable,
}