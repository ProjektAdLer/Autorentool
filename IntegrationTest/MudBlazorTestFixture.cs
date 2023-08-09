using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using PresentationTest;
using TestHelpers;
using TestContext = Bunit.TestContext;

namespace IntegrationTest;

public class MudBlazorTestFixture<T> where T : ComponentBase
{
    protected IStringLocalizer<T> Localizer { get; set; }
    protected TestContext Context { get; set; }


    [SetUp]
    public void Setup()
    {
        Context = new TestContext();
        Context.AddMudBlazorTestServices();
        Localizer = Context.AddLocalizerForTest<T>();
        Context.Services.AddSingleton(Localizer);
        Context.Services.AddLogging(builder => builder.AddProvider(NullLoggerProvider.Instance));
    }
}