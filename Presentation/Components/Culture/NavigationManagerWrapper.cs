using Microsoft.AspNetCore.Components;

namespace Presentation.Components.Culture;

/// <summary>
/// Thin wrapper for NavigationManager to make it mockable.
/// </summary>
public class NavigationManagerWrapper : INavigationManagerWrapper
{
    private readonly NavigationManager _manager;

    public NavigationManagerWrapper(NavigationManager manager)
    {
        _manager = manager;
    }

    public string Uri => _manager.Uri;
    public void NavigateTo(string uri, bool forceLoad)
    {
        _manager.NavigateTo(uri, forceLoad);
    }
}