namespace AuthoringToolLib.PresentationLogic;

/// <summary>
/// Label interface to ensure correct types only are passed in PresentationLogic save and load methods.
/// </summary>
public interface ISerializableViewModel
{
    public string FileEnding { get; }
}