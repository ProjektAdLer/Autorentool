using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BusinessLogic.Validation.Validators.CustomValidators;

public static class HttpValidator
{
    private const string UrlRegex = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
    public static IRuleBuilderOptions<T, string> IsHttpOrHttpsUrl<T>(this IRuleBuilder<T, string> ruleBuilder, string message)
    {
        return ruleBuilder
            .Must(IsHttpOrHttpsUrl)
            .WithMessage(message);
    }
    
    private static bool IsHttpOrHttpsUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)
               && Regex.IsMatch(url, UrlRegex);
    }
}