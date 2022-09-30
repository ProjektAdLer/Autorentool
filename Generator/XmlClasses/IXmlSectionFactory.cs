using System.IO.Abstractions;
using Generator.DSL;
using Generator.XmlClasses.Entities._sections.Inforef.xml;
using Generator.XmlClasses.Entities._sections.Section.xml;

namespace Generator.XmlClasses;

public interface IXmlSectionFactory
{

    void CreateSectionFactory();

}