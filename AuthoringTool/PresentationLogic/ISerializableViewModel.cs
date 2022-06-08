namespace AuthoringTool.PresentationLogic;

/// <summary>
/// Label interface to ensure correct types only are passed in PresentationLogic save and load methods.
/// </summary>
internal interface ISerializableViewModel
{
    public string FileEnding { get; }
}