using System.Diagnostics.CodeAnalysis;

namespace BackendAccess.BackendEntities;

[ExcludeFromCodeCoverage]
public class ErrorBE
{
    // ReSharper disable once UnassignedGetOnlyAutoProperty
    public string? Detail { set; get; }
}