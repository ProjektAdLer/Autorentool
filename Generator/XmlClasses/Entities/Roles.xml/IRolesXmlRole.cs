namespace Generator.XmlClasses.Entities.Roles.xml;

public interface IRolesXmlRole
{
    string Name { get; set; }
        
    string Shortname { get; set; }
        
    string NameInCourse { get; set; }

    string Description { get; set; }

    string Sortorder { get; set; }

    string Archetype { get; set; }

    string Id { get; set; }
}