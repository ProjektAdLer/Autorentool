using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using JetBrains.Annotations;
using Shared.LearningOutcomes;

namespace PersistEntities.LearningOutcome;

[Serializable]
[DataContract]
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

    [DataMember] public TaxonomyLevel TaxonomyLevel { get; set; }
    [DataMember] public string What { get; set; }
    [DataMember] public string Whereby { get; set; }
    [DataMember] public string WhatFor { get; set; }
    [DataMember] public string VerbOfVisibility { get; set; }

    [DataMember]
    public string CultureCode
    {
        get => Language.Name;
        set => Language = new CultureInfo(value);
    }

    [IgnoreDataMember] public CultureInfo Language { get; set; }

    [IgnoreDataMember] public Guid Id { get; set; }

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


    [OnDeserializing]
    private void OnDeserializing(StreamingContext context)
    {
        Id = Guid.NewGuid();
    }
}