﻿using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities._activities.Inforef.xml;

[XmlRoot(ElementName="grade_item")]
public class ActivitiesInforefXmlGradeItem : IActivitiesInforefXmlGradeItem{

    public ActivitiesInforefXmlGradeItem()
    {
        Id = "1";
    }
    
   
    [XmlElement(ElementName="id")]
    public string Id { get; set; }
}