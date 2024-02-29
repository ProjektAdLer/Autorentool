using BusinessLogic.Entities.FloorPlans;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Entities;

[TestFixture]
public class FloorPlansUt
{
    [Test]
    public void FloorPlanProvider_GetFloorPlan_ReturnsCorrectFloorPlanWithCorrectCapacity(
        [Values] FloorPlanEnum floorPlanName)
    {
        var expectedEntityName = floorPlanName.ToString().Replace("_", "");
        var expectedCapacity = int.Parse(floorPlanName.ToString().Split("_").Last().Replace("L", ""));

        // Act
        var entity = FloorPlanProvider.GetFloorPlan(floorPlanName);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(entity.GetType().Name, Is.EqualTo(expectedEntityName));
            Assert.That(entity.Capacity, Is.EqualTo(expectedCapacity));
        });
    }

    [Test]
    public void FloorPlanProvider_GetFloorPlan_ThrowsExceptionWhenFloorPlanIsNotSupported()
    {
        Assert.That(() => FloorPlanProvider.GetFloorPlan((FloorPlanEnum)9999),
            Throws.TypeOf<ArgumentOutOfRangeException>());
    }
}