using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Shared.Theme;

namespace Presentation.PresentationLogic.WorldThemeSelect
{
    public class LearningWorldThemeViewModel : ILearningWorldThemeViewModel
    {
        // Zustand
        public WorldTheme? Selected { get; set; }

        // Von der UI injizierte Abhängigkeiten
        public bool UsingBinding { get; set; }
        public Func<WorldTheme, Task>? ValueChanged { get; set; }
        public EditContext? EditContext { get; set; }
        public Expression<Func<WorldTheme>>? ValueExpression { get; set; }
        public IWorldThemeForm? FormModel { get; set; }

        public string GetThemeImagePath(WorldTheme theme, bool large = false)
        {
            var suffix = large ? "_lg.png" : "_sm.png";
            return theme switch
            {
                WorldTheme.Suburb              => $"/CustomIcons/World/WorldTheme/Suburb/theme-bg-narrativeframework-suburb{suffix}",
                WorldTheme.CampusKempten       => $"/CustomIcons/World/WorldTheme/CampusKE/theme-bg-narrativeframework-campuske{suffix}",
                WorldTheme.CampusAschaffenburg => $"/CustomIcons/World/WorldTheme/CampusAB/theme-bg-narrativeframework-campusab{suffix}",
                WorldTheme.Company             => $"/CustomIcons/World/WorldTheme/Company/theme-bg-narrativeframework-company{suffix}",
                _ => ""
            };
        }

        public string BuildItemCss(WorldTheme theme, bool disabled)
        {
            var isSelected = Selected == theme && !disabled;

            // gleiche Logik wie vorher, nur ohne direkte Abhängigkeit vom Blazor-CssBuilder
            // (falls ihr CssBuilder als Util öffentlich habt, könnt ihr ihn hier verwenden.)
            var classes = "rounded-2xl transition-all";
            classes += !disabled ? " cursor-pointer" : " opacity-50 cursor-not-allowed";
            classes += isSelected ? " bg-adlerblue-600 shadow-lg" : (!disabled ? " bg-white hover:bg-adlergrey-100" : "");
            classes += (!isSelected || disabled) ? " border-default" : "";
            return classes.Trim();
        }

        public async Task PickAsync(WorldTheme theme, bool disabled)
        {
            if (disabled) return;

            if (UsingBinding)
            {
                // VM-internen Zustand aktualisieren
                Selected = theme;

                if (ValueChanged is not null)
                {
                    await ValueChanged.Invoke(theme);
                }

                // Validierungs-Trigger wie im Original
                if (EditContext is not null && ValueExpression is not null)
                {
                    var field = FieldIdentifier.Create(ValueExpression);
                    EditContext.NotifyFieldChanged(field);
                }
            }
            else
            {
                // Fallback: direkt ins FormModel schreiben (wie im Original)
                Selected = theme;
                if (FormModel is not null)
                {
                    FormModel.WorldTheme = theme;
                }
            }

            // Wichtig: StateHasChanged gehört in die Razor-Komponente, NICHT ins ViewModel.
            // Die Komponente kann nach dem Aufruf von PickAsync selbst StateHasChanged() rufen.
        }
    }
}