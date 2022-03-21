using System.Xml;
using System.Xml.Linq;
using AuthoringTool.DataAccess.WorldExport;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using AuthoringTool.DataAccess.XmlClasses;
using AuthoringTool.DataAccess.XmlClasses.course;
using AuthoringTool.DataAccess.XmlClasses.sections;



namespace AuthoringTool.DataAccess.WorldExport;

public class ConstructBackupFile : IConstructBackupFile
{

    public void CreateXMLFiles()
    {
 
            //define Xml-files 
            var initRolesXml = new RolesXmlInit().Init();
            var initFilesXml = new FilesXmlInit().Init();
            var initGradebookXml = new GradebookXmlInit().Init();
            var initOutcomeXml = new OutcomesXmlInit().Init();
            var initQuestionsXml = new QuestionsXmlInit().Init();
            var initScalesXml = new ScalesXmlInit().Init();
            var initGroupsXml = new GroupsXmlInit().Init();
            var initCourseInforefXml = new CourseInforefXmlInit().Init();
            var initCourseRolesXml = new CourseRolesXmlInit().Init();
            var initCourseCourseXml = new CourseCourseXmlInit().Init();
            var initCourseEnrolmentsXml = new CourseEnrolmentsXmlInit().Init();
            var initSectionsSectionXml = new SectionsSectionXmlInit().Init();
            var initMoodleBackupXml = new MoodleBackupXmlInit().Init();
            

            //create xml-files
            var currWorkDir = Directory.GetCurrentDirectory();
            Directory.CreateDirectory( currWorkDir+"/XMLFilesForExport");
            var xml = new XmlSer();
            xml.serialize(initRolesXml, "roles.xml");
            xml.serialize(initFilesXml, "files.xml");
            xml.serialize(initGradebookXml, "gradebook.xml");
            xml.serialize(initOutcomeXml, "outcomes.xml");
            xml.serialize(initQuestionsXml, "questions.xml");
            xml.serialize(initScalesXml, "scales.xml");
            xml.serialize(initGroupsXml, "groups.xml");
            xml.serialize(initMoodleBackupXml, "moodle_backup.xml");
        
            Directory.CreateDirectory( currWorkDir+"/XMLFilesForExport/course");
            xml.serialize(initCourseInforefXml, "course/inforef.xml"); 
            xml.serialize(initCourseRolesXml, "course/roles.xml");
            xml.serialize(initCourseCourseXml, "course/course.xml");
            xml.serialize(initCourseEnrolmentsXml, "course/enrolments.xml");
            
            Directory.CreateDirectory( currWorkDir+"/XMLFilesForExport/sections");
            Directory.CreateDirectory( currWorkDir+"/XMLFilesForExport/sections/section_160");
            xml.serialize(initSectionsSectionXml, "sections/section_160/section.xml");
    }

    public void OverwriteEncoding()
    {
            //load xml files to change encoding to Uppercase UTF-8
            var tempDir = Directory.GetCurrentDirectory()+"/XMLFilesForExport";
            var rolesPath = tempDir + "/roles.xml";
            var filesPath = tempDir + "/files.xml";
            var gradebookPath = tempDir + "/gradebook.xml";
            var outcomesPath = tempDir + "/outcomes.xml";
            var questionPath = tempDir + "/questions.xml";
            var scalesPath = tempDir + "/scales.xml";
            var groupsPath = tempDir + "/groups.xml";
            var moodleBackupPath = tempDir + "/moodle_backup.xml";
            var courseInforefPath = tempDir + "/course/inforef.xml";
            var courseRolesPath = tempDir + "/course/roles.xml";
            var courseCoursePath = tempDir + "/course/course.xml";
            var courseEnrolmentsPath = tempDir + "/course/enrolments.xml";
            var sectionsSectionPath = tempDir + "/sections/section_160/section.xml";
            
            var moodleBackupRoles = XDocument.Load(rolesPath);
            var moodleBackupFiles = XDocument.Load(filesPath);
            var moodleBackupGradebook = XDocument.Load(gradebookPath);
            var moodleBackupOutcomes = XDocument.Load(outcomesPath);
            var moodleBackupQuestions = XDocument.Load(questionPath);
            var moodleBackupScales = XDocument.Load(scalesPath);
            var moodleBackupGroups = XDocument.Load(groupsPath);
            var moodleBackupDocument = XDocument.Load(moodleBackupPath);
            var moodleBackupCourseInforef = XDocument.Load(courseInforefPath);
            var moodleBackupCourseRoles = XDocument.Load(courseRolesPath);
            var moodleBackupCourseCourse = XDocument.Load(courseCoursePath);   
            var moodleBackupCourseEnrolments = XDocument.Load(courseEnrolmentsPath);   
            var moodleBackupSectionsSection = XDocument.Load(sectionsSectionPath);   
            
            var settings = new XmlWriterSettings
            {
                Encoding = new UpperCaseUTF8Encoding(), // Moodle needs Encoding in Uppercase!

                NewLineHandling = System.Xml.NewLineHandling.Replace,
                NewLineOnAttributes = true,
                Indent = true // Generate new lines for each element
            };
            
            using (var xmlWriter = XmlWriter.Create(rolesPath, settings))
            {
                moodleBackupRoles.Save(xmlWriter);
            }
            using (var xmlWriter = XmlWriter.Create(filesPath, settings))
            {
                moodleBackupFiles.Save(xmlWriter);
            }
            using (var xmlWriter = XmlWriter.Create(gradebookPath, settings))
            {
                moodleBackupGradebook.Save(xmlWriter);
            }
            using (var xmlWriter = XmlWriter.Create(outcomesPath, settings))
            {
                moodleBackupOutcomes.Save(xmlWriter);
            }
            using (var xmlWriter = XmlWriter.Create(questionPath, settings))
            {
                moodleBackupQuestions.Save(xmlWriter);
            }
            using (var xmlWriter = XmlWriter.Create(scalesPath, settings))
            {
                moodleBackupScales.Save(xmlWriter);
            }
            using (var xmlWriter = XmlWriter.Create(groupsPath, settings))
            {
                moodleBackupGroups.Save(xmlWriter);
            }
            using (var xmlWriter = XmlWriter.Create(courseInforefPath, settings))
            {
                moodleBackupCourseInforef.Save(xmlWriter);
            }
            using (var xmlWriter = XmlWriter.Create(courseRolesPath, settings))
            {
                moodleBackupCourseRoles.Save(xmlWriter);
            }
            using (var xmlWriter = XmlWriter.Create(courseCoursePath, settings))
            {
                moodleBackupCourseCourse.Save(xmlWriter);
            }
            using (var xmlWriter = XmlWriter.Create(courseEnrolmentsPath, settings))
            {
                moodleBackupCourseEnrolments.Save(xmlWriter);
            }
            using (var xmlWriter = XmlWriter.Create(sectionsSectionPath, settings))
            {
                moodleBackupSectionsSection.Save(xmlWriter);
            }
            using (var xmlWriter = XmlWriter.Create(moodleBackupPath, settings))
            {
                moodleBackupDocument.Save(xmlWriter);
            }
    }
    
