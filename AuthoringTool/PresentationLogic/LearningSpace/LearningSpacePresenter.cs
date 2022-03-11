namespace AuthoringTool.PresentationLogic.LearningSpace
{
    internal class LearningSpacePresenter : ILearningSpacePresenter
    {
        public LearningSpaceViewModel CreateNewLearningSpace(string name, string shortname, string authors,
            string description, string goals)
        {
            return new LearningSpaceViewModel(name, shortname, authors, description, goals);
        }

        public LearningSpaceViewModel EditLearningSpace(LearningSpaceViewModel space, string name, string shortname,
            string authors, string description, string goals)
        {
            space.Name = name;
            space.Shortname = shortname;
            space.Authors = authors;
            space.Description = description;
            space.Goals = goals;
            return space;
        }
    }
}