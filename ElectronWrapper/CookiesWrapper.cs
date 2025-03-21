﻿using System;
using System.Threading.Tasks;
using ElectronSharp.API;
using ElectronSharp.API.Entities;

namespace ElectronWrapper;

/// <summary>
/// Query and modify a session's cookies.
/// </summary>
class CookiesWrapper : ICookiesWrapper
{
    private readonly Cookies _cookies;

    public CookiesWrapper()
    {
        var temp = Electron.WindowManager.CreateWindowAsync();

        while (temp.Result == null)
        {
            Task.Delay(100).Wait();
        }
        _cookies = temp.Result.WebContents.Session.Cookies;
    }

    /// <summary>
    /// Gets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public int Id => _cookies.Id;


    /// <summary>
    /// Emitted when a cookie is changed because it was added, edited, removed, or expired.
    /// </summary>
    public event Action<Cookie, CookieChangedCause, bool> OnChanged
    {
        add => _cookies.OnChanged += value;
        remove => _cookies.OnChanged -= value;
    }


    /// <summary>
    /// Sends a request to get all cookies matching filter, and resolves a callack with the response.
    /// </summary>
    /// <param name="filter">
    /// </param>
    /// <returns>A task which resolves an array of cookie objects.</returns>
    public Task<Cookie[]> GetAsync(CookieFilter filter)
    {
        return _cookies.GetAsync(filter);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="details"></param>
    /// <returns></returns>
    public Task SetAsync(CookieDetails details)
    {
        return _cookies.SetAsync(details);
    }

    /// <summary>
    /// Removes the cookies matching url and name
    /// </summary>
    /// <param name="url">The URL associated with the cookie.</param>
    /// <param name="name">The name of cookie to remove.</param>
    /// <returns>A task which resolves when the cookie has been removed</returns>
    public Task RemoveAsync(string url, string name)
    {
        return _cookies.RemoveAsync(url, name);
    }

    /// <summary>
    /// Writes any unwritten cookies data to disk.
    /// </summary>
    /// <returns>A task which resolves when the cookie store has been flushed</returns>
    public Task FlushStoreAsync()
    {
        return _cookies.FlushStoreAsync();
    }
}