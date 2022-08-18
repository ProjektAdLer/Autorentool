namespace Generator.PersistEntities;

public interface ILearningContentPe
{
    string Name { get; set; }
    string Type { get; set; }
    byte[] Content { get; set; }
}