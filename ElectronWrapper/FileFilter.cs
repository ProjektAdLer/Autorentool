namespace ElectronWrapper;

public class FileFilter
{
    public FileFilter()
    {
        InnerFileFilter = new ElectronNET.API.Entities.FileFilter();
    }

    internal FileFilter(ElectronNET.API.Entities.FileFilter innerFileFilter)
    {
        InnerFileFilter = innerFileFilter;
    }
    internal ElectronNET.API.Entities.FileFilter InnerFileFilter { get; }

    public string[] Extensions
    {
        get => InnerFileFilter.Extensions;
        set => InnerFileFilter.Extensions = value;
    }

    public string Name
    {
        get => InnerFileFilter.Name;
        set => InnerFileFilter.Name = value;
    }
}