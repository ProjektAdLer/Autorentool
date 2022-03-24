using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using AuthoringTool.Entities;

namespace AuthoringTool.DataAccess.Persistence;

internal class FileSaveHandler<T> : IFileSaveHandler<T> where T : class //new() constraint for default ctor
{
    private readonly ILogger<FileSaveHandler<T>> _logger;
    private readonly XmlSerializer _serializer;

    public FileSaveHandler(ILogger<FileSaveHandler<T>> logger)
    {
        _logger = logger;
        //Check if type T is serializable in the first place
        var typeT = typeof(T);
        if (!typeT.IsSerializable && !typeof(ISerializable).IsAssignableFrom(typeT))
        {
            throw new InvalidOperationException($"Type {typeT.Name} is not serializable.");
        }
        
        //Check that type T has a parameterless constructor (otherwise deserialization will fail)
        if (typeT.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, Type.EmptyTypes) == null)
        {
            throw new InvalidOperationException($"Type {typeT.Name} has no required parameterless constructor.");
        }
        
        //Create actual serializer once for performance
        _serializer = new XmlSerializer(typeof(T));
    }

    public void SaveToDisk(T obj, string filepath)
    {
        using var fileStream = File.Open(filepath, FileMode.Create);
        SaveToStream(obj, fileStream);
    }

    public void SaveToStream(T obj, Stream stream)
    {
        _serializer.Serialize(stream, obj);
        stream.Flush();
    }

    public T LoadFromDisk(string filepath)
    {
        try
        {
            using var fileStream = File.OpenRead(filepath);
            return LoadFromStream(fileStream);
        }
        catch (Exception e)
        {
            _logger.LogError($"Couldn't deserialize file at {filepath} into {typeof(T).Name} object.");
            throw new SerializationException($"Couldn't deserialize file at {filepath} into {typeof(T).Name} object.", e);
        }
    }

    public T LoadFromStream(Stream stream)
    {
        return _serializer.Deserialize(stream) as T ?? throw new SerializationException($"Cast result to type {typeof(T).Name} null.");
    }
}