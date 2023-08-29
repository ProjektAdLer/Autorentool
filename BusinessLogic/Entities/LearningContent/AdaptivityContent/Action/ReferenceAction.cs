namespace BusinessLogic.Entities.LearningContent.AdaptivityContent.Action;

public class ReferenceAction
{
    public ReferenceAction(ILearningElement reference)
    {
        Reference = reference;
    }

    //TODO: decide if we really want to design this this way
    public ILearningElement Reference { get; set; }
}