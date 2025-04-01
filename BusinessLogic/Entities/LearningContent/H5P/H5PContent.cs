namespace BusinessLogic.Entities.LearningContent.H5P;

public class H5PContent
{
    public H5PContentStateEnum _state { get; set; }

    public H5PContent(H5PContentStateEnum state)
    {
        if (!Enum.IsDefined(typeof(H5PContentStateEnum), state))
        {
            throw new ArgumentOutOfRangeException(nameof(state), state, "Invalid enum value provided.");
        }

        _state = state;
    }

    public H5PContent()
    {
        _state = H5PContentStateEnum.NOT_VALIDATED;
    }
}