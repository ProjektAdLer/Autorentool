namespace AuthoringTool.PresentationLogic.LearningWorld;

public interface ILearningWorldPresenter
{
    LearningWorldViewModel CreateNewLearningWorld(string name, string shortname, string authors,
        string language, string description, string goals);

    LearningWorldViewModel EditLearningWorld(LearningWorldViewModel world, string name, string shortname,
        string authors, string language, string description, string goals);
}