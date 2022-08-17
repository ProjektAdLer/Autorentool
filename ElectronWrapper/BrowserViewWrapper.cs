using ElectronNET.API;
using ElectronNET.API.Entities;
using System.Threading.Tasks;


namespace ElectronWrapper;

/// <summary>
/// A BrowserView can be used to embed additional web content into a BrowserWindow. 
/// It is like a child window, except that it is positioned relative to its owning window. 
/// It is meant to be an alternative to the webview tag.
/// </summary>
class BrowserViewWrapper : IBrowserViewWrapper
{
	public  BrowserViewWrapper()
	{
		var tmp = Electron.WindowManager.CreateBrowserViewAsync();

		while (tmp.Result == null)
		{
			Task.Delay(100).Wait();
		}

		browserView = tmp.Result;
	}


	private BrowserView browserView;
	/// <summary>
	/// Gets the identifier.
	/// </summary>
	/// <value>
	/// The identifier.
	/// </value>
	public int Id => browserView.Id;

	//Set Methode ist Internal also hier nicht möglich?
	/// <summary>
	/// Render and control web pages.
	/// </summary>
	public WebContents WebContents => browserView.WebContents;

	//Set Methode ist Internal also hier nicht möglich?
	/// <summary>
	/// Resizes and moves the view to the supplied bounds relative to the window.
	/// 
	/// (experimental)
	/// </summary>
	public Rectangle Bounds
	{
		get => browserView.Bounds;
		set => browserView.Bounds = value;
	}
	/*
	/// <summary>
	/// BrowserView
	/// </summary>
	Constructor ist Intern
	*/

	/// <summary>
	/// (experimental)
	/// </summary>
	/// <param name="options"></param>
	public void SetAutoResize(AutoResizeOptions options)
	{
		browserView.SetAutoResize(options);
	}

	/// <summary>
	/// Color in #aarrggbb or #argb form. The alpha channel is optional.
	/// 
	/// (experimental)
	/// </summary>
	/// <param name="color">Color in #aarrggbb or #argb form. The alpha channel is optional.</param>
	public void SetBackgroundColor(string color)
	{
		browserView.SetBackgroundColor(color);
	}

	// JsonSerializer nur im Original Objekt nötig.
}