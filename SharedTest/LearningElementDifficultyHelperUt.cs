using Microsoft.Extensions.Localization;
using NSubstitute;
using NUnit.Framework;
using Shared;

namespace SharedTest;

[TestFixture]
public class LearningElementDifficultyHelperUt
{
    [SetUp]
    public void Setup()
    {
        _localizer = Substitute.For<IStringLocalizer<LearningElementDifficultyEnum>>();
        _localizer[Arg.Any<string>()].Returns(ci => new LocalizedString(ci.Arg<string>(), ci.Arg<string>()));
    }

    private IStringLocalizer<LearningElementDifficultyEnum> _localizer = null!;

    [Test]
    public void Localize_WhenNotInitialized_ThrowsException()
    {
        // Arrange
        Shared.LearningElementDifficultyHelper.Initialize(null);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            Shared.LearningElementDifficultyHelper.Localize(LearningElementDifficultyEnum.Medium));
    }

    [Test]
    public void Localize_WhenInitialized_ReturnsLocalizedValue([Values] LearningElementDifficultyEnum difficulty)
    {
        // Arrange
        LearningElementDifficultyHelper.Initialize(_localizer);

        // Act
        var localizedValue = LearningElementDifficultyHelper.Localize(difficulty);

        // Assert
        Assert.That(localizedValue, Is.EqualTo($"Enum.LearningElementDifficultyEnum.{difficulty}"));
    }
}