using System.Text;

namespace AuthoringTool.DataAccess.WorldExport;


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