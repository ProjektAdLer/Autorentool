using System.Globalization;
using System.Text;
using JetBrains.Annotations;
using Shared.LearningOutcomes;

namespace PersistEntities.LearningOutcome;

public class StructuredLearningOutcomePe : ILearningOutcomePe
{
    [UsedImplicitly]
    private StructuredLearningOutcomePe()
    {
        TaxonomyLevel = TaxonomyLevel.None;
        What = string.Empty;
        Whereby = string.Empty;
        WhatFor = string.Empty;
        VerbOfVisibility = string.Empty;
        Language = CultureInfo.InvariantCulture;
        Id = Guid.Empty;
    }

    public StructuredLearningOutcomePe(TaxonomyLevel taxonomyLevel, string what, string whereby, string whatFor,
        string verbOfVisibility,
        CultureInfo language)
    {
        TaxonomyLevel = taxonomyLevel;
        What = what;
        Whereby = whereby;
        WhatFor = whatFor;
        VerbOfVisibility = verbOfVisibility;
        Language = language;
    }

    public TaxonomyLevel TaxonomyLevel { get; set; }
    public string What { get; set; }
    public string Whereby { get; set; }
    public string WhatFor { get; set; }
    public string VerbOfVisibility { get; set; }
    public CultureInfo Language { get; set; }

    public Guid Id { get; set; }

    public string GetOutcome()
    {
        switch (Language)
        {
            case { Name: "de-DE" }:
                return GetOutcomeDe();
            case { Name: "en-DE" }:
                return GetOutcomeEn();
            default:
                return "Failure";
        }
    }

    private string GetOutcomeEn()
    {
        var sb = new StringBuilder("The students will be able to ");
        sb.Append(VerbOfVisibility).Append(", ").Append(What);

        if (!IsNullOrEmpty(Whereby))
        {
            sb.Append(", \n by ").Append(Whereby);
        }

        if (!IsNullOrEmpty(WhatFor))
        {
            sb.Append(",\n to ").Append(WhatFor);
        }

        sb.Append(".");

        return sb.ToString();
    }

    private string GetOutcomeDe()
    {
        var sb = new StringBuilder("Die Studierenden können ");
        sb.Append(What).Append(" ").Append(VerbOfVisibility);

        if (!IsNullOrEmpty(Whereby))
        {
            sb.Append(", \n indem sie ").Append(Whereby);
        }

        if (!IsNullOrEmpty(WhatFor))
        {
            sb.Append(",\n um ").Append(WhatFor);
        }

        sb.Append(".");

        return sb.ToString();
    }

    private static bool IsNullOrEmpty(string value) =>
        string.IsNullOrWhiteSpace(value);
}