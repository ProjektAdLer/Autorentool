using System.Xml;
using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.WorldExport;

public class XmlSerialize
{
    public void Serialize(object xml, string xmlname)
    {
        var settings = new XmlWriterSettings
        {
            Encoding = new UpperCaseUTF8Encoding(), // Moodle needs Encoding in Uppercase!
            NewLineHandling = System.Xml.NewLineHandling.Replace,
            NewLineOnAttributes = true,
            Indent = true // Generate new lines for each element
        };
        
        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        ns.Add("", "");
        XmlSerializer x = new XmlSerializer(xml.GetType());
        using (var xmlWriter = XmlWriter.Create("XMLFilesForExport/"+xmlname, settings))
        {
            x.Serialize(xmlWriter, xml, ns);
        }
    }
    
}