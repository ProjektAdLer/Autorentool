using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Presentation.Components.Forms;
using Shared;

namespace PresentationTest.Components.Forms;

[TestFixture]
public class NpcMoodPreviewHandlerUt
{
    [Test]
    public void GetIconForNpcAndMood_NotANpcElementModel_ThrowsArgumentException()
    {
        var notNpcElementModels = Enum.GetValues<ElementModel>()
            .Where(model => !model.ToString().Contains("a_npc")).ToList();
        Assert.Multiple(() =>
        {
            foreach (var elementModel in notNpcElementModels)
            {
                Assert.Throws<ArgumentException>(() =>
                    NpcMoodPreviewHandler.GetIconForNpcAndMood(elementModel, NpcMood.Welcome));
            }
        });
    }

    [Test]
    public void GetIconForNpcAndMood_AnNpcElementModel_DoNotThrowArgumentException()
    {
        var npcElementModels = Enum.GetValues<ElementModel>()
            .Where(model => model.ToString().Contains("a_npc")).ToList();
        Assert.Multiple(() =>
        {
            foreach (var elementModel in npcElementModels)
            {
                Assert.DoesNotThrow(() =>
                    NpcMoodPreviewHandler.GetIconForNpcAndMood(elementModel, NpcMood.Welcome));
            }
        });
    }

    [Test]
    public void GetIconForNpcAndMood_EachValidCombination_ReturnsIconPath([Values] NpcMood npcMood)
    {
        var npcElementModels = Enum.GetValues<ElementModel>()
            .Where(model => model.ToString().Contains("a_npc")).ToList();

        Assert.Multiple(() =>
        {
            foreach (var elementModel in npcElementModels)
            {
                var testElementModel = ElementModelHelper.IsObsolete(elementModel)
                    ? ElementModelHelper.GetAlternateValue(elementModel)
                    : elementModel;

                string iconPath = null!;
                Assert.DoesNotThrow(() => iconPath =
                    NpcMoodPreviewHandler.GetIconForNpcAndMood(testElementModel, npcMood));
                Assert.Multiple(() =>
                {
                    Assert.That(iconPath, Is.Not.Null);
                    Assert.That(iconPath, Is.Not.Empty);
                    Assert.That(iconPath, Contains.Substring(testElementModel.ToString().Replace("_", "-")));
                });
            }
        });
    }

    [Test]
    [TestCaseSource(nameof(NpcElementModelsAndMoods))]
    public void GetIconForNpcAndMood_EachValidCombination_PathExists(ElementModel elementModel, NpcMood npcMood)
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var projectDirectory = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName;
        var iconDirectory = Path.Combine(projectDirectory!, "AuthoringTool", "wwwroot");

        Assert.Multiple(() =>
        {
            var testElementModel = ElementModelHelper.IsObsolete(elementModel)
                ? ElementModelHelper.GetAlternateValue(elementModel)
                : elementModel;

            string iconPath = null!;
            Assert.DoesNotThrow(() => iconPath =
                NpcMoodPreviewHandler.GetIconForNpcAndMood(testElementModel, npcMood));
            Assert.That(iconPath, Is.Not.Null);
            Assert.That(iconPath, Is.Not.Empty);
            Assert.That(File.Exists(Path.Combine(iconDirectory, iconPath)), Is.True,
                $"Icon file does not exist for enum value {testElementModel}. " +
                $"The returned path is {iconPath}");
        });
    }
    
    public static IEnumerable<object[]> NpcElementModelsAndMoods()
    {
        var npcModels = Enum.GetValues<ElementModel>()
            .Where(e => e.ToString().StartsWith("a_npc"));
    
        var moods = Enum.GetValues<NpcMood>();
    
        return from model in npcModels
            from mood in moods
            select new object[] { model, mood };
    }
    
}