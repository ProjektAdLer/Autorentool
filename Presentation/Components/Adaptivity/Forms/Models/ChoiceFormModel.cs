namespace Presentation.Components.Adaptivity.Forms.Models;

public class ChoiceFormModel
{
    public ChoiceFormModel(string text)
    {
        Text = text;
        Id = Guid.NewGuid();
    }

    // ReSharper disable once UnusedMember.Local
    private ChoiceFormModel()
    {
        Text = "";
        Id = Guid.Empty;
    }

    public Guid Id { get; set; }
    public string Text { get; set; }
}