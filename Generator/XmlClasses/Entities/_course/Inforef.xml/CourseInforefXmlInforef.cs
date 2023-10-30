using System.Xml.Serialization;
using Generator.WorldExport;

namespace Generator.XmlClasses.Entities._course.Inforef.xml;

[XmlRoot(ElementName = "inforef")]
public class CourseInforefXmlInforef : ICourseInforefXmlInforef
{
    public CourseInforefXmlInforef()
    {
        Roleref = new CourseInforefXmlRoleref();
        QuestionCategoryref = new CourseInforefXmlQuestionCategoryref();
    }

    [XmlElement(ElementName = "roleref")] public CourseInforefXmlRoleref Roleref { get; set; }

    [XmlElement(ElementName = "question_categoryref")]
    public CourseInforefXmlQuestionCategoryref QuestionCategoryref { get; set; }

    public void Serialize()
    {
        var xml = new XmlSerialize();
        xml.Serialize(this, Path.Join("course", "inforef.xml"));
    }
}

public class CourseInforefXmlQuestionCategoryref
{
    public CourseInforefXmlQuestionCategoryref()
    {
        QuestionCategories = new List<CourseInforefXmlQuestionCategoryrefQuestionCategory>()
        {
            new CourseInforefXmlQuestionCategoryrefQuestionCategory(3),
            new CourseInforefXmlQuestionCategoryrefQuestionCategory(4)
        };
    }


    [XmlElement("question_category")]
    public List<CourseInforefXmlQuestionCategoryrefQuestionCategory> QuestionCategories { get; set; }
}

public class CourseInforefXmlQuestionCategoryrefQuestionCategory
{
    public CourseInforefXmlQuestionCategoryrefQuestionCategory()
    {
        Id = 0;
    }

    public CourseInforefXmlQuestionCategoryrefQuestionCategory(int id)
    {
        Id = id;
    }

    [XmlElement("id")] public int Id { get; set; }
}