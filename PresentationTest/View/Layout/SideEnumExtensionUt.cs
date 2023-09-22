using NUnit.Framework;
using Presentation.View.Layout;

namespace PresentationTest.View.Layout;

[TestFixture]
public class SideEnumExtensionUt
{
    [Test]
    [TestCase(Side.Left, ExpectedResult = "Left")]
    [TestCase(Side.Right, ExpectedResult = "Right")]
    public string AsString_ReturnsCorrectString(Side side) => side.AsString();
    
    [Test]
    [TestCase(Side.Left, ExpectedResult = "left-0")]
    [TestCase(Side.Right, ExpectedResult = "right-0")]
    public string AsCssClass_ReturnsCorrectString(Side side) => side.AsCssClass();
    
    [Test]
    [TestCase(Side.Left, ExpectedResult = Side.Right)]
    [TestCase(Side.Right, ExpectedResult = Side.Left)]
    public Side GetOpposite_ReturnsCorrectSide(Side side) => side.GetOpposite();
    
    [Test]
    [TestCase(Side.Left, ExpectedResult = "Right")]
    [TestCase(Side.Right, ExpectedResult = "Left")]
    public string OppositeAsString_ReturnsCorrectString(Side side) => side.OppositeAsString();
    
    [Test]
    [TestCase(Side.Left, ExpectedResult = "right-0")]
    [TestCase(Side.Right, ExpectedResult = "left-0")]
    public string OppositeAsCssClass_ReturnsCorrectString(Side side) => side.OppositeAsCssClass();
}