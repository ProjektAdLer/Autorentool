namespace Presentation.PresentationLogic.ElectronNET;

public class FileFilterProxy
{
    internal readonly string Name;
    internal readonly string[] Extensions;

    public FileFilterProxy(string name, string[] extensions)
    {
        Name = name;
        Extensions = extensions;
    }
}