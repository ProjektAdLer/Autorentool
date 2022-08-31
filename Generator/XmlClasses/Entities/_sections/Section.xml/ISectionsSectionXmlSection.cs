namespace Generator.XmlClasses.Entities._sections.Section.xml;

public interface ISectionsSectionXmlSection : IXmlSerializablePath
{
    string Number { get; set; }
        
    string Name { get; set; }

    string Summary { get; set; }

    string SummaryFormat { get; set; }
        
    string Sequence { get; set; }
        
    string Visible { get; set; }

    string AvailabilityJson { get; set; }

    string Timemodified { get; set; }
        
    string Id { get; set; }
}