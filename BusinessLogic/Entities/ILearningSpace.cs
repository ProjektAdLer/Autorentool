namespace BusinessLogic.Entities;

public interface ILearningSpace : ISpace
{
    string Shortname { get; set; }
    string Authors { get; set; }
    string Goals { get; set; }
    List<LearningElement> LearningElements { get; set; }
}