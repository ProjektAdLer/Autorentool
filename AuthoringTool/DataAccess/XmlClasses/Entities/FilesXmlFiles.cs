﻿using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses;


[XmlRoot(ElementName = "files")]
public partial class FilesXmlFiles : IXmlSerializable
{
    public void SetParameters()
    {
        
    }

    public void Serialize()
    {
        var xml = new XmlSer();
        xml.Serialize(this, "files.xml");
    }

}
