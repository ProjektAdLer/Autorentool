using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Entities;
using BusinessLogic.Validation;
using BusinessLogic.Validation.Validators;
using FluentValidation;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms;
using TestHelpers;

namespace IntegrationTest.Forms;

[TestFixture]
public class ValidationWrapperIt
{
    [Test]
    public async Task LearningWorld_WithLearningWorldValidator_ValidEntity_IsValidated()
    {
        var namesProvider = Substitute.For<ILearningWorldNamesProvider>();
        var validator = new LearningWorldValidator(namesProvider);
        var entity = EntityProvider.GetLearningWorld();
        
        var sut = GetSystemUnderTest(validator);

        Assert.That(await sut.ValidateAsync(entity, "Name"), Is.Empty);
        Assert.That(await sut.ValidateAsync(entity, "Shortname"), Is.Empty);
    }

    [Test]
    public async Task LearningWorld_WithLearningWorldValidator_InvalidEntity_GivesErrors()
    {
        var namesProvider = Substitute.For<ILearningWorldNamesProvider>();
        namesProvider.WorldNames.Returns(new[]{(new Guid(), "a")});
        namesProvider.WorldShortnames.Returns(new[]{(new Guid(), "b")});
        var validator = new LearningWorldValidator(namesProvider);
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