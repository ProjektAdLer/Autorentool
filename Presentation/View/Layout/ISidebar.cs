namespace Presentation.View.Layout;

public interface ISidebar
{
    SidebarItem? CurrentItem { get; }
    Side Side { get; }
    void SetSidebarItem(SidebarItem sidebarItem);
    void ClearSidebarItem();
}