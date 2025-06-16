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
        localizer["LearningWorldValidator.Name.Duplicate"].Returns(new LocalizedString("en","Already in use."));
        localizer["LearningWorldValidator.Shortname.Duplicate"].Returns(new LocalizedString("en","Already in use."));
        localizer["LearningWorldValidator.Name.Valid"].Returns(new LocalizedString("en","Valid name."));
        localizer["LearningWorldValidator.Shortname.Valid"].Returns(new LocalizedString("en","Valid shortname."));
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
        localizer["LearningWorldValidator.Name.Duplicate"].Returns(new LocalizedString("en","Already in use."));
        localizer["LearningWorldValidator.Shortname.Duplicate"].Returns(new LocalizedString("en","Already in use."));
        localizer["LearningWorldValidator.Name.Valid"].Returns(new LocalizedString("en","Valid name."));
        localizer["LearningWorldValidator.Shortname.Valid"].Returns(new LocalizedString("en","Valid shortname."));
        var namesProvider = Substitute.For<ILearningWorldNamesProvider>();
        namesProvider.WorldNames.Returns(new[] { (new Guid(), "a") });
        namesProvider.WorldShortnames.Returns(new[] { (new Guid(), "b") });
        var validator = new LearningWorldPropertyValidator(namesProvider, localizer);
        var entity = EntityProvider.GetLearningWorld();

        var sut = GetSystemUnderTest(validator);

        var nameErrors = (await sut.ValidateAsync(entity, "Name")).ToArray();
        var shortnameErrors = (await sut.ValidateAsync(entity, "Shortname")).ToArray();
        Assert.That(nameErrors, Has.Length.EqualTo(1));
        Assert.That(nameErrors, Does.Contain("Already in use."));
        Assert.That(shortnameErrors, Has.Length.EqualTo(1));
        Assert.That(shortnameErrors, Does.Contain("Already in use."));
    }

    private ValidationWrapper<T> GetSystemUnderTest<T>(IValidator<T> validator)
    {
        return new(validator);
    }
}