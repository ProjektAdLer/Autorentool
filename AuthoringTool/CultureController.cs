using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace AuthoringTool;

/// <summary>
/// Sets RequestCulture cookie for localization.
/// </summary>
[Route("[controller]/[action]")]
public class CultureController : Controller
{
    /// <summary>
    /// Sets the RequestCulture cookie to the given value.
    /// </summary>
    /// <param name="culture">The culture to set it to, e.g. "de-DE" or "en-US".</param>
    /// <param name="redirectUri">The URL to redirect to afterwards.</param>
    /// <returns>The redirect action.</returns>
    public IActionResult Set(string? culture, string redirectUri)
    {
        if (culture != null)
        {
            HttpContext.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(
                    new RequestCulture(culture, culture)));
        }

        return LocalRedirect(redirectUri);
    }
}