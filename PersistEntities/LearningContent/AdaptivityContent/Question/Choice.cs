namespace PersistEntities.LearningContent.Question;

public class Choice
{
    public Choice(string text)
    {
        Text = text;
        Id = Guid.NewGuid();
    }

    private Choice()
    {
        Text = "";
        Id = Guid.Empty;
    }

    public string Text { get; set; }
    public Guid Id { get; set; }
}