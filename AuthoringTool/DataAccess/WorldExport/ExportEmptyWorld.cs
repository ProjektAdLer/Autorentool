using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;


namespace AuthoringTool.DataAccess.WorldExport;

public class ExportEmptyWorld : IExportEmptyWorld
{

    public void ModifyExistingXMLStructure()
    {
        //get current time ONCE so its consistent
        var currTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            
        //copy template from current workdir
        var tempDir = GetTempDir();
        DirectoryCopy("TemplateMoodleExport", tempDir);

        //get paths
        var coursePath = tempDir + "/course/course.xml";
        var moodle_backupPath = tempDir + "/moodle_backup.xml";

        //load xdocuments
        var course = XDocument.Load(coursePath);
        var moodle_backup = XDocument.Load(moodle_backupPath);
        moodle_backup.Declaration = new XDeclaration("1.0", "UTF-8", "");
         
        //change moodle_backup
        var info = moodle_backup.Root.Element("information");
        info.Element("original_course_fullname").SetValue("EmptyWorld");
        info.Element("original_course_shortname").SetValue("EW");
        /*info.Element("backup_date").SetValue(currTime);
        info.Element("original_course_startdate").SetValue(currTime);
        info.Element("original_course_enddate").SetValue(0);*/
            
        var settings = new XmlWriterSettings 
        {
            Encoding = new UpperCaseUTF8Encoding(), // Moodle needs Encoding in Uppercase!

            NewLineHandling = System.Xml.NewLineHandling.Replace,
            NewLineOnAttributes = true,
            Indent = true                           // Generate new lines for each element
        };

        using (var xmlWriter =XmlTextWriter.Create(moodle_backupPath, settings)) 
        {
            moodle_backup.Save(xmlWriter); 
        }

        //construct tarball 
        const string tarName = "EmptyWorld.mbz";
        var tarPath = Path.Combine(tempDir, tarName);

        using(var outStream = File.Create(tarPath))
        using(var gzoStream = new GZipOutputStream(outStream))
        using(var tarArchive = TarArchive.CreateOutputTarArchive(gzoStream))
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
    /// Change the encoding from utf-8 to UTF-8
    /// Otherwise moodle will throw an error when the backup is restored.
    /// </summary>
    public class UpperCaseUTF8Encoding : UTF8Encoding
    { 
        public override string WebName
        {
            get { return base.WebName.ToUpper(); }
        }
        //Check if encoding already exists
        public static UpperCaseUTF8Encoding UpperCaseUTF8
        {
            get
            {
                if (upperCaseUtf8Encoding == null) 
                { 
                    upperCaseUtf8Encoding = new UpperCaseUTF8Encoding();
                }
                return upperCaseUtf8Encoding;
            }
        }
        private static UpperCaseUTF8Encoding upperCaseUtf8Encoding = null;
    }

    /// <summary>
    /// Creates a temporary directory in the users temporary user folder and returns the path to it.
    /// </summary>
    /// <returns>Path to the temporary directory.</returns>
    private string GetTempDir()
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
    private void DirectoryCopy(string source, string targetPrefix)
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

