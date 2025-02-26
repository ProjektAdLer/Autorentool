using ElectronSharp.API;
using ElectronSharp.API.Entities;

namespace ElectronWrapper;

public interface IBrowserViewWrapper
{
	int Id { get; }
	WebContents WebContents { get; }
	void SetAutoResize(AutoResizeOptions options);
	void SetBackgroundColor(string color);
}