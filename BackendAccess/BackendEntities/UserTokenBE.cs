using System.Diagnostics.CodeAnalysis;

namespace BackendAccess.BackendEntities;
// ReSharper disable once InconsistentNaming
[ExcludeFromCodeCoverage]
public class UserTokenBE
{
    public string LmsToken { get; init; } = null!; //deserialization - n.stich
}