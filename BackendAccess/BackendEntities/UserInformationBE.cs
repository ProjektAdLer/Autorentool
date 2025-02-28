using System.Diagnostics.CodeAnalysis;

namespace BackendAccess.BackendEntities;
// ReSharper disable once InconsistentNaming
[ExcludeFromCodeCoverage]
public class UserInformationBE
{
    public string LmsUserName { get; init; } = null!; //deserialization - n.stich
    public bool IsAdmin { get; init; }
    public int UserId { get; init; }
    public string UserEmail { get; init; } = null!; //deserialization - n.stich
}