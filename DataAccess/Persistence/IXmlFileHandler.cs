using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace DataAccess.Persistence;

/// <summary>
/// Wrapper around XmlSerializer for typesafe (de-)serialization.
/// </summary>
/// <typeparam name="T">The type which should be (de-)serialized.</typeparam>
public interface IXmlFileHandler<T> where T : class
{
    /// <summary>
    /// Serializes passed object of type T to disk. Existing files will be overwritten.
    /// </summary>
    /// <param name="obj">The object to be serialized.</param>
    /// <param name="filepath">The desired filepath of the file.</param>
    /// <exception cref="SerializationException">Thrown when serialization fails for various reasons, see inner exception for details.</exception>
    void SaveToDisk(T obj, string filepath);
    
    /// <summary>
    /// Serializes passed object of type T to the stream.
    /// </summary>
    /// <param name="obj">The object to be serialized.</param>
    /// <param name="stream">The desired stream to which the object should be written.</param>
    /// <exception cref="InvalidOperationException">Please see <see cref="XmlSerializer.Serialize(Stream,object)"/></exception>
    void SaveToStream(T obj, Stream stream);
    
    /// <summary>
    /// Deserializes passed file into object of type T.
    /// </summary>
    /// <param name="filepath">The filepath of the desired file.</param>
    /// <returns>The deserialized object of type <see cref="T"/></returns>
    /// <exception cref="SerializationException">Thrown when deserialization fails for various reasons, see inner exception for details.</exception>
    T LoadFromDisk(string filepath);
    
    /// <summary>
    /// Deserializes contents of passed stream into object of type T.
    /// </summary>
    /// <param name="stream">The stream which contents should be deserialized.</param>
    /// <returns>The deserialized object of type T.</returns>
    /// <exception cref="SerializationException">Thrown when deserialized object cannot be cast to type T for whatever reason.</exception>
    T LoadFromStream(Stream stream);
}