using System.Threading.Tasks;
using ElectronSharp.API;
using ElectronSharp.API.Entities;


namespace ElectronWrapper;

/// <summary>
/// Perform copy and paste operations on the system clipboard.
/// </summary>
class ClipboardWrapper : IClipboardWrapper
{
    private readonly Clipboard _clipboard = Electron.Clipboard;

    /// <summary>
    /// Read the content in the clipboard as plain text.
    /// </summary>
    /// <param name="type"></param>
    /// <returns>The content in the clipboard as plain text.</returns>
    public Task<string> ReadTextAsync(string type = "")
    {
        return _clipboard.ReadTextAsync(type);
    }


    /// <summary>
    /// Writes the text into the clipboard as plain text.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="type"></param>
    public void WriteText(string text, string type = "")
    {
        _clipboard.WriteText(text, type);
    }

    /// <summary>
    /// The content in the clipboard as markup.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Task<string> ReadHtmlAsync(string type = "")
    {
        return _clipboard.ReadHTMLAsync(type);
    }

    /// <summary>
    /// Writes markup to the clipboard.
    /// </summary>
    /// <param name="markup"></param>
    /// <param name="type"></param>
    public void WriteHtml(string markup, string type = "")
    {
        _clipboard.WriteHTML(markup, type);
    }

    /// <summary>
    /// The content in the clipboard as RTF.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Task<string> ReadRtfAsync(string type = "")
    {
        return _clipboard.ReadRTFAsync(type);
    }



    /// <summary>
    /// Writes the text into the clipboard in RTF.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="type"></param>
    public void WriteRtf(string text, string type = "")
    {
        _clipboard.WriteRTF(text, type);
    }

    /// <summary>
    /// Clears the clipboard content.
    /// </summary>
    /// <param name="type"></param>
    public void Clear(string type = "")
    {
        _clipboard.Clear(type);
    }

    /// <summary>
    /// An array of supported formats for the clipboard type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Task<string[]> AvailableFormatsAsync(string type = "")
    {
        return _clipboard.AvailableFormatsAsync(type);
    }


    /// <summary>
    /// Writes data to the clipboard.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="type"></param>
    public void Write(Data data, string type = "")
    {
        _clipboard.Write(data, type);
    }


    /// <summary>
    /// Reads an image from the clipboard.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Task<NativeImage> ReadImageAsync(string type = "")
    {
        return _clipboard.ReadImageAsync(type);
    }


    /// <summary>
    /// Writes an image to the clipboard.
    /// </summary>
    /// <param name="image"></param>
    /// <param name="type"></param>
    public void WriteImage(NativeImage image, string type = "")
    {
        _clipboard.WriteImage(image, type);
    }
}