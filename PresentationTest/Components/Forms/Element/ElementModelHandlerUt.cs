using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms.Element;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningContent.FileContent;
using Shared;

namespace PresentationTest.Components.Forms.Element;

[TestFixture]
public class ElementModelHandlerUt
{
    [Test]
    public void GetElementModels_TypeTextThemeCampus_AdaptivityModeFalse_ReturnsTextElementModels()
    {
        var systemUnderTest = new ElementModelHandler();
        var learningContent = Substitute.For<IFileContentViewModel>();
        learningContent.Type.Returns("txt");
        var elementModels = systemUnderTest.GetElementModels(learningContent, Theme.Campus, false);
        var enumerable = elementModels.ToList();
        Assert.That(enumerable, Is.Not.Null);
        Assert.That(enumerable, Is.Not.Empty);
        Assert.That(enumerable.First(), Is.EqualTo(ElementModel.l_random));
    }

    [Test]
    public void GetElementModels_TypeTextThemeCampus_AdaptivityModeTrue_ReturnsTextElementModels()
    {
        var systemUnderTest = new ElementModelHandler();
        var learningContent = Substitute.For<IFileContentViewModel>();
        learningContent.Type.Returns("txt");
        var elementModels = systemUnderTest.GetElementModels(learningContent, Theme.Campus, true);
        var expectedModels = new[]
        {
            ElementModel.a_npc_alerobot
        };

        Assert.That(elementModels, Is.EquivalentTo(expectedModels));
    }

    [Test]
    public void GetIconForElementModel_EachEnumValue_ReturnsIconPath()
    {
        var systemUnderTest = new ElementModelHandler();
        var elementModels = (ElementModel[])Enum.GetValues(typeof(ElementModel));
        foreach (var elementModel in elementModels)
        {
            Assert.That(systemUnderTest.GetIconForElementModel(elementModel), Is.Not.Null);
        }
    }

    [Test]
    public void GetIconForElementModel_UnknownEnumValue_ThrowsException()
    {
        var systemUnderTest = new ElementModelHandler();
        var unknownEnumValue = Enum.GetValues(typeof(ElementModel)).Length;
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            systemUnderTest.GetIconForElementModel((ElementModel)unknownEnumValue));
    }

    [Test]
    public void GetIconForElementModel_EachEnumValue_ReturnedIconPathExists()
    {
        var systemUnderTest = new ElementModelHandler();
        var elementModels = (ElementModel[])Enum.GetValues(typeof(ElementModel));

        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var projectDirectory = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName;
        var iconDirectory = Path.Combine(projectDirectory, "AuthoringTool", "wwwroot");

        foreach (var elementModel in elementModels)
        {
            var iconPath = systemUnderTest.GetIconForElementModel(elementModel);
            Assert.That(File.Exists(Path.Combine(iconDirectory, iconPath)), Is.True,
                $"Icon file does not exist for enum value: {elementModel}");
        }
    }

    [Test]
    public void GetElementModelsForType_ContainsEachElementModel()
    {
        var elementModelsFromAllTypes = ElementModelHandler.GetElementModelsForModelType(ContentTypeEnum.H5P)
            .Concat(ElementModelHandler.GetElementModelsForModelType(ContentTypeEnum.Text))
            .Concat(ElementModelHandler.GetElementModelsForModelType(ContentTypeEnum.Image))
            .Concat(ElementModelHandler.GetElementModelsForModelType(ContentTypeEnum.Video))
            .Concat(ElementModelHandler.GetElementModelsForModelType(ContentTypeEnum.Adaptivity))
            .ToList();
        var elementModels = ((ElementModel[])Enum.GetValues(typeof(ElementModel))).Except(new[]
        {
            ElementModel.a_npc_defaultnpc, ElementModel.a_npc_dozentlukas, ElementModel.a_npc_sheriffjustice
        });
        elementModels = elementModels.Where(elementModel => elementModel != ElementModel.l_random).ToArray();

        // Assert.That(elementModelsFromAllTypes.Count, Is.GreaterThanOrEqualTo(elementModels.Length));
        Assert.That(elementModels, Is.EquivalentTo(elementModelsFromAllTypes));
    }

    [Test]
    public void GetElementModelsForTheme_ContainsCaseForEachTheme()
    {
        var systemUnderTest = new ElementModelHandler();
        var themes = (Theme[])Enum.GetValues(typeof(Theme));
        foreach (var theme in themes)
        {
            Assert.DoesNotThrow(() =>
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                ElementModelHandler.GetElementModelsForTheme(theme).ToList());
        }
    }

    [Test]
    public void GetElementModelsForTheme_UnknownValueForTheme_ThrowsException()
    {
        var systemUnderTest = new ElementModelHandler();
        var unknownEnumValue = Enum.GetValues(typeof(Theme)).Length;
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            ElementModelHandler.GetElementModelsForTheme((Theme)unknownEnumValue).ToList());
    }

    [Test]
    public void GetElementModelsForTheme_ContainsEachElementModel()
    {
        var systemUnderTest = new ElementModelHandler();

        var themes = (Theme[])Enum.GetValues(typeof(Theme));

        var elementModelsFromAllThemes = new List<ElementModel>();
        foreach (var theme in themes)
        {
            elementModelsFromAllThemes.AddRange(ElementModelHandler.GetElementModelsForTheme(theme));
        }

        var adaptivityNpcs = systemUnderTest.GetElementModels(null, null, true);
        elementModelsFromAllThemes = elementModelsFromAllThemes.Union(adaptivityNpcs).ToList();

        var elementModels = (ElementModel[])Enum.GetValues(typeof(ElementModel));
        elementModels = elementModels.Where(elementModel => elementModel != ElementModel.l_random).ToArray();

        Assert.That(elementModels, Is.EquivalentTo(elementModelsFromAllThemes));
    }
}