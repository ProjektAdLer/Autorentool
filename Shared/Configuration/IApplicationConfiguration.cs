
using Microsoft.Extensions.Logging;

namespace Shared.Configuration;

public interface IApplicationConfiguration
{
    IObservableDictionary<string, string> Configuration { get; }
    string this[string key] { get; set; }
}