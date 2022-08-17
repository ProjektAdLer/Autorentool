using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;


namespace ElectronWrapper;

/// <summary>
/// Perform copy and paste operations on the system clipboard.
/// </summary>
class ClipboardWrapper : IClipboardWrapper
{
    private Clipboard clipboard;

    public ClipboardWrapper()
    {
        clipboard = Electron.Clipboard;
    }
    /// <summary>
    /// Read the content in the clipboard as plain text.
    /// </summary>
    /// <param name="type"></param>
    /// <returns>The content in the clipboard as plain text.</returns>
    public Task<string> ReadTextAsync(string type = "")
    {
        return clipboard.ReadTextAsync(type);
    }


    /// <summary>
    /// Writes the text into the clipboard as plain text.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="type"></param>
    public void WriteText(string text, string type = "")
    {
        clipboard.WriteText(text, type);
    }

    /// <summary>
    /// The content in the clipboard as markup.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Task<string> ReadHtmlAsync(string type = "")
    {
        return clipboard.ReadHTMLAsync(type);
    }

    /// <summary>
    /// Writes markup to the clipboard.
    /// </summary>
    /// <param name="markup"></param>
    /// <param name="type"></param>
    public void WriteHtml(string markup, string type = "")
    {
        clipboard.WriteHTML(markup, type);
    }

    /// <summary>
    /// The content in the clipboard as RTF.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Task<string> ReadRtfAsync(string type = "")
    {
        return clipboard.ReadRTFAsync(type);
    }



    /// <summary>
    /// Writes the text into the clipboard in RTF.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="type"></param>
    public void WriteRtf(string text, string type = "")
    {
        clipboard.WriteRTF(text, type);
    }


    /// <summary>
    /// Returns an Object containing title and url keys representing 
    /// the bookmark in the clipboard. The title and url values will 
    /// be empty strings when the bookmark is unavailable.
    /// </summary>
    /// <returns></returns>
    public Task<ReadBookmark> ReadBookmarkAsync()
    {
        return clipboard.ReadBookmarkAsync();
    }

    /// <summary>
    /// Writes the title and url into the clipboard as a bookmark.
    /// 
    /// Note: Most apps on Windows don’t support pasting bookmarks
    /// into them so you can use clipboard.write to write both a 
    /// bookmark and fallback text to the clipboard.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="url"></param>
    /// <param name="type"></param>
    public void WriteBookmark(string title, string url, string type = "")
    {
        clipboard.WriteBookmark(title, url, type);
    }


    /// <summary>
    /// macOS: The text on the find pasteboard. This method uses synchronous IPC
    /// when called from the renderer process. The cached value is reread from the
    /// find pasteboard whenever the application is activated.
    /// </summary>
    /// <returns></returns>
    public Task<string> ReadFindTextAsync()
    {
        return clipboard.ReadFindTextAsync();
    }

    /// <summary>
    /// macOS: Writes the text into the find pasteboard as plain text. This method uses 
    /// synchronous IPC when called from the renderer process.
    /// </summary>
    /// <param name="text"></param>
    public void WriteFindText(string text)
    {
        clipboard.WriteFindText(text);
    }

    /// <summary>
    /// Clears the clipboard content.
    /// </summary>
    /// <param name="type"></param>
    public void Clear(string type = "")
    {
        clipboard.Clear(type);
    }

    /// <summary>
    /// An array of supported formats for the clipboard type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Task<string[]> AvailableFormatsAsync(string type = "")
    {
        return clipboard.AvailableFormatsAsync(type);
    }


    /// <summary>
    /// Writes data to the clipboard.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="type"></param>
    public void Write(Data data, string type = "")
    {
        clipboard.Write(data, type);
    }


    /// <summary>
    /// Reads an image from the clipboard.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Task<NativeImage> ReadImageAsync(string type = "")
    {
        return clipboard.ReadImageAsync(type);
    }


    /// <summary>
    /// Writes an image to the clipboard.
    /// </summary>
    /// <param name="image"></param>
    /// <param name="type"></param>
    public void WriteImage(NativeImage image, string type = "")
    {
        clipboard.WriteImage(image, type);
    }
}