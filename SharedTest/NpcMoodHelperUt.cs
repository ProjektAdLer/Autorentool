using System.Resources;
using Microsoft.Extensions.Localization;
using NSubstitute;
using NUnit.Framework;
using Shared;

namespace SharedTest;

[TestFixture]
public class NpcMoodHelperUt
{
    [SetUp]
    public void Setup()
    {
        _localizer = Substitute.For<IStringLocalizer<NpcMood>>();
        _localizer[Arg.Any<string>()].Returns(ci => new LocalizedString(ci.Arg<string>(), ci.Arg<string>()));
    }

    private IStringLocalizer<NpcMood> _localizer = null!;

    [Test]
    public void Localize_WhenNotInitialized_ThrowsException()
    {
        // Arrange
        NpcMoodHelper.Initialize(null!);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            NpcMoodHelper.Localize(NpcMood.Happy));
    }

    [Test]
    public void Localize_WhenInitialized_ReturnsLocalizedValue([Values] NpcMood npcMood)
    {
        // Arrange
        NpcMoodHelper.Initialize(_localizer);

        // Act
        var localizedValue = NpcMoodHelper.Localize(npcMood);

        // Assert
        Assert.That(localizedValue, Is.EqualTo($"Enum.NpcMood.{npcMood}"));
    }
}