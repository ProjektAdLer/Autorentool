using BusinessLogic.Entities.FloorPlans;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Entities;

[TestFixture]
public class FloorPlansUt
{
    [Test]
    [TestCase(FloorPlanEnum.Rectangle2X2, typeof(Rectangle2X2), 4)]
    [TestCase(FloorPlanEnum.LShape3L2, typeof(LShape3L2), 5)]
    [TestCase(FloorPlanEnum.Rectangle2X3, typeof(Rectangle2X3), 6)]
    public void FloorPlanProvider_GetFloorPlan_ReturnsCorrectFloorPlanWithCorrectCapacity(FloorPlanEnum floorPlanName,
        Type floorPlanType, int floorPlanCapacity)
    {
        var floorPlanInstance = FloorPlanProvider.GetFloorPlan(floorPlanName);
        Assert.Multiple(() =>
        {
            Assert.That(floorPlanInstance, Is.InstanceOf(floorPlanType));
            Assert.That(floorPlanInstance.Capacity, Is.EqualTo(floorPlanCapacity));
        });
    }

    [Test]
    public void FloorPlanProvider_GetFloorPlan_ThrowsExceptionWhenFloorPlanIsNotSupported()
    {
        Assert.That(() => FloorPlanProvider.GetFloorPlan((FloorPlanEnum) 9999),
            Throws.TypeOf<ArgumentOutOfRangeException>());
    }
}