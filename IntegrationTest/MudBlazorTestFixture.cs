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


    /// <summary>
    /// The SetUp attribute is inherited from any base class.
    /// Therefore, if a base class has defined a SetUp method, that method will be called before each test method in the derived class.
    /// You may define a SetUp method in the base class and another in the derived class.
    /// NUnit will call base class SetUp methods before those in the derived classes.
    /// [NUnit Documentation: SetUp Attribute](https://docs.nunit.org/articles/nunit/writing-tests/attributes/setup.html#inheritance)
    /// </summary>
    [SetUp]
    public void Setup()
    {
        Context = new TestContext();
        Context.AddMudBlazorTestServices();
        Localizer = Context.AddLocalizerForTest<T>();
        Context.Services.AddSingleton(Localizer);
        Context.Services.AddLogging(builder => builder.AddProvider(NullLoggerProvider.Instance));
    }

    /// <summary>
    /// The TearDown attribute is inherited from any base class.
    /// Therefore, if a base class has defined a TearDown method, that method will be called after each test method in the derived class.
    /// You may define a TearDown method in the base class and another in the derived class.
    /// NUnit will call base class TearDown methods after those in the derived classes.
    /// [NUnit Documentation: TearDown Attribute](https://docs.nunit.org/articles/nunit/writing-tests/attributes/teardown.html#inheritance)
    /// </summary>
    [TearDown]
    public void TearDown()
    {
        Context.Dispose();
    }
}