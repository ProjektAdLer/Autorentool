namespace Presentation.View.Layout;

public interface ISidebar
{
    void SetSidebarItem(SidebarItem sidebarItem);
    SidebarItem CurrentItem { get; }
    Side Side { get; }
    void ClearSidebarItem();
}