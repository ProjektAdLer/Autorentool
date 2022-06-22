namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

public interface IActivitiesLessonXmlLesson
{
    void SetParameters(string? course, string? name, string? intro, string? introformat,
        string? practice, string? modattempts, string? usepassword, string? password, string? dependency,
        string? conditions, string? grade, string? custom, string? ongoing, string? usemaxgrade, string? maxanswers,
        string? maxattempts, string? review, string? nextpagedefault, string? feedback, string? minquestions,
        string? maxpages, string? timelimit, string? retake, string? activitylink, string? mediafile,
        string? mediaheight, string? mediawidth, string? mediaclose, string? slideshow, string? width,
        string? height, string? bgcolor, string? displayleft, string? displayleftif, string? progressbar,
        string? available, string? deadline, string? timemodified, string? completionendreached,
        string? completiontimespent, string? allowofflineattempts, ActivitiesLessonXmlPages? pages,
        string? grades, string? timers, string? overrides, string? id);
}