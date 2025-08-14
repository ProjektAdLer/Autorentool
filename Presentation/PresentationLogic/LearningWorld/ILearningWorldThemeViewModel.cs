using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Shared.Theme;

namespace Presentation.PresentationLogic.WorldThemeSelect
{
    /// <summary>
    /// Anzeigelogik + Auswahlsteuerung für ThemeModelGridSelect.
    /// </summary>
    public interface ILearningWorldThemeViewModel
    {
        WorldTheme? Selected { get; set; }

        // Abhängigkeits-Properties (werden von der Razor-Komponente gesetzt)
        bool UsingBinding { get; set; }
        Func<WorldTheme, Task>? ValueChanged { get; set; }
        EditContext? EditContext { get; set; }
        Expression<Func<WorldTheme>>? ValueExpression { get; set; }
        IWorldThemeForm? FormModel { get; set; }

        string GetThemeImagePath(WorldTheme theme, bool large = false);
        string BuildItemCss(WorldTheme theme, bool disabled);
        Task PickAsync(WorldTheme theme, bool disabled);
    }
}