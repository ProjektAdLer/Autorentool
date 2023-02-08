namespace BusinessLogic.Validation.Validators.CustomValidators;

public static class UniqueNameHelper
{
    public static bool IsUnique(IEnumerable<(Guid, string)> allNames, string name, Guid id)
    {
        var allNamesExceptSelf = allNames
            .Where(tup => tup.Item1 != id)
            .Select(tup => tup.Item2);
        return !allNamesExceptSelf.Contains(name);
    }
}