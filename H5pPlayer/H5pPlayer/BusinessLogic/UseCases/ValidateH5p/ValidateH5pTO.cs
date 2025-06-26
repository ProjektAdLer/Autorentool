namespace H5pPlayer.BusinessLogic.UseCases.ValidateH5p;

public struct ValidateH5pTO
{
    public ValidateH5pTO(bool isValidationCompleted)
    {
        IsValidationCompleted = isValidationCompleted;
    }

    public bool IsValidationCompleted { get; }
}

