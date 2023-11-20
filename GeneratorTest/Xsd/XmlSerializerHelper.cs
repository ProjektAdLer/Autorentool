using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace GeneratorTest.Xsd;

public class XmlSerializerHelper
{
    public static string SerializeObjectToXmlString<T>(T obj) where T : class
    {
        var serializer = new XmlSerializer(typeof(T));
        using var writer = new StringWriter();
        serializer.Serialize(writer, obj);
        return writer.ToString();
    }

    public static bool ValidateXmlAgainstXsd(string xmlContent, string xsdContent)
    {
        try
        {
            var schemas = new XmlSchemaSet();
            using var xsdReader = new StringReader(xsdContent);
            var schema = XmlSchema.Read(xsdReader, null!);
            if (schema != null) schemas.Add(schema);

            var settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema,
                Schemas = schemas
            };

            using var xmlReader = XmlReader.Create(new StringReader(xmlContent), settings);
            while (xmlReader.Read())
            {
            }

            return true;
        }
        catch (XmlSchemaValidationException)
        {
            return false;
        }
    }
}