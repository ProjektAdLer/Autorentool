using System.Globalization;

namespace BusinessLogic.Entities.LearningOutcome;

public class StructuredLearningOutcome : ILearningOutcome
{
    public StructuredLearningOutcome(string what, string whereby, string whatFor, string verbOfVisibility,
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
}