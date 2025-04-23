using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Presentation.Components.Forms.Element;
using Shared;
using Shared.Theme;

namespace PresentationTest.Components.Forms.Element;

[TestFixture]
public class ElementModelHandlerUt
{
    [Test]
    public void GetElementModels_TypeTextThemeCampus_AdaptivityModeFalse_ReturnsTextElementModels()
    {
        var systemUnderTest = new ElementModelHandler();
        var elementModels =
            systemUnderTest.GetElementModels(ElementModelContentType.File, "txt", WorldTheme.CampusAschaffenburg);
        var enumerable = elementModels.ToList();
        Assert.That(enumerable, Is.Not.Null);
        Assert.That(enumerable, Is.Not.Empty);
        Assert.That(enumerable.First(), Is.EqualTo(ElementModel.l_random));
    }

    [Test]
    public void GetElementModels_TypeTextThemeCampus_AdaptivityModeTrue_ReturnsTextElementModels()
    {
        var systemUnderTest = new ElementModelHandler();
        var elementModels =
            systemUnderTest.GetElementModels(ElementModelContentType.Adaptivity, "txt", WorldTheme.CampusAschaffenburg);
        var expectedModels = new[]
        {
            ElementModel.a_npc_alerobot
        };

        Assert.That(elementModels, Is.EquivalentTo(expectedModels));
    }

    [Test]
    public void GetIconForElementModel_ReturnsIconPath([Values] ElementModel elementModel)
    {
        var systemUnderTest = new ElementModelHandler();
        string iconPath = null!;
        Assert.DoesNotThrow(() => { iconPath = systemUnderTest.GetIconForElementModel(elementModel); },
            $"The model {elementModel} might not have added to the methode {nameof(ElementModelHandler.GetIconForElementModel)} in {nameof(ElementModelHandler)}.");
        Assert.That(iconPath, Is.Not.Null);
        Assert.That(iconPath, Is.Not.Empty);
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
    public void GetIconForElementModel_ReturnedIconPathExists([Values] ElementModel elementModel)
    {
        var systemUnderTest = new ElementModelHandler();

        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var projectDirectory = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName;
        var iconDirectory = Path.Combine(projectDirectory!, "AuthoringTool", "wwwroot");

        string iconPath = null!;
        Assert.DoesNotThrow(() => { iconPath = systemUnderTest.GetIconForElementModel(elementModel); },
            $"See test {nameof(GetIconForElementModel_ReturnsIconPath)}");
        Assert.That(File.Exists(Path.Combine(iconDirectory, iconPath)), Is.True,
            $"Icon file does not exist for enum value {elementModel}. " +
            $"The path is set to {iconPath} in {nameof(ElementModelHandler.GetIconForElementModel)} in {nameof(ElementModelHandler)}.");
    }

    [Test]
    public void GetElementModelsForType_ContainsEachElementModel()
    {
        var elementModelsFromAllTypes = ElementModelHandler.GetElementModelsForModelType(ContentTypeEnum.H5P)
            .Concat(ElementModelHandler.GetElementModelsForModelType(ContentTypeEnum.Text))
            .Concat(ElementModelHandler.GetElementModelsForModelType(ContentTypeEnum.Image))
            .Concat(ElementModelHandler.GetElementModelsForModelType(ContentTypeEnum.Video))
            .Concat(ElementModelHandler.GetElementModelsForModelType(ContentTypeEnum.Adaptivity))
            .Concat(ElementModelHandler.GetElementModelsForModelType(ContentTypeEnum.Story))
            .ToList();
        var elementModels = (ElementModel[])Enum.GetValues(typeof(ElementModel));
        elementModels = elementModels.Where(elementModel => elementModel != ElementModel.l_random).ToArray();

        Assert.Multiple(() =>
        {
            Assert.That(elementModelsFromAllTypes.Count, Is.GreaterThanOrEqualTo(elementModels.Length));
            foreach (var elementModel in elementModels)
            {
                Assert.That(elementModelsFromAllTypes.Contains(elementModel), Is.True,
                    $"ElementModel {elementModel} is not assigned to any type");
            }
        });
    }

    [Test]
    public void GetElementModelsForTheme_ContainsCaseForEachTheme([Values] WorldTheme worldTheme)
    {
        Assert.DoesNotThrow(() => _ = ElementModelHandler.GetElementModelsForTheme(worldTheme).ToList());
    }

    [Test]
    public void GetElementModelsForTheme_UnknownValueForTheme_ThrowsException()
    {
        var unknownEnumValue = Enum.GetValues(typeof(WorldTheme)).Length;
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            _ = ElementModelHandler.GetElementModelsForTheme((WorldTheme)unknownEnumValue).ToList());
    }
}