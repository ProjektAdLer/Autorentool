namespace BusinessLogic.Entities.LearningContent.AdaptivityContent.Question;

public class Choice
{
    public Choice(string text)
    {
        Text = text;
    }

    private Choice()
    {
        Text = "";
    }

    public string Text { get; set; }
}