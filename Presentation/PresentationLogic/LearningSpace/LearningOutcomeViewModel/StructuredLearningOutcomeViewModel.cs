using System.Text;

namespace Presentation.PresentationLogic.LearningSpace.LearningOutcomeViewModel;

public class StructuredLearningOutcomeViewModel : ILearningOutcomeViewModel
{
    public StructuredLearningOutcomeViewModel(string what, string whereby, string whatFor, string verbOfVisibility,
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

    public string GetOutcome()
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