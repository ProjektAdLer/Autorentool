namespace Presentation.Components.Forms.Models;

public class LinkContentFormModel : ILinkContentFormModel, IEquatable<LinkContentFormModel>
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

    public bool Equals(LinkContentFormModel? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name && Link == other.Link;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((LinkContentFormModel)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Link);
    }
}