using JetBrains.Annotations;

namespace BackendAccess.BackendEntities;
// ReSharper disable once InconsistentNaming
[UsedImplicitly]
public class UploadResponseBE
{
    public string WorldNameInLms { get; set; } = null!;
    public string WorldLmsUrl { get; set; } = null!;
    public string World3DUrl { get; set; } = null!;
}