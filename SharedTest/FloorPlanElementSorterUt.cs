using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Shared;

namespace SharedTest;

[TestFixture]
public class FloorPlanElementSorterUt
{
    [Test]
    public void GetElementsInOrder_InvalidFloorPlanEnum_ThrowsException()
    {
        var invalidFloorPlan = (FloorPlanEnum) 999999;
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            FloorPlanElementSorter.GetElementsInOrder(new Dictionary<int, object>(), invalidFloorPlan));
    }

    [Test]
    public void GetElementsInOrder_ForEachFloorPlanEnum_ContainsEachElementOnce([Values] FloorPlanEnum floorPlan)
    {
        var expectedCount = ExtractCountFromFloorPlanName(floorPlan);
        // FloorPlanElementSorter takes Dictionary<int, T> with any type, so it is tested only with string type here.
        var elements = Enumerable.Range(0, expectedCount).ToDictionary(i => i, i => i.ToString());
        Assert.That(elements.Values, Is.Unique);

        var orderedElements = FloorPlanElementSorter.GetElementsInOrder(elements, floorPlan).ToList();

        TestListContent(elements.Values.ToList(), orderedElements, floorPlan);
    }


    [Test]
    public void GetListInOrder_InvalidFloorPlanEnum_ThrowsException()
    {
        var invalidFloorPlan = (FloorPlanEnum) 999999;
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            FloorPlanElementSorter.GetListInOrder(new List<int?>(), invalidFloorPlan));
    }

    [Test]
    public void GetListInOrder_ForEachFloorPlanEnum_ContainsEachElementOnce([Values] FloorPlanEnum floorPlan)
    {
        var expectedCount = ExtractCountFromFloorPlanName(floorPlan);
        var elements = Enumerable.Range(0, expectedCount).Select(i => (int?) i).ToList();
        Assert.That(elements, Is.Unique);

        var orderedElements = FloorPlanElementSorter.GetListInOrder(elements, floorPlan).ToList();

        TestListContent(elements, orderedElements, floorPlan);
    }

    private static void TestListContent<T>(List<T> unorderedElements, List<T> orderedElements, FloorPlanEnum floorPlan)
    {
        var expectedCount = ExtractCountFromFloorPlanName(floorPlan);
        Assert.That(orderedElements, Has.Count.EqualTo(expectedCount));
        Assert.That(orderedElements, Is.Unique);
        foreach (var element in unorderedElements)
        {
            Assert.That(orderedElements, Contains.Item(element));
        }

        var configureField = typeof(FloorPlanElementSorter).GetField($"ConfigureOrderFor_{floorPlan}",
            BindingFlags.NonPublic | BindingFlags.Static);
        Assert.That(configureField, Is.Not.Null);
        var configureOrderFor = configureField!.GetValue(typeof(FloorPlanElementSorter)) as List<int>;
        for (var i = 0; i < expectedCount; i++)
        {
            Assert.That(orderedElements[i]?.ToString(), Is.EqualTo(configureOrderFor?[i].ToString()));
        }
    }

    private static int ExtractCountFromFloorPlanName(FloorPlanEnum floorPlan)
    {
        var floorPlanName = floorPlan.ToString();
        const string pattern = "_([0-9]+)L";
        var match = Regex.Match(floorPlanName, pattern);
        var countString = match.Groups[1].Value;
        return int.Parse(countString);
    }
}