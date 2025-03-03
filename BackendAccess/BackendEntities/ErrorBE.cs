using System.Diagnostics.CodeAnalysis;

namespace BackendAccess.BackendEntities;
// ReSharper disable once InconsistentNaming
[ExcludeFromCodeCoverage]
public class ErrorBE
{
    // ReSharper disable once UnassignedGetOnlyAutoProperty
    public string? Detail { set; get; }
    
    public string? Type { set; get; }
    
    public string? StatusCode { set; get; }
}