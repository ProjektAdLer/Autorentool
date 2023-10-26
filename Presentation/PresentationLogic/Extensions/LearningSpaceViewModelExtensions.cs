using Presentation.PresentationLogic.LearningSpace;
using Shared.Extensions;

namespace Presentation.PresentationLogic.Extensions;

public static class LearningSpaceViewModelExtensions
{
    /// <summary>
    /// Returns all spaces that are reachable from the initial space.
    /// </summary>
    public static IEnumerable<ILearningSpaceViewModel> GetFollowingSpaces(this ILearningSpaceViewModel initialSpace)
    {
        var queue = new Queue<IObjectInPathWayViewModel>(new[] { initialSpace });
        var retval = new List<ILearningSpaceViewModel>();
        while (queue.Any())
        {
            var objInPathway = queue.Dequeue();
            queue.EnqueueRange(objInPathway.OutBoundObjects);
            var followingSpaces = objInPathway.OutBoundObjects.Where(obj => obj is ILearningSpaceViewModel)
                .Cast<ILearningSpaceViewModel>();
            retval.AddRange(followingSpaces);
        }

        return retval;
    }
}