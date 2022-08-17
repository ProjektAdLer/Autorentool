namespace ElectronWrapper;

interface IMenuWrapperFactory
{
    MenuWrapper Create();
}
class MenuWrapperFactory: IMenuWrapperFactory
{

    public MenuWrapper Create()
    {
        return new MenuWrapper();
    }
}