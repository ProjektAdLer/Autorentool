using System;
using System.Threading.Tasks;

namespace ElectronWrapper;

class BrowserViewWrapperFactory
{
	public Task<BrowserViewWrapper> Create()
	{
		//Lässt sich ohne Zugriff auf den Internal Konstruktor nur über den Umweg über den WindowManager erstellen, was nicht einfach so funktioniert hat.
		throw new NotImplementedException();
	}
}