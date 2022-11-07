namespace Presentation.View.Layout;

public enum Side
{
    Left, Right
}

public static class SideEnumExtension
{
    public static string AsString(this Side side) => side.ToString();

    public static string AsCssClass(this Side side)
    {
        return side switch
        {
            Side.Left => "left-0",
            Side.Right => "right-0",
            _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
        };
    }
    public static Side GetOpposite(this Side side) => 
        side switch
        {
            Side.Left => Side.Right,
            Side.Right => Side.Left,
            _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
        };

    public static string OppositeAsString(this Side side) => side.GetOpposite().AsString();

    public static string OppositeAsCssClass(this Side side) => side.GetOpposite().AsCssClass();
}