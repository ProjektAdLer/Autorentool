using System.IO.Abstractions;

namespace Presentation.PresentationLogic.MyLearningWorlds;

public class FileInfoFullNameComparer : IEqualityComparer<IFileInfo?>
{
    public bool Equals(IFileInfo? x, IFileInfo? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.FullName == y.FullName;
    }

    public int GetHashCode(IFileInfo obj)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        return obj.FullName != null ? obj.FullName.GetHashCode() : 0;
    }
}