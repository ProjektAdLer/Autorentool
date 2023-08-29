using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using NSubstitute;

namespace TestHelpers;

public static class LocalizerExtensions
{
    public static IStringLocalizer<T> AddLocalizerForTest<T>(this TestContext context)
    {
        var localizer = GetConfiguredLocalizer<T>();
        context.Services.AddSingleton(localizer);
        return localizer;
    }

    public static IStringLocalizerFactory AddLocalizerFactoryForTest(this TestContext context)
    {
        var localizerFactory = Substitute.For<IStringLocalizerFactory>();
        var localizer = GetConfiguredLocalizer();
        localizerFactory.Create(Arg.Any<Type>()).Returns(localizer);
        localizerFactory.Create(Arg.Any<string>(), Arg.Any<string>()).Returns(localizer);
        context.Services.AddSingleton(localizerFactory);
        return localizerFactory;
    }

    private static IStringLocalizer<T> GetConfiguredLocalizer<T>()
    {
        var localizer = Substitute.For<IStringLocalizer<T>>();
        localizer[Arg.Any<string>()].Returns(ci => new LocalizedString(ci.Arg<string>(), ci.Arg<string>()));
        localizer[Arg.Any<string>(), Arg.Any<object[]>()].Returns(ci =>
            new LocalizedString(ci.Arg<string>() + string.Concat(ci.Arg<object[]>()),
                ci.Arg<string>() + string.Concat(ci.Arg<object[]>())));
        return localizer;
    }

    private static IStringLocalizer GetConfiguredLocalizer()
    {
        var localizer = Substitute.For<IStringLocalizer>();
        localizer[Arg.Any<string>()].Returns(ci => new LocalizedString(ci.Arg<string>(), ci.Arg<string>()));
        localizer[Arg.Any<string>(), Arg.Any<object[]>()].Returns(ci =>
            new LocalizedString(ci.Arg<string>() + string.Concat(ci.Arg<object[]>()),
                ci.Arg<string>() + string.Concat(ci.Arg<object[]>())));
        return localizer;
    }
}