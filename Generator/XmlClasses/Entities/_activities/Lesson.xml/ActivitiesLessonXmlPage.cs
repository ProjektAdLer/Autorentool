using System.Xml.Serialization;

namespace Generator.XmlClasses.Entities._activities.Lesson.xml;

[XmlRoot(ElementName="page")]
public class ActivitiesLessonXmlPage : IActivitiesLessonXmlPage{

        public ActivitiesLessonXmlPage()
        {
                PrevPageId = "0";
                NextPageId = "0";
                Qtype = "20";
                Qoption = "0";
                Layout = "1";
                Display = "1";
                Timecreated = "";
                Timemodified = "";
                Title = "";
                Contents = "";
                ContentsFormat = "1";
                Answers = new ActivitiesLessonXmlAnswers();
                Branches = "";
                Id = "";
        }
        
        
        [XmlElement(ElementName="prevpageid")]
        public string PrevPageId { get; set; }
        
        [XmlElement(ElementName="nextpageid")]
        public string NextPageId { get; set; }
        
        [XmlElement(ElementName="qtype")]
        public string Qtype { get; set; }
        
        [XmlElement(ElementName="qoption")]
        public string Qoption { get; set; }
        
        [XmlElement(ElementName="layout")]
        public string Layout { get; set; }
        
        [XmlElement(ElementName="display")]
        public string Display { get; set; }
        
        [XmlElement(ElementName="timecreated")]
        public string Timecreated { get; set; }
        
        [XmlElement(ElementName="timemodified")]
        public string Timemodified { get; set; }
        
        [XmlElement(ElementName="title")]
        public string Title { get; set; }
        
        //Content of the current Lesson-Page (Most of the Time its only 1 learning Element)
        [XmlElement(ElementName="contents")]
        public string Contents { get; set; }
        
        [XmlElement(ElementName="contentsformat")]
        public string ContentsFormat { get; set; }
        
        [XmlElement(ElementName="answers")]
        public ActivitiesLessonXmlAnswers Answers { get; set; }
        
        [XmlElement(ElementName="branches")]
        public string Branches { get; set; }
        
        [XmlAttribute(AttributeName="id")]
        public string Id { get; set; }
}
