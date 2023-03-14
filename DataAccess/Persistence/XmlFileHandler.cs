using System.IO.Abstractions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;

namespace DataAccess.Persistence;

/// <inheritdoc cref="IXmlFileHandler{T}"/>
public class XmlFileHandler<T> : IXmlFileHandler<T> where T : class //new() constraint for default ctor
{
    private readonly ILogger<XmlFileHandler<T>> _logger;
    private readonly IFileSystem _fileSystem;
    private readonly DataContractSerializer _serializer;
    private readonly DataContractSerializerSettings _settings;

    /// <summary>
    /// Default constructor, no FileSystem mocking.
    /// </summary>
    /// <param name="logger">Logger for log output.</param>
    /// <exception cref="InvalidOperationException">Thrown when class T is either not serializable or doesn't have a
    /// parameterless constructor (necessary for XmlSerializer deserialization).</exception>
    public XmlFileHandler(ILogger<XmlFileHandler<T>> logger) : this(logger, new FileSystem()) { }

    /// <summary>
    /// Testable constructor with insertable IFileSystem for mocking.
    /// </summary>
    /// <param name="logger">Logger for log output.</param>
    /// <param name="fileSystem">File system to be used.</param>
    /// <exception cref="InvalidOperationException">Thrown when class T is either not serializable or doesn't have a
    /// parameterless constructor (necessary for XmlSerializer deserialization).</exception>
    public XmlFileHandler(ILogger<XmlFileHandler<T>> logger, IFileSystem fileSystem)
    {
        _logger = logger;
        _fileSystem = fileSystem;
        _settings = new DataContractSerializerSettings
        {
            PreserveObjectReferences = true
        };
        //Check if type T is serializable in the first place
        var typeT = typeof(T);
        if (typeT.IsGenericType && typeT.GetInterface("IEnumerable") != null)
        {
            typeT = typeT.GetGenericArguments()[0];
        }
        if (typeT.GetCustomAttributesData().All(ca => ca.AttributeType != typeof(DataContractAttribute)))
        {
            throw new InvalidOperationException($"Type {typeT.Name} is not serializable.");
        }
        
        //Create actual serializer once for performance
        _serializer = new DataContractSerializer(typeof(T), _settings);
    }

    /// <inheritdoc cref="IXmlFileHandler{T}.SaveToDisk"/>
    public void SaveToDisk(T obj, string filepath)
    {
        try
        {
            using var fileStream = _fileSystem.File.Open(filepath, FileMode.Create);
            SaveToStream(obj, fileStream);
            fileStream.Close();
        }
        catch (Exception e)
        {
            _logger.LogError("Couldn\'t serialize {Name} object into file at {Filepath}: {EMessage}", typeof(T).Name, filepath, e.Message);
            throw new SerializationException($"Couldn't serialize {typeof(T).Name} object into file at {filepath}.", e);
        }
    }

    /// <inheritdoc cref="IXmlFileHandler{T}.SaveToStream"/>
    public void SaveToStream(T obj, Stream stream)
    {
        _serializer.WriteObject(stream, obj);
        stream.Flush();
    }

    /// <inheritdoc cref="IXmlFileHandler{T}.LoadFromDisk"/>
    public T LoadFromDisk(string filepath)
    {
        Stream? fileStream = null;
        try
        {
            fileStream = _fileSystem.File.OpenRead(filepath);
            return LoadFromStream(fileStream);
        }
        catch (Exception e)
        {
            _logger.LogError("Couldn\'t deserialize file at {Filepath} into {Name} object: {EMessage}", filepath, typeof(T).Name, e.Message);
            throw new SerializationException($"Couldn't deserialize file at {filepath} into {typeof(T).Name} object.", e);
        }
        finally
        {
            fileStream?.Close();
        }
    }

    /// <inheritdoc cref="IXmlFileHandler{T}.LoadFromStream"/>
    public T LoadFromStream(Stream stream)
    {
        return _serializer.ReadObject(stream) as T ?? throw new SerializationException($"Cast result to type {typeof(T).Name} null.");
    }
}