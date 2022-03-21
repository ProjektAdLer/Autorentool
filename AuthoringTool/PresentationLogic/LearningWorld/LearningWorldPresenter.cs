namespace AuthoringTool.PresentationLogic.LearningWorld
{
    internal class LearningWorldPresenter : ILearningWorldPresenter
    {
        public LearningWorldViewModel CreateNewLearningWorld(string name, string shortname, string authors,
            string language, string description, string goals)
        {
            return new LearningWorldViewModel(name, shortname, authors, language, description, goals);
        }

        public LearningWorldViewModel EditLearningWorld(LearningWorldViewModel world, string name, string shortname,
            string authors, string language, string description, string goals)
        {
            world.Name = name;
            world.Shortname = shortname;
            world.Authors = authors;
            world.Language = language;
            world.Description = description;
            world.Goals = goals;
            return world;
        }
    }
}