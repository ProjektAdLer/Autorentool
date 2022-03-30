namespace AuthoringTool.DataAccess.Persistence;

internal interface IFileSaveHandler<T> where T : class
{
    void SaveToDisk(T obj, string filepath);
    void SaveToStream(T obj, Stream stream);
    T LoadFromDisk(string filepath);
    T LoadFromStream(Stream stream);
}