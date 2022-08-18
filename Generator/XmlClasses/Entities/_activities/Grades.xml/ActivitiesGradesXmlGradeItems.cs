﻿using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities._activities.Grades.xml;


[XmlRoot(ElementName="grade_items")]
public class ActivitiesGradesXmlGradeItems : IActivitiesGradesXmlGradeItems {
    
    public ActivitiesGradesXmlGradeItems()
    {
        GradeItem = new ActivitiesGradesXmlGradeItem();
    }

    
    [XmlElement(ElementName="grade_item")]
    public ActivitiesGradesXmlGradeItem GradeItem { get; set; }
    
}