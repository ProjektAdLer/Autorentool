﻿using System.Xml.Serialization;
using AuthoringTool.DataAccess.WorldExport;

namespace AuthoringTool.DataAccess.XmlClasses;

[XmlRoot(ElementName="moodle_backup")]
public partial class MoodleBackupXmlMoodleBackup : IXmlSerializable {

 
    public void SetParameters(MoodleBackupXmlInformation moodleBackupInformation)
    {
        Information = moodleBackupInformation;
    }
    
    public void Serialize()
    {
        var xml = new XmlSer();
        xml.Serialize(this, "moodle_backup.xml"); 
    }
    
    [XmlElement(ElementName="information")]
    public MoodleBackupXmlInformation Information { get; set; }
}