using Shared.Extensions;

namespace BusinessLogic.Entities;

public class LearningSpaceExtensions
{
   public static IEnumerable<ILearningSpace> GetFollowingSpacesFrom(ILearningSpace initialSpace)
{
    var queue = new Queue<IObjectInPathWay>(new[] { initialSpace });
    var retval = new List<ILearningSpace>();

    TraverseLearningSpaces(queue, retval);

    return retval;
}

private static void TraverseLearningSpaces(Queue<IObjectInPathWay> queue, List<ILearningSpace> retval)
{
    while (queue.Any())
    {
        var objInPathway = queue.Dequeue();
        EnqueueOutboundObjects(queue, objInPathway.OutBoundObjects);
        AddLearningSpacesToRetVal(retval, objInPathway.OutBoundObjects);
    }
}

private static void EnqueueOutboundObjects(Queue<IObjectInPathWay> queue, IEnumerable<IObjectInPathWay> outboundObjects)
{
    queue.EnqueueRange(outboundObjects);
}

private static void AddLearningSpacesToRetVal(List<ILearningSpace> retval, IEnumerable<IObjectInPathWay> outboundObjects)
{
    var learningSpaces = outboundObjects.Where(obj => obj is ILearningSpace).Cast<ILearningSpace>();
    retval.AddRange(learningSpaces);
}

}