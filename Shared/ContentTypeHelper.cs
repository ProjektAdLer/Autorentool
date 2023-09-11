namespace Shared;

public static class ContentTypeHelper
{
    public static ContentTypeEnum GetContentType(string type)
    {
        return type switch
        {
            "h5p" => ContentTypeEnum.H5P,
            "txt" or "c" or "h" or "cpp" or "cc" or "c++" or "py" or "cs" or "js" or "php" or "html" or "css" =>
                ContentTypeEnum.Text,
            "jpg" or "png" or "webp" or "bmp" => ContentTypeEnum.Image,
            "pdf" => ContentTypeEnum.Text,
            "video" => ContentTypeEnum.Video,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type,
                $"Type {type} is not in the list of supported types.")
        };
    }
}