using System.Runtime.Serialization;

namespace PersistEntities.LearningContent;

[KnownType(typeof(AdaptivityTaskPe))]
public class AdaptivityContentPe : IAdaptivityContentPe
{
    public AdaptivityContentPe(string name, ICollection<IAdaptivityTaskPe> tasks)
    {
        Tasks = tasks;
        Name = name;
    }

    /// <summary>
    /// Automapper constructor. DO NOT USE.
    /// </summary>
    private AdaptivityContentPe()
    {
        Tasks = null!;
        Name = "";
    }

    [DataMember] public ICollection<IAdaptivityTaskPe> Tasks { get; set; }
    [DataMember] public string Name { get; set; }
}