using System;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Validation;
using BusinessLogic.Validation.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms;
using TestHelpers;

namespace IntegrationTest.Forms;

[TestFixture]
public class ValidationWrapperIt
{
    [Test]
    // ANF-ID: [AHO22]
    public async Task LearningWorld_WithLearningWorldValidator_ValidEntity_IsValidated()
    {
        var localizer = Substitute.For<IStringLocalizer<LearningWorldPropertyValidator>>();
        localizer[Arg.Any<string>()].Returns(callInfo => new LocalizedString("en", callInfo.Arg<string>()));
        var namesProvider = Substitute.For<ILearningWorldNamesProvider>();
        var validator = new LearningWorldPropertyValidator(namesProvider, localizer);
        var entity = EntityProvider.GetLearningWorld();

        var sut = GetSystemUnderTest(validator);

        Assert.That(await sut.ValidateAsync(entity, "Name"), Is.Empty);
        Assert.That(await sut.ValidateAsync(entity, "Shortname"), Is.Empty);
    }

    [Test]
    // ANF-ID: [AHO22]
    public async Task LearningWorld_WithLearningWorldValidator_InvalidEntity_GivesErrors()
    {
        var localizer = Substitute.For<IStringLocalizer<LearningWorldPropertyValidator>>();
        localizer[Arg.Any<string>()].Returns(callInfo => new LocalizedString("en", callInfo.Arg<string>()));
        var namesProvider = Substitute.For<ILearningWorldNamesProvider>();
        namesProvider.WorldNames.Returns(new[] { (new Guid(), "a") });
        namesProvider.WorldShortnames.Returns(new[] { (new Guid(), "b") });
        var validator = new LearningWorldPropertyValidator(namesProvider, localizer);
        var entity = EntityProvider.GetLearningWorld();

        var sut = GetSystemUnderTest(validator);

        var nameErrors = (await sut.ValidateAsync(entity, "Name")).ToArray();
        var shortnameErrors = (await sut.ValidateAsync(entity, "Shortname")).ToArray();
        Assert.That(nameErrors, Has.Length.EqualTo(1));
        Assert.That(nameErrors.First(), Does.Contain(".Name.Duplicate"));
        Assert.That(shortnameErrors, Has.Length.EqualTo(1));
        Assert.That(shortnameErrors.First(), Does.Contain(".Shortname.Duplicate"));
    }

    private ValidationWrapper<T> GetSystemUnderTest<T>(IValidator<T> validator)
    {
        return new(validator);
    }
}