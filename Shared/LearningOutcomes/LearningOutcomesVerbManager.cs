using System.Globalization;

namespace Shared.LearningOutcomes;

public class LearningOutcomesVerbManager
{
    private readonly Dictionary<CultureInfo, Dictionary<TaxonomyLevel, string>> _languageToTaxonomyLevel = new()
    {
        {
            new CultureInfo("de-DE"), new Dictionary<TaxonomyLevel, string>()
            {
                { TaxonomyLevel.None, "" },
                { TaxonomyLevel.Level1, "Taxo1deutsch" },
                { TaxonomyLevel.Level2, "Taxo2deutsch" },
                { TaxonomyLevel.Level3, "Taxo3deutsch" },
                { TaxonomyLevel.Level4, "Taxo4deutsch" },
                { TaxonomyLevel.Level5, "Taxo5deutsch" },
                { TaxonomyLevel.Level6, "Taxo6deutsch" },
            }
        },
        {
            new CultureInfo("en-DE"), new Dictionary<TaxonomyLevel, string>()
            {
                { TaxonomyLevel.None, "" },
                { TaxonomyLevel.Level1, "Taxo1englisch" },
                { TaxonomyLevel.Level2, "Taxo2englisch" },
                { TaxonomyLevel.Level3, "Taxo3englisch" },
                { TaxonomyLevel.Level4, "Taxo4englisch" },
                { TaxonomyLevel.Level5, "Taxo5englisch" },
                { TaxonomyLevel.Level6, "Taxo6englisch" },
            }
        }
    };

    private readonly Dictionary<CultureInfo, Dictionary<PredefinedVerbOfVisibility, string>>
        _languageToVerbOfVisibility = new()
        {
            {
                new CultureInfo("de-DE"), new Dictionary<PredefinedVerbOfVisibility, string>()
                {
                    { PredefinedVerbOfVisibility.Verb1, "Verb1deutsch" },
                    { PredefinedVerbOfVisibility.Verb2, "Verb2deutsch" },
                    { PredefinedVerbOfVisibility.Verb3, "Verb3deutsch" },
                    { PredefinedVerbOfVisibility.Verb4, "Verb4deutsch" },
                    { PredefinedVerbOfVisibility.Verb5, "Verb5deutsch" },
                    { PredefinedVerbOfVisibility.Verb6, "Verb6deutsch" }
                }
            },
            {
                new CultureInfo("en-DE"), new Dictionary<PredefinedVerbOfVisibility, string>()
                {
                    { PredefinedVerbOfVisibility.Verb1, "Verb1englisch" },
                    { PredefinedVerbOfVisibility.Verb2, "Verb2englisch" },
                    { PredefinedVerbOfVisibility.Verb3, "Verb3englisch" },
                    { PredefinedVerbOfVisibility.Verb4, "Verb4englisch" },
                    { PredefinedVerbOfVisibility.Verb5, "Verb5englisch" }
                }
            }
        };

    private readonly Dictionary<TaxonomyLevel, List<PredefinedVerbOfVisibility>> _taxonomyLevelToVerbsOfVisibilityDe =
        new()
        {
            {
                TaxonomyLevel.None,
                new List<PredefinedVerbOfVisibility>()
                {
                    PredefinedVerbOfVisibility.Verb1, PredefinedVerbOfVisibility.Verb2,
                    PredefinedVerbOfVisibility.Verb3, PredefinedVerbOfVisibility.Verb4,
                    PredefinedVerbOfVisibility.Verb5, PredefinedVerbOfVisibility.Verb6
                }
            },
            {
                TaxonomyLevel.Level1,
                new List<PredefinedVerbOfVisibility>()
                    { PredefinedVerbOfVisibility.Verb1, PredefinedVerbOfVisibility.Verb2 }
            },
            {
                TaxonomyLevel.Level2,
                new List<PredefinedVerbOfVisibility>()
                    { PredefinedVerbOfVisibility.Verb3, PredefinedVerbOfVisibility.Verb4 }
            },
            {
                TaxonomyLevel.Level3,
                new List<PredefinedVerbOfVisibility>()
                    { PredefinedVerbOfVisibility.Verb5, PredefinedVerbOfVisibility.Verb6 }
            },
        };

    private readonly Dictionary<TaxonomyLevel, List<PredefinedVerbOfVisibility>> _taxonomyLevelToVerbsOfVisibilityEn =
        new()
        {
            {
                TaxonomyLevel.None,
                new List<PredefinedVerbOfVisibility>()
                {
                    PredefinedVerbOfVisibility.Verb1, PredefinedVerbOfVisibility.Verb2,
                    PredefinedVerbOfVisibility.Verb3, PredefinedVerbOfVisibility.Verb4, PredefinedVerbOfVisibility.Verb5
                }
            },
            {
                TaxonomyLevel.Level1,
                new List<PredefinedVerbOfVisibility>()
                    { PredefinedVerbOfVisibility.Verb4, PredefinedVerbOfVisibility.Verb5 }
            },
            {
                TaxonomyLevel.Level2,
                new List<PredefinedVerbOfVisibility>()
                    { PredefinedVerbOfVisibility.Verb3, PredefinedVerbOfVisibility.Verb2 }
            },
            { TaxonomyLevel.Level3, new List<PredefinedVerbOfVisibility>() { PredefinedVerbOfVisibility.Verb1 } },
        };

    public List<string> GetVerbsOfVisibility(TaxonomyLevel taxonomyLevel, CultureInfo language)
    {
        List<PredefinedVerbOfVisibility> verbOfVisibilities = new();
        switch (language)
        {
            case { Name: "de-DE" }:
                verbOfVisibilities = _taxonomyLevelToVerbsOfVisibilityDe[taxonomyLevel];
                break;
            case { Name: "en-DE" }:
                verbOfVisibilities = _taxonomyLevelToVerbsOfVisibilityEn[taxonomyLevel];
                break;
        }

        return _languageToVerbOfVisibility[language].Where(x => verbOfVisibilities.Contains(x.Key)).Select(x => x.Value)
            .ToList();
    }

    public Dictionary<TaxonomyLevel, string> GetTaxonomyLevelNames(CultureInfo culture)
    {
        return _languageToTaxonomyLevel[culture];
    }
}