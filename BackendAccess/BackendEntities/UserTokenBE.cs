using System.Diagnostics.CodeAnalysis;

namespace BackendAccess.BackendEntities;

[ExcludeFromCodeCoverage]
public class UserTokenBE
{
    public string LmsToken { get; init; } = null!; //deserialization - n.stich
}