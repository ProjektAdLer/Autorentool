﻿using BusinessLogic.Entities.FloorPlans;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Entities;

[TestFixture]
public class FloorPlansUt
{
    [Test]
    [TestCase(FloorPlanEnum.R_20X20_6L, typeof(R20X206L), 6)]
    [TestCase(FloorPlanEnum.R_20X30_8L, typeof(R20X308L), 8)]
    [TestCase(FloorPlanEnum.L_32X31_10L, typeof(L32X3110L), 10)]
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