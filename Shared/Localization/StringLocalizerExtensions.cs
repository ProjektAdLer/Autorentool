using Microsoft.Extensions.Localization;

namespace Shared.Localization;

public static class StringLocalizerExtensions
{
    /// <summary>
    /// Provides <see cref="IStringLocalizer"/> for generic type <typeparamref name="T"/> as the default implementation
    /// of localization is very picky about generics.
    /// </summary>
    /// <param name="stringLocalizerFactory">The <see cref="IStringLocalizerFactory"/> injected by the DI container.</param>
    /// <typeparam name="T">The (generic) type for which the localizer should be created</typeparam>
    /// <returns>A localizer for the generic base type of <typeparamref name="T"/>,
    /// e.g. <c>IStringLocalizer&lt;Foobar&gt;</c> for <typeparamref name="T"/> equal to <c>Foobar&lt;string&gt;</c></returns>
    public static IStringLocalizer GetGenericLocalizer<T>(this IStringLocalizerFactory stringLocalizerFactory)
    {
        //get type of T
        var type = typeof(T);
        //get assembly name of containing assembly
        var assemblyName = type.Assembly.GetName().Name;
        //get type name of T without generic arguments
        var typeName = type.Name.Remove(type.Name.IndexOf('`'));
        //get 'Namespace.Class' without generic arguments
        var baseName = (type.Namespace + "." + typeName)[assemblyName!.Length..].Trim('.');

        return stringLocalizerFactory.Create(baseName, assemblyName);
    }
}