using System.Diagnostics;
using System.Reflection;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms;
using Shared;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components.Forms;

[TestFixture]
public class NpcMoodSelectUt
{
    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();

        _localizer = Substitute.For<IStringLocalizer<NpcMoodSelect>>();
        _localizer[Arg.Any<string>()].Returns(ci => new LocalizedString(ci.Arg<string>(), ci.Arg<string>()));
        _localizer[Arg.Any<string>(), Arg.Any<object[]>()].Returns(ci =>
            new LocalizedString(ci.Arg<string>() + string.Concat(ci.Arg<object[]>()),
                ci.Arg<string>() + string.Concat(ci.Arg<object[]>())));

        _helperLocalizer = Substitute.For<IStringLocalizer<NpcMood>>();

        NpcMoodHelper.Initialize(_helperLocalizer);

        _testContext.Services.AddSingleton(_localizer);
        _testContext.AddMudBlazorTestServices();

        _testContext.RenderComponent<MudPopoverProvider>();
    }

    [TearDown]
    public void TearDown()
    {
        _testContext.Dispose();
    }

    private TestContext _testContext;
    private IStringLocalizer<NpcMoodSelect> _localizer;
    private IStringLocalizer<NpcMood> _helperLocalizer;

    [Test]
    public void Constructor_InjectsDependencies()
    {
        const ElementModel initialElementModel = ElementModel.a_npc_sheriffjustice;
        const NpcMood initialNpcMood = NpcMood.Tired;
        var systemUnderTest = GetRenderedComponent(initialElementModel, initialNpcMood);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.Localizer, Is.EqualTo(_localizer));
            Assert.That(systemUnderTest.Instance.ElementModel, Is.EqualTo(initialElementModel));
            Assert.That(systemUnderTest.Instance.Value, Is.EqualTo(initialNpcMood));
        });
    }

    [Test]
    public void Select_ChangesValue()
    {
        const ElementModel initialElementModel = ElementModel.a_npc_sheriffjustice;
        const NpcMood initialNpcMood = NpcMood.Tired;
        var systemUnderTest = GetRenderedComponent(initialElementModel, initialNpcMood);

        const NpcMood newNpcMood = NpcMood.Happy;
        var mudSelect = systemUnderTest.FindComponent<MudSelect<NpcMood>>();

        Assert.That(mudSelect.Instance, Is.Not.Null);

        mudSelect.InvokeAsync(() => mudSelect.Instance.ValueChanged.InvokeAsync(newNpcMood));

        Assert.That(systemUnderTest.Instance.Value, Is.EqualTo(newNpcMood));
    }

    private IRenderedComponent<NpcMoodSelect> GetRenderedComponent(
        ElementModel elementModel = ElementModel.a_npc_defaultdark_female, NpcMood npcMood = NpcMood.Welcome)
    {
        return _testContext.RenderComponent<NpcMoodSelect>(builder =>
        {
            builder.Add(p => p.ElementModel, elementModel);
            builder.Add(p => p.Value, npcMood);
        });
    }
}