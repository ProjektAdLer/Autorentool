namespace PersistEntities.LearningOutcome;

public class StructuredLearningOutcomePe : ILearningOutcomePe
{
    public StructuredLearningOutcomePe(string what, string whereby, string whatFor, string verbOfVisibility,
        string language)
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
    private string Language { get; }
}