namespace BusinessLogic.Entities.LearningContent.H5P;

public class H5PContent
{
    public H5PContentStateEnum State { get; set; }

    public H5PContent(H5PContentStateEnum state)
    {
        State = state;
    }

    public H5PContent()
    {
        State = H5PContentStateEnum.UNKNOWN;
    }
}