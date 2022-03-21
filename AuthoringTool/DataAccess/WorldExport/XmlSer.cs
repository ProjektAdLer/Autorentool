using System.Xml;
using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.WorldExport;

public class XmlSer
{
    public void serialize(object xml, string xmlname)
    {
        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        ns.Add("", "");
        XmlSerializer x = new XmlSerializer(xml.GetType());
        StreamWriter Writer = new StreamWriter("XMLFilesForExport/"+xmlname);
        using (var xmlWriter = XmlTextWriter.Create(Writer))
        {
            x.Serialize(xmlWriter, xml, ns);
        }
        Writer.Close();
    }
    
}