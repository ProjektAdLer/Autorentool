using System.Linq;

namespace ElectronWrapper;

public class SaveDialogOptions
{
    public SaveDialogOptions()
    {
        InnerSaveDialogOptions = new ElectronSharp.API.SaveDialogOptions();
    }

    internal SaveDialogOptions(ElectronSharp.API.SaveDialogOptions innerSaveDialogOptions)
    {
        InnerSaveDialogOptions = innerSaveDialogOptions;
    }
    
    internal readonly ElectronSharp.API.SaveDialogOptions InnerSaveDialogOptions;

    public string Title
    {
        get => InnerSaveDialogOptions.Title;
        set => InnerSaveDialogOptions.Title = value;
    }

    public string DefaultPath
    {
        get => InnerSaveDialogOptions.DefaultPath;
        set => InnerSaveDialogOptions.DefaultPath = value;
    }

    public string ButtonLabel
    {
        get => InnerSaveDialogOptions.ButtonLabel;
        set => InnerSaveDialogOptions.ButtonLabel = value;
    }

    public FileFilter[] Filters
    {
        get => InnerSaveDialogOptions.Filters.Select(f => new FileFilter(f)).ToArray();
        set => InnerSaveDialogOptions.Filters = value.Select(f => f.InnerFileFilter).ToArray();
    }
}