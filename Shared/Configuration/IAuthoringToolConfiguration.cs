
using Microsoft.Extensions.Logging;

namespace Shared.Configuration;

public interface IAuthoringToolConfiguration
{
    ILogger Logger { get; set; }
}