using System.Text;

namespace AuthoringTool.DataAccess.WorldExport;

/// <summary>
/// The purpose of this class is to change the Encoding from "utf-8" to "UTF-8"
/// Moodle needs the Encoding in Uppercase Format, else the Backup Restore process will throw an error.
/// </summary>
public class UpperCaseUTF8Encoding : UTF8Encoding
{ 
    public override string WebName
    {
        get { return base.WebName.ToUpper(); }
    }
    //Check if encoding already exists

    private static UpperCaseUTF8Encoding? upperCaseUtf8Encoding;
}