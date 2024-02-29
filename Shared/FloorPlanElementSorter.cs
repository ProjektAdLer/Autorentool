namespace Shared;

public static class FloorPlanElementSorter
{
    public static IEnumerable<T> GetElementsInOrder<T>(
        IDictionary<int, T> learningElements,
        FloorPlanEnum floorPlanName)
    {
        return floorPlanName switch
        {
            FloorPlanEnum.L_32X31_10L => ConfigureOrderFor_L_32X31_10L.Where(learningElements.ContainsKey)
                .Select(key => learningElements[key]),
            FloorPlanEnum.R_20X20_6L => ConfigureOrderFor_R_20X20_6L.Where(learningElements.ContainsKey)
                .Select(key => learningElements[key]),
            FloorPlanEnum.R_20X30_8L => ConfigureOrderFor_R_20X30_8L.Where(learningElements.ContainsKey)
                .Select(key => learningElements[key]),
            FloorPlanEnum.T_40X32_13L => ConfigureOrderFor_T_40X32_13L.Where(learningElements.ContainsKey)
                .Select(key => learningElements[key]),
            FloorPlanEnum.D_40X37_15L => ConfigureOrderFor_D_40X37_15L.Where(learningElements.ContainsKey)
                .Select(key => learningElements[key]),
            _ => throw new ArgumentOutOfRangeException(nameof(floorPlanName), floorPlanName, null)
        };
    }

    public static List<int?> GetListInOrder(List<int?> spaceSpaceSlotContents, FloorPlanEnum floorPlanName)
    {
        return floorPlanName switch
        {
            FloorPlanEnum.L_32X31_10L => ConfigureOrderFor_L_32X31_10L.Select(x => spaceSpaceSlotContents[x]).ToList(),
            FloorPlanEnum.R_20X20_6L => ConfigureOrderFor_R_20X20_6L.Select(x => spaceSpaceSlotContents[x]).ToList(),
            FloorPlanEnum.R_20X30_8L => ConfigureOrderFor_R_20X30_8L.Select(x => spaceSpaceSlotContents[x]).ToList(),
            FloorPlanEnum.T_40X32_13L => ConfigureOrderFor_T_40X32_13L.Select(x => spaceSpaceSlotContents[x]).ToList(),
            FloorPlanEnum.D_40X37_15L => ConfigureOrderFor_D_40X37_15L.Select(x => spaceSpaceSlotContents[x]).ToList(),
            _ => throw new ArgumentOutOfRangeException(nameof(floorPlanName), floorPlanName, null)
        };
    }

    // ReSharper disable InconsistentNaming
    private static readonly List<int> ConfigureOrderFor_L_32X31_10L = new() { 0, 1, 2, 3, 9, 8, 7, 4, 6, 5 };
    private static readonly List<int> ConfigureOrderFor_R_20X20_6L = new() { 0, 1, 2, 3, 5, 4 };
    private static readonly List<int> ConfigureOrderFor_R_20X30_8L = new() { 0, 1, 2, 3, 7, 6, 5, 4 };

    private static readonly List<int>
        ConfigureOrderFor_T_40X32_13L = new() { 0, 1, 2, 5, 3, 4, 6, 11, 12, 7, 10, 9, 8 };

    private static readonly List<int>
        ConfigureOrderFor_D_40X37_15L = new() { 0, 1, 2, 3, 14, 4, 5, 13, 12, 6, 11, 7, 10, 9, 8 };

    // ReSharper restore InconsistentNaming
}