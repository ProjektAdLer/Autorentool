﻿using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.Lesson.xml;

[XmlRoot(ElementName="pages")]
public class ActivitiesLessonXmlPages : IActivitiesLessonXmlPages{

    public ActivitiesLessonXmlPages()
    {
        Page = new ActivitiesLessonXmlPage();
    }


    [XmlElement(ElementName="page")]
    public ActivitiesLessonXmlPage Page { get; set; }
    
}
    