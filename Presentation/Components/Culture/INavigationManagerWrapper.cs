namespace Presentation.Components.Culture;

public interface INavigationManagerWrapper
{
    public string Uri { get; }
    public void NavigateTo(string uri, bool forceLoad);
}