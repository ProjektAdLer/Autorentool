using Shared.Theme;

namespace Presentation.PresentationLogic.WorldThemeSelect
{
    /// <summary>
    /// Minimales Contract-Interface für beliebige Form-Modelle,
    /// die eine WorldTheme-Auswahl tragen.
    /// </summary>
    public interface IWorldThemeForm
    {
        WorldTheme WorldTheme { get; set; }
    }
}