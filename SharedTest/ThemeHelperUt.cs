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
        Assert.Throws<InvalidOperationException>(() => ThemeHelper<SpaceTheme>.Localize(SpaceTheme.LearningArea));
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
            Assert.That(localizedValue, Is.EqualTo($"Enum.SpaceTheme.{theme}"));
        }
    }
    
    [Test]
    public void Localize_WithContext_KeyFound_ReturnsContextSpecificValue()
    {
        // Arrange
        ThemeHelper<SpaceTheme>.Initialize(_localizer);

        var theme = SpaceTheme.LearningArea;
        var context = "Card";

        var contextKey = $"Enum.SpaceTheme.{theme}.{context}";

        _localizer[contextKey].Returns(new LocalizedString(contextKey, "Localized with context", false));

        // Act
        var result = ThemeHelper<SpaceTheme>.Localize(theme, context);

        // Assert
        Assert.That(result, Is.EqualTo("Localized with context"));
    }

    [Test]
    public void Localize_WithContext_KeyNotFound_ReturnsBaseKeyValue()
    {
        // Arrange
        ThemeHelper<SpaceTheme>.Initialize(_localizer);

        var theme = SpaceTheme.LearningArea;
        var context = "Card";

        var contextKey = $"Enum.SpaceTheme.{theme}.{context}";
        var baseKey = $"Enum.SpaceTheme.{theme}";

        _localizer[contextKey].Returns(new LocalizedString(contextKey, "Missing", true)); // ResourceNotFound = true
        _localizer[baseKey].Returns(new LocalizedString(baseKey, "Base Value", false));

        // Act
        var result = ThemeHelper<SpaceTheme>.Localize(theme, context);

        // Assert
        Assert.That(result, Is.EqualTo("Base Value"));
    }

}