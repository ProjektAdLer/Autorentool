namespace AuthoringTool.DataAccess.XmlClasses.course;

public interface ICourseEnrolmentsXmlEnrol
{
    void SetParametersShort(string roleid, string id, string enrolchild, string status);

    void SetParametersFull(string roleid, string id, string enrolchild, string status,
        string customint1, string customint2, string customint3, string customint4, string customint5,
        string customint6);
}