    public void CreateBackupFile()
    {
        //copy template from current workdir
        var tempDir = GetTempDir();
        DirectoryCopy("XMLFilesForExport", tempDir);

        //construct tarball 
        const string tarName = "EmptyWorld.mbz";
        var tarPath = Path.Combine(tempDir, tarName);

        using (var outStream = File.Create(tarPath))
        using (var gzoStream = new GZipOutputStream(outStream))
        using (var tarArchive = TarArchive.CreateOutputTarArchive(gzoStream))
        {
            tarArchive.RootPath = tempDir;
            SaveDirectoryToTar(tarArchive, tempDir, true);
        }

        //move file and delete dir
        if (File.Exists(tarName))
        {
            File.Delete(tarName);
        }

        File.Move(tarPath, tarName);
        Directory.Delete(tempDir, true);
    }

    /// <summary>
    /// Creates a temporary directory in the users temporary user folder and returns the path to it.
    /// </summary>
    /// <returns>Path to the temporary directory.</returns>
    public string GetTempDir()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempDir);
        return tempDir;
    }

    /// <summary>
    /// Copies an entire directories contents into a target.
    /// </summary>
    /// <param name="source">The path of the folder which should be copied.</param>
    /// <param name="targetPrefix">The path of the target folder to which the contents should be copied.</param>
    public void DirectoryCopy(string source, string targetPrefix)
    {
        var directories = Directory.EnumerateDirectories(source, "*", SearchOption.AllDirectories);
        foreach (var directory in directories)
        {
            var directoryName = directory.Remove(0, (source + "\\").Length);
            Directory.CreateDirectory(Path.Combine(targetPrefix, directoryName));
        }
        var files = Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories);
        foreach (var file in files) 
        {
            var filename = file.Remove(0, (source + "\\").Length);
            File.Copy(file, Path.Combine(targetPrefix, filename));
        }
    }
    
    /// <summary>
    /// Saves a directory and its contents (recursively) into a given tar archive.
    /// </summary>
    /// <param name="tar">The tar archive to which the directory and its contents should be saved.</param>
    /// <param name="source">The path of the source folder which should be saved to the archive.</param>
    /// <param name="recursive">Whether or not directories in the source should be saved recursively too.</param>
    private void SaveDirectoryToTar(TarArchive tar, string source, bool recursive)
    {
        TarEntry tarEntry = TarEntry.CreateEntryFromFile(source);
        tar.WriteEntry(tarEntry, false);
        var filenames = Directory.GetFiles(source).Where(s => !s.EndsWith(".mbz"));
        foreach (string filename in filenames)
        {
            tarEntry = TarEntry.CreateEntryFromFile(filename);
            tar.WriteEntry(tarEntry, false);
        }
        if (recursive)
        {
            string[] directories = Directory.GetDirectories(source);
            foreach (string directory in directories)
                SaveDirectoryToTar(tar, directory, recursive);
        }
    }
}

