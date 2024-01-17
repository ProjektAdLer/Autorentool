using System.Globalization;

namespace PersistEntities.LearningOutcome;

public class StructuredLearningOutcomePe : ILearningOutcomePe
{
    public StructuredLearningOutcomePe(string what, string whereby, string whatFor, string verbOfVisibility,
        CultureInfo language)
    {
        What = what;
        Whereby = whereby;
        WhatFor = whatFor;
        VerbOfVisibility = verbOfVisibility;
        Language = language;
    }

    private string What { get; }
    private string Whereby { get; }
    private string WhatFor { get; }
    private string VerbOfVisibility { get; }
    private CultureInfo Language { get; }

    public string GetOutcome()
    {
        throw new NotImplementedException();
    }
}