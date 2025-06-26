namespace ElectronWrapper;

public class FileFilter
{
    public FileFilter()
    {
        InnerFileFilter = new ElectronSharp.API.Entities.FileFilter();
    }

    internal FileFilter(ElectronSharp.API.Entities.FileFilter innerFileFilter)
    {
        InnerFileFilter = innerFileFilter;
    }
    internal ElectronSharp.API.Entities.FileFilter InnerFileFilter { get; }

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