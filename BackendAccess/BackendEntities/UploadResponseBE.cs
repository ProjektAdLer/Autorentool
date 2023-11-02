using JetBrains.Annotations;

namespace BackendAccess.BackendEntities;

[UsedImplicitly]
public class UploadResponseBE
{
    public string WorldNameInLms { get; set; } = null!;
    public string WorldLmsUrl { get; set; } = null!;
    public string World3DUrl { get; set; } = null!;
}