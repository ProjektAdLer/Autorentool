namespace ElectronWrapper;

interface IWindowManagerWraperFactory
{
    ElectronWrapper.WindowManagerWrapper Create();
}

class WindowManagerWraperFactory : IWindowManagerWraperFactory
{

    public WindowManagerWrapper Create() { return new WindowManagerWrapper(); }
}