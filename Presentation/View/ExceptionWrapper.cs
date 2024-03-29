using System.Text;

namespace Presentation.View;

public class ExceptionWrapper
{
    public string CallSite { get; }
    public Exception? Exception { get; }
    
    public ExceptionWrapper(string callSite)
    {
        CallSite = callSite;
        Exception = null;
    }

    public ExceptionWrapper(string callSite, Exception exception)
    {
        CallSite = callSite;
        Exception = exception;
    }

    public override string ToString()
    {
        return PrettyPrint();
    }

    public string PrettyPrint()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append($"Exception encountered at {CallSite}");
        if (Exception is null)
        {
            stringBuilder.Append('.');
            return stringBuilder.ToString();
        }

        stringBuilder.AppendLine(":");
        stringBuilder.AppendLine("Exception:");
        var exception = Exception;
        while (exception != null)
        {
            stringBuilder.Append(exception.Message);
            if (exception.InnerException != null)
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("Inner Exception:");
            }
            exception = exception.InnerException;
        }

        return stringBuilder.ToString();
    }

}