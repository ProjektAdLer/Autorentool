using ElectronNET.API;

namespace ElectronWrapper;

public class HybridSupportWrapper : IHybridSupportWrapper
{
    /// <summary>
    /// Gets a value indicating whether this instance is electron active.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is electron active; otherwise, <c>false</c>.
    /// </value>
    public bool IsElectronActive => HybridSupport.IsElectronActive;
}