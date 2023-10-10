namespace BusinessLogic.Entities.LearningContent.Adaptivity.Question;

public class Choice : IEquatable<Choice>
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

    public bool Equals(Choice? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Text == other.Text && Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Choice) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Text, Id);
    }

    public static bool operator ==(Choice? left, Choice? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Choice? left, Choice? right)
    {
        return !Equals(left, right);
    }
}