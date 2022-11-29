namespace ElectronWrapper;

interface IWindowManagerWraperFactory
{
    WindowManagerWrapper Create();
}

class WindowManagerWraperFactory : IWindowManagerWraperFactory
{

    public WindowManagerWrapper Create() { return new WindowManagerWrapper(); }
}