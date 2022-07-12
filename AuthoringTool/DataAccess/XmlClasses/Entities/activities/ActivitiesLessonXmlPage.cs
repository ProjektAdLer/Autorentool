using System.Xml.Serialization;

namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

[XmlRoot(ElementName="page")]
public class ActivitiesLessonXmlPage : IActivitiesLessonXmlPage{
        
        public void SetParameters(string? prevpageid, string? nextpageid, string? qtype, string? qoption, 
                string? layout, string? display, string? timecreated, string? timemodified, string? title, 
                string? contents, string? contentsformat, ActivitiesLessonXmlAnswers? answers, string? branches, string? id)
        {
                Prevpageid = prevpageid;
                Nextpageid = nextpageid;
                Qtype = qtype;
                Qoption = qoption;
                Layout = layout;
                Display = display;
                Timecreated = timecreated;
                Timemodified = timemodified;
                Title = title;
                Contents = contents;
                Contentsformat = contentsformat;
                Answers = answers;
                Branches = branches;
                Id = id;
        }

        [XmlElement(ElementName="prevpageid")]
        public string? Prevpageid { get; set; }
        
        [XmlElement(ElementName="nextpageid")]
        public string? Nextpageid { get; set; }
        
        [XmlElement(ElementName="qtype")]
        public string? Qtype { get; set; }
        
        [XmlElement(ElementName="qoption")]
        public string? Qoption { get; set; }
        
        [XmlElement(ElementName="layout")]
        public string? Layout { get; set; }
        
        [XmlElement(ElementName="display")]
        public string? Display { get; set; }
        
        [XmlElement(ElementName="timecreated")]
        public string? Timecreated { get; set; }
        
        [XmlElement(ElementName="timemodified")]
        public string? Timemodified { get; set; }
        
        [XmlElement(ElementName="title")]
        public string? Title { get; set; }
        
        [XmlElement(ElementName="contents")]
        public string? Contents { get; set; }
        
        [XmlElement(ElementName="contentsformat")]
        public string? Contentsformat { get; set; }
        
        [XmlElement(ElementName="answers")]
        public ActivitiesLessonXmlAnswers? Answers { get; set; }
        
        [XmlElement(ElementName="branches")]
        public string? Branches { get; set; }
        
        [XmlAttribute(AttributeName="id")]
        public string? Id { get; set; }
}
