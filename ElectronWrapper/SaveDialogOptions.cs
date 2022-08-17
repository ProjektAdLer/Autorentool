using System.Linq;

namespace ElectronWrapper;

public class SaveDialogOptions
{
    public SaveDialogOptions()
    {
        InnerSaveDialogOptions = new ElectronNET.API.SaveDialogOptions();
    }

    internal SaveDialogOptions(ElectronNET.API.SaveDialogOptions innerSaveDialogOptions)
    {
        InnerSaveDialogOptions = innerSaveDialogOptions;
    }
    
    internal readonly ElectronNET.API.SaveDialogOptions InnerSaveDialogOptions;

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