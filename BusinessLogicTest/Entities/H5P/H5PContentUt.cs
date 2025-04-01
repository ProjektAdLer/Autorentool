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
}