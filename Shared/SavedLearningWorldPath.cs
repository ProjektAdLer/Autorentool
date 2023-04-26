using System.Xml.Serialization;

namespace Shared;

[Serializable]
public class SavedLearningWorldPath
{
    [XmlIgnore]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
}