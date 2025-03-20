using System;
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
        npcElementModels.Remove(ElementModel.a_npc_defaultnpc);
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
    public void GetIconForNpcAndMood_EachValidCombination_ReturnsIconPath()
    {
        var npcElementModels = Enum.GetValues<ElementModel>()
            .Where(model => model.ToString().Contains("a_npc")).ToList();
        npcElementModels.Remove(ElementModel.a_npc_defaultnpc);
        Assert.Multiple(() =>
        {
            foreach (var elementModel in npcElementModels)
            {
                foreach (var npcMood in Enum.GetValues<NpcMood>())
                {
                    string iconPath = null!;
                    Assert.DoesNotThrow(() => iconPath =
                        NpcMoodPreviewHandler.GetIconForNpcAndMood(elementModel, npcMood));
                    Assert.Multiple(() =>
                    {
                        Assert.That(iconPath, Is.Not.Null);
                        Assert.That(iconPath, Is.Not.Empty);
                        Assert.That(iconPath, Contains.Substring(elementModel.ToString().Replace("_", "-")));
                    });
                }
            }
        });
    }
    
    [Test]
    public void GetIconForNpcAndMood_EachValidCombination_PathExists()
    {
        var npcElementModels = Enum.GetValues<ElementModel>()
            .Where(model => model.ToString().Contains("a_npc")).ToList();
        npcElementModels.Remove(ElementModel.a_npc_defaultnpc);
        Assert.Multiple(() =>
        {
            foreach (var elementModel in npcElementModels)
            {
                foreach (var npcMood in Enum.GetValues<NpcMood>())
                {
                    var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    var projectDirectory = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName;
                    var iconDirectory = Path.Combine(projectDirectory!, "AuthoringTool", "wwwroot");
                    
                    string iconPath = null!;
                    Assert.DoesNotThrow(() => iconPath =
                        NpcMoodPreviewHandler.GetIconForNpcAndMood(elementModel, npcMood));
                    Assert.That(iconPath, Is.Not.Null);
                    Assert.That(iconPath, Is.Not.Empty);
                    Assert.That(File.Exists(Path.Combine(iconDirectory, iconPath)), Is.True,
                        $"Icon file does not exist for enum value {elementModel}. " +
                        $"The returned path is {iconPath}");

                }
            }
        });
    }
    
}