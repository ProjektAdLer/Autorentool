namespace Presentation.Components.Forms.Models;

public class LinkContentFormModel
{
    /// <summary>
    /// Parameterless constructor required for <see cref="BaseForm{TForm,TEntity}"/> TForm constraint.
    /// </summary>
    public LinkContentFormModel()
    {
        Name = "";
        Link = "";
    }

    public string Name { get; set; }
    public string Link { get; set; }
}