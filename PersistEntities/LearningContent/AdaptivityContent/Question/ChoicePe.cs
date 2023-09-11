namespace PersistEntities.LearningContent.Question;

public class ChoicePe
{
    public ChoicePe(string text)
    {
        Text = text;
        Id = Guid.NewGuid();
    }

    private ChoicePe()
    {
        Text = "";
        Id = Guid.Empty;
    }

    public string Text { get; set; }
    public Guid Id { get; set; }
}