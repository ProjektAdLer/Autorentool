using Microsoft.Extensions.Localization;
using NSubstitute;
using NUnit.Framework;
using Shared;
using Shared.Theme;

namespace SharedTest;

[TestFixture]
public class ThemeHelperUt
{
    [SetUp]
    public void Setup()
    {
        _localizer = Substitute.For<IStringLocalizer<SpaceTheme>>();
        _localizer[Arg.Any<string>()].Returns(ci => new LocalizedString(ci.Arg<string>(), ci.Arg<string>()));
    }

    private IStringLocalizer<SpaceTheme> _localizer = null!;

    [Test]
    public void Localize_WhenNotInitialized_ThrowsException()
    {
        // Arrange
        ThemeHelper<SpaceTheme>.Initialize(null!);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => ThemeHelper<SpaceTheme>.Localize(SpaceTheme.Suburb));
    }

    [Test]
    public void Localize_WhenInitialized_ReturnsLocalizedValue()
    {
        // Arrange
        ThemeHelper<SpaceTheme>.Initialize(_localizer);

        foreach (SpaceTheme theme in Enum.GetValues(typeof(SpaceTheme)))
        {
            // Act
            var localizedValue = ThemeHelper<SpaceTheme>.Localize(theme);

            // Assert
            Assert.That(localizedValue, Is.EqualTo($"Enum.Theme.{theme}"));
        }
    }
}