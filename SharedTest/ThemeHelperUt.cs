using Microsoft.Extensions.Localization;
using NSubstitute;
using NUnit.Framework;
using Shared;

namespace SharedTest;

[TestFixture]
public class ThemeHelperUt
{
    [SetUp]
    public void Setup()
    {
        _localizer = Substitute.For<IStringLocalizer<Theme>>();
        _localizer[Arg.Any<string>()].Returns(ci => new LocalizedString(ci.Arg<string>(), ci.Arg<string>()));
    }

    private IStringLocalizer<Theme> _localizer = null!;

    [Test]
    public void Localize_WhenNotInitialized_ThrowsException()
    {
        // Arrange
        ThemeHelper.Initialize(null);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => ThemeHelper.Localize(Theme.Suburb));
    }

    [Test]
    public void Localize_WhenInitialized_ReturnsLocalizedValue()
    {
        // Arrange
        ThemeHelper.Initialize(_localizer);

        foreach (Theme theme in Enum.GetValues(typeof(Theme)))
        {
            // Act
            var localizedValue = ThemeHelper.Localize(theme);

            // Assert
            Assert.That(localizedValue, Is.EqualTo($"Enum.Theme.{theme}"));
        }
    }
}