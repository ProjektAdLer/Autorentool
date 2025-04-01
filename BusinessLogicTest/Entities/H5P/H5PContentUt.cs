using BusinessLogic.Entities.LearningContent.H5P;
using NUnit.Framework;

namespace BusinessLogicTest.Entities.H5P;

[TestFixture]
public class H5PContentUt
{
    [Test]
    public void H5PContent_ConstructorWithValidParameter_ConstructsH5PContentWithStateParameter(
        [Values] H5PContentStateEnum state)
    {
        var systemUnderTest = new H5PContent(state);
        Assert.That(systemUnderTest._state, Is.EqualTo(state));
    }
    [Test]
    public void H5PContent_ConstructorWithInvalidEnumValue_ThrowsArgumentOutOfRangeException()
    {
        var invalidState = (H5PContentStateEnum)999;

        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            _ = new H5PContent(invalidState);
        });
    }
}
    
