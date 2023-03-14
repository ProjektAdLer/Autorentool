using System.Text.RegularExpressions;
using FluentValidation;

namespace BusinessLogic.Validation.Validators.CustomValidators;

public static class HttpValidator
{
    private const string UrlRegex = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
    public static IRuleBuilderOptions<T, string> IsHttpOrHttpsUrl<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(IsHttpOrHttpsUrl)
            .WithMessage("Must be a valid URL (e.g. https://www.youtube.com/watch?v=dQw4w9WgXcQ)");
    }
    
    private static bool IsHttpOrHttpsUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)
               && Regex.IsMatch(url, UrlRegex);
    }
}