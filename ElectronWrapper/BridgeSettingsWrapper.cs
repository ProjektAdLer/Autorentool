using ElectronNET.API;

namespace ElectronWrapper;

class BridgeSettingsWrapper : IBridgeSettingsWrapper
{
    /// <summary>
    /// Gets the socket port.
    /// </summary>
    /// <value>
    /// The socket port.
    /// </value>
    public string SocketPort => BridgeSettings.SocketPort;

    /// <summary>
    /// Gets the web port.
    /// </summary>
    /// <value>
    /// The web port.
    /// </value>
    public static string WebPort => BridgeSettings.WebPort;
}