namespace AuthoringTool.DataAccess.XmlClasses.Entities.course;

public interface ICourseCourseXmlCourse : IXmlSerializable
{
    void SetParameters(string? shortname, string? fullname, string idnumber, string summary,
        string summaryformat, string format, string showgrades, string newsitems, string startdate, string enddate,
        string marker, string maxbytes, string legacyfiles, string showreports, string visible, string groupmode,
        string groupmodeforce, string defaultgroupingid, string lang, string theme, string timecreated,
        string timemodified, string requested, string showactivitydates, string showcompletionconditions,
        string enablecompletion, string completionnotify, string hiddensections, string coursedisplay,
        CourseCourseXmlCategory? category, string? tags, string customfields, string id, string contextid);
}