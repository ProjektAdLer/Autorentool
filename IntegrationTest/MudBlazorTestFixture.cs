using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Dialogues;
using PresentationTest;
using TestContext = Bunit.TestContext;

namespace IntegrationTest;

[TestFixture]
public class MudBlazorTestFixture<T>
{
    protected IStringLocalizer<T> Localizer { get; set; }
    protected TestContext Context { get; set; }
    
    [SetUp]
    public void Setup()
    {
        Context = new TestContext();
        Context.AddMudBlazorTestServices();
        Localizer = Substitute.For<IStringLocalizer<T>>();
        Localizer[Arg.Any<string>()].Returns(ci => new LocalizedString(ci.Arg<string>(), ci.Arg<string>()));
        Localizer[Arg.Any<string>(), Arg.Any<object[]>()].Returns(ci =>
            new LocalizedString(ci.Arg<string>() + string.Concat(ci.Arg<object[]>()),
                ci.Arg<string>() + string.Concat(ci.Arg<object[]>())));
        Context.Services.AddSingleton(Localizer);
    }

}