﻿using System.Xml;
using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.WorldExport;

public class XmlSer
{
    public void serialize(object xml, string xmlname)
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
        //StreamWriter Writer = new StreamWriter("XMLFilesForExport/"+xmlname);
        using (var xmlWriter = XmlTextWriter.Create("XMLFilesForExport/"+xmlname, settings))
        {
            x.Serialize(xmlWriter, xml, ns);
        }
        //Writer.Close();
    }
    
}