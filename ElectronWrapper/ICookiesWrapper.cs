using ElectronNET.API.Entities;
using System;
using System.Threading.Tasks;

namespace ElectronWrapper;

interface ICookiesWrapper
{
    int Id { get; }

    event Action<Cookie, CookieChangedCause, bool> OnChanged;

    Task FlushStoreAsync();
    Task<Cookie[]> GetAsync(CookieFilter filter);
    Task RemoveAsync(string url, string name);
    Task SetAsync(CookieDetails details);
}