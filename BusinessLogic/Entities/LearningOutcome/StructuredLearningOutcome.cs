using System.Globalization;
using JetBrains.Annotations;
using Shared.LearningOutcomes;

namespace BusinessLogic.Entities.LearningOutcome;

public class StructuredLearningOutcome : ILearningOutcome
{
    [UsedImplicitly]
    private StructuredLearningOutcome()
    {
        TaxonomyLevel = TaxonomyLevel.None;
        What = string.Empty;
        Whereby = string.Empty;
        WhatFor = string.Empty;
        VerbOfVisibility = string.Empty;
        Language = CultureInfo.InvariantCulture;
        Id = Guid.Empty;
    }

    public StructuredLearningOutcome(TaxonomyLevel taxonomyLevel, string what, string whereby, string whatFor,
        string verbOfVisibility,
        CultureInfo language)
    {
        TaxonomyLevel = taxonomyLevel;
        What = what;
        Whereby = whereby;
        WhatFor = whatFor;
        VerbOfVisibility = verbOfVisibility;
        Language = language;
        Id = Guid.NewGuid();
    }

    public TaxonomyLevel TaxonomyLevel { get; set; }
    public string What { get; set; }
    public string Whereby { get; set; }
    public string WhatFor { get; set; }
    public string VerbOfVisibility { get; set; }
    public CultureInfo Language { get; set; }
    public Guid Id { get; set; }
}