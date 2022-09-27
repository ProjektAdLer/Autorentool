using System.Runtime.Serialization;

namespace DataAccess.Persistence;

public interface IDataContractSerializer<T> where T : IExtensibleDataObject
{
    /// <inheritdoc cref="DataContractSerializer.WriteObject(System.Xml.XmlDictionaryWriter,object?)"/>
    public void WriteObject(Stream writer, T graph);
    /// <inheritdoc cref="DataContractSerializer.ReadObject(System.Xml.XmlDictionaryReader)"/>
    public T ReadObject(Stream reader);
}