﻿using ElectronSharp.API.Entities;
using System.Threading.Tasks;

namespace ElectronWrapper;

interface IClipboardWrapper
{
    Task<string[]> AvailableFormatsAsync(string type = "");
    void Clear(string type = "");
    Task<string> ReadHtmlAsync(string type = "");
    Task<NativeImage> ReadImageAsync(string type = "");
    Task<string> ReadRtfAsync(string type = "");
    Task<string> ReadTextAsync(string type = "");
    void Write(Data data, string type = "");
    void WriteHtml(string markup, string type = "");
    void WriteImage(NativeImage image, string type = "");
    void WriteRtf(string text, string type = "");
    void WriteText(string text, string type = "");
}