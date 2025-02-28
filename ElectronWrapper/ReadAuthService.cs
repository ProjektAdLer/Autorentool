using ElectronSharp.API;

namespace ElectronWrapper;

public class ReadAuthService : IReadAuthService
{
    public void ReadAuth()
    {
        Electron.ReadAuth();
    }
}