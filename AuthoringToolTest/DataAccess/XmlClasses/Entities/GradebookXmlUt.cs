using AuthoringTool.DataAccess.XmlClasses.Entities;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.XmlClasses.Entities;

[TestFixture]
public class GradebookXmlUt
{


    [Test]
    public void GradebookXmlGradebook_SetParameters_ObjectsAreEqual()
    {
        //Arrange 
        var gradeitem = new GradebookXmlGradeItem();
        var gradeitems = new GradebookXmlGradeItems();
        var gradecategory = new GradebookXmlGradeCategory();
        var gradecategories = new GradebookXmlGradeCategories();
        var gradeSetting = new GradebookXmlGradeSetting();
        gradeSetting.SetParameters("minmaxtouse", "1", "");

        var gradeSettings = new GradebookXmlGradeSettings();
        gradeSettings.SetParameters(gradeSetting);

        var gradebook = new GradebookXmlGradebook();

        gradeitem.SetParameters("$@NULL@$", "$@NULL@$", "course",
            "$@NULL@$", "1", "$@NULL@$", "$@NULL@$",
            "$@NULL@$", "$@NULL@$", "1", "100.00000", "0.00000",
            "$@NULL@$", "$@NULL@$", "0.00000", "1.00000",
            "0.00000", "0.00000", "0.00000", "0",
            "1", "0", "$@NULL@$", "0", "0", "0",
            "0", "currentTime", "currentTime", "", "1");
        gradeitems.SetParameters(gradeitem as GradebookXmlGradeItem);
        gradecategory.SetParameters("$@NULL@$", "1", "/1/", "?", "13",
            "0", "0", "1", "0", "currentTime", "currentTime",
            "0", "1");
        gradecategories.SetParameters(gradecategory as GradebookXmlGradeCategory);
        gradeSetting.SetParameters("minmaxtouse", "1", "");
        gradeSettings.SetParameters(gradeSetting as GradebookXmlGradeSetting);
        gradebook.SetParameters("", gradecategories as GradebookXmlGradeCategories,
            gradeitems as GradebookXmlGradeItems, "", gradeSettings as GradebookXmlGradeSettings);

        gradebook.Serialize();

        //Assert
        Assert.That(gradebook.Grade_items, Is.EqualTo(gradeitems));
        Assert.That(gradebook.Grade_categories, Is.EqualTo(gradecategories));
        Assert.That(gradebook.Grade_settings, Is.EqualTo(gradeSettings));
    }
}