using System.Runtime.Serialization;

namespace DataAccess.Persistence;

public class DataContractSerializerWrapper<T> : IDataContractSerializer<T> where T : class, IExtensibleDataObject
{
    private readonly DataContractSerializer _serializer;

    public DataContractSerializerWrapper()
    {
        _serializer = new DataContractSerializer(typeof(T));
    }
    public void WriteObject(Stream writer, T graph)
    {
        _serializer.WriteObject(writer, graph);
    }

    public T ReadObject(Stream reader)
    {
        return _serializer.ReadObject(reader) as T ??
               throw new SerializationException($"could not cast object to {typeof(T).Name}");
    }
}