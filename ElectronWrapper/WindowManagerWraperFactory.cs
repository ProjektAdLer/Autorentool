interface IWindowManagerWraperFactory
{
    ElectronWrapper.WindowManagerWrapper Create();
}

namespace ElectronWrapper
{
    class WindowManagerWraperFactory : IWindowManagerWraperFactory
    {

        public WindowManagerWrapper Create() { return new WindowManagerWrapper(); }
    }
}